//using Dyvenix.System.Shared.DTOs;
//using Dyvenix.System.Shared.Exceptions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using Dyvenix.System.Apis.Extensions;

//namespace Dyvenix.System.Apis.Controllers;

//[ApiController]
//public abstract class ApiControllerBase : ControllerBase
//{
//	#region Fields

//	protected ILogger _logger;

//	#endregion

//	#region Ctor

//	protected ApiControllerBase(ILogger logger)
//	{
//		_logger = logger;
//	}

//	#endregion

//	[MethodImpl(MethodImplOptions.NoInlining)]
//	protected IActionResult CallService(Action action)
//	{
//		var apiResponse = new ApiResponse();
//		try
//		{
//			action();
//			return Ok(ApiResponse.Succeed());
//		}
//		catch (Exception ex)
//		{
//			return this.HandleException(ex);
//		}
//	}

//	[MethodImpl(MethodImplOptions.NoInlining)]
//	protected async Task<IActionResult> CallServiceAsync(Func<Task> func)
//	{
//		var apiResponse = new ApiResponse();
//		try
//		{
//			await func();
//			apiResponse.ErrorCode = 200;
//			return Ok(apiResponse);
//		}
//		catch (Exception ex)
//		{
//			return this.HandleException(ex);
//		}
//	}

//	private IActionResult HandleException(Exception ex)
//	{
//		_logger.Error(ex);

//		var statusCode = MapExceptionToStatusCode(ex);
//		var apiResponse = ApiResponse.Fail(statusCode, ex.Message);

//		return StatusCode(statusCode, apiResponse);
//	}

//	private int MapExceptionToStatusCode(Exception ex)
//	{
//		if (ex is ValidationException)
//			return 400;

//		// Default to 500
//		return 500;
//	}
//}
