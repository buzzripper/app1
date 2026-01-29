using Dyvenix.App1.Common.Api.Logging;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dyvenix.App1.Common.Api.Filters;

/// <summary>
/// Global exception filter that handles unhandled exceptions from controllers.
/// </summary>
public abstract class SystemExceptionFilterBase<T> : IExceptionFilter
{
	protected static string _moduleName;

	private readonly IModuleLogger _logger;
	private readonly string _sourceClass;

	public SystemExceptionFilterBase(IModuleLogger logger)
	{
		_logger = logger;
		_sourceClass = typeof(T).Name;
	}

	public void OnException(ExceptionContext context)
	{
		var sourceMethod = GetSourceMethodFromPath(context.HttpContext.Request.Path);

		_logger.Error(context.Exception, _sourceClass, sourceMethod);

		var statusCode = MapExceptionToStatusCode(context.Exception);
		var apiResponse = ApiResponse.Fail(statusCode, context.Exception.GetBaseException().Message);

		context.Result = new ObjectResult(apiResponse)
		{
			StatusCode = statusCode
		};

		context.ExceptionHandled = true;
	}

	private static string GetSourceMethodFromPath(string path)
	{
		// Remove leading slash and split by '/'
		var segments = path.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

		// Use last segment for sourceMethod (endpoint) name
		if (segments.Length >= 1)
			return (segments[^1]);

		return string.Empty;
	}

	private static int MapExceptionToStatusCode(Exception ex)
	{
		if (ex is ValidationException)
			return 400;

		// Default to 500
		return 500;
	}
}
