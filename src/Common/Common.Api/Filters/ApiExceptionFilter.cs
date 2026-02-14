using Dyvenix.App1.Common.Api.Logging;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Dyvenix.App1.Common.Api.Filters;

/// <summary>
/// Global exception filter that handles unhandled exceptions from controllers.
/// </summary>
public class ApiExceptionFilter<T> : IExceptionFilter
{
	private readonly ILogger<T> _logger;
	private readonly string _sourceClass;

	public ApiExceptionFilter(ILogger<T> logger)
	{
		_logger = logger;
		_sourceClass = typeof(T).Name;
	}

	public void OnException(ExceptionContext context)
	{
		var sourceMethod = GetSourceMethodFromPath(context.HttpContext.Request.Path);

		_logger.Error(context.Exception, _sourceClass, sourceMethod);

		var result = MapExceptionToResult(context.Exception);

		context.Result = new ObjectResult(result)
		{
			StatusCode = (int)result.StatusCode
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

	private static Result MapExceptionToResult(Exception ex)
	{
		if (ex is ValidationException)
			return Result.Validation(ex.GetBaseException().Message);

		if (ex is UnauthorizedException)
			return Result.Validation(ex.GetBaseException().Message);

		if (ex is NotFoundException)
			return Result.NotFound(ex.GetBaseException().Message);

		if (ex is ConcurrencyException)
			return Result.Conflict(ex.GetBaseException().Message);

		if (ex is ConcurrencyException)
			return Result.Conflict(ex.GetBaseException().Message);

		return Result.Failure(ex.GetBaseException().Message);
	}
}
