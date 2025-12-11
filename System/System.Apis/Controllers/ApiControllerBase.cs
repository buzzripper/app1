using Dyvenix.System.Shared.DTOs;
using Dyvenix.System.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.System.Apis.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
	protected async Task<IActionResult> Execute<T>(Func<Task<T>> action)
	{
		try
		{
			var result = await action();

			return Ok(new ApiResponse
			{
				Success = true,
				Data = result
			});
		}
		catch (ValidationException ex)
		{
			return Ok(new ApiResponse
			{
				Success = false,
				Error = new ApiError
				{
					Code = "VALIDATION",
					Message = ex.Message
				}
			});
		}
		catch (NotFoundException ex)
		{
			return Ok(new ApiResponse
			{
				Success = false,
				Error = new ApiError
				{
					Code = "NOT_FOUND",
					Message = ex.Message
				}
			});
		}
		catch (Exception ex)
		{
			return Ok(new ApiResponse
			{
				Success = false,
				Error = new ApiError
				{
					Code = "SERVER_ERROR",
					Message = "An unexpected error occurred.",
					Details = ex.Message
				}
			});
		}
	}
}
