using Dyvenix.System.Apis.Extensions;
using Dyvenix.System.Shared.DTOs;
using Dyvenix.System.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Dyvenix.System.Apis.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
	private readonly ILogger<GlobalExceptionFilter> _logger;

	public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		var (sourceClass, sourceMethod) = GetSourceFromPath(context.HttpContext.Request.Path);

		_logger.Error(context.Exception, sourceClass, sourceMethod);

		var statusCode = MapExceptionToStatusCode(context.Exception);
		var apiResponse = ApiResponse.Fail(statusCode, context.Exception.GetBaseException().Message);

		context.Result = new ObjectResult(apiResponse)
		{
			StatusCode = statusCode
		};

		context.ExceptionHandled = true;
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
