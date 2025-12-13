using Dyvenix.System.Apis.Extensions;
using Dyvenix.System.Shared.DTOs;
using Dyvenix.System.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dyvenix.System.Apis.Filters;

/// <summary>
/// Global exception filter that handles unhandled exceptions from controllers.
/// </summary>
public partial class GlobalExceptionFilter : IExceptionFilter
{
	private readonly ILogger<GlobalExceptionFilter> _logger;

	// Namespace prefixes to skip when walking the stack trace
	private static readonly string[] SkipNamespacePrefixes =
	[
		"System",
		"Microsoft",
		"OpenTelemetry",
		"Yarp",
		"NLog"
	];

	// Regex to extract method name from async state machine class name (e.g., "<Health>d__3" -> "Health")
	[GeneratedRegex(@"<(\w+)>")]
	private static partial Regex AsyncMethodNameRegex();

	public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		var (sourceClass, sourceMethod) = GetSourceFromException(context.Exception);

		// Fallback to URL path if we couldn't find source from stack trace
		if (string.IsNullOrEmpty(sourceClass))
		{
			(sourceClass, sourceMethod) = GetSourceFromPath(context.HttpContext.Request.Path);
		}

		_logger.Error(context.Exception, sourceClass, sourceMethod);

		var statusCode = MapExceptionToStatusCode(context.Exception);
		var apiResponse = ApiResponse.Fail(statusCode, context.Exception.GetBaseException().Message);

		context.Result = new ObjectResult(apiResponse)
		{
			StatusCode = statusCode
		};

		context.ExceptionHandled = true;
	}

	private static (string sourceClass, string sourceMethod) GetSourceFromException(Exception ex)
	{
		var stackTrace = new StackTrace(ex, false);
		var frames = stackTrace.GetFrames();

		if (frames == null || frames.Length == 0)
			return (string.Empty, string.Empty);

		// Find the first frame that's from user code (not system/framework)
		foreach (var frame in frames)
		{
			var method = frame.GetMethod();
			if (method == null)
				continue;

			var declaringType = method.DeclaringType;
			if (declaringType == null)
				continue;

			var namespaceName = declaringType.Namespace ?? string.Empty;

			// Skip empty namespaces
			if (string.IsNullOrEmpty(namespaceName))
				continue;

			// Skip system/framework namespaces
			if (ShouldSkipNamespace(namespaceName))
				continue;

			// Found user code - extract class and method name
			var (className, methodName) = ExtractClassAndMethod(declaringType, method.Name);
			return (className, methodName);
		}

		// Couldn't find user code frame
		return (string.Empty, string.Empty);
	}

	private static (string className, string methodName) ExtractClassAndMethod(Type declaringType, string methodName)
	{
		var className = declaringType.Name;

		// Check if this is an async state machine (e.g., "<Health>d__3")
		if (className.StartsWith('<') && methodName == "MoveNext")
		{
			// Extract original method name from state machine class name
			var match = AsyncMethodNameRegex().Match(className);
			if (match.Success)
			{
				methodName = match.Groups[1].Value;
			}

			// Get the actual class name from the declaring type's declaring type
			if (declaringType.DeclaringType != null)
			{
				className = declaringType.DeclaringType.Name;
			}
		}

		return (className, methodName);
	}

	private static bool ShouldSkipNamespace(string namespaceName)
	{
		foreach (var prefix in SkipNamespacePrefixes)
		{
			if (namespaceName.StartsWith(prefix, StringComparison.Ordinal))
				return true;
		}
		return false;
	}

	private static (string sourceClass, string sourceMethod) GetSourceFromPath(string path)
	{
		// Remove leading slash and split by '/'
		var segments = path.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

		if (segments.Length >= 2)
		{
			// Use last 2 segments: e.g., "/api/auth/v1/users/get" -> ("users", "get")
			return (segments[^2], segments[^1]);
		}
		else if (segments.Length == 1)
		{
			return (segments[0], string.Empty);
		}

		return (string.Empty, string.Empty);
	}

	private static int MapExceptionToStatusCode(Exception ex)
	{
		if (ex is ValidationException)
			return 400;

		// Default to 500
		return 500;
	}
}
