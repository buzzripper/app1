namespace Dyvenix.App1.Common.Shared.DTOs;

public class ApiResponse
{
	#region Static

	public static ApiResponse Succeed(string correlationId = null)
	{
		return new ApiResponse
		{
			Success = true,
			StatusCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	public static ApiResponse Fail(int errorCode, string message)
	{
		return new ApiResponse
		{
			Success = false,
			StatusCode = errorCode,
			Message = message
		};
	}

	#endregion

	#region Ctors / Init

	public ApiResponse()
	{
		Success = true;
		StatusCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message) : this()
	{
		Message = message;
	}

	public ApiResponse(int statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public ApiResponse(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public ApiResponse(int statusCode, string message, string correlationId)
	{
		StatusCode = statusCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#endregion

	#region Properties

	public bool Success { get; set; }
	public int StatusCode { get; set; }
	public string Message { get; set; }
	public string CorrelationId { get; set; }

	#endregion
}

public class ApiResponse<T> : ApiResponse
{
	#region Static

	new public static ApiResponse<T> Succeed(string correlationId = null)
	{
		return new ApiResponse<T>
		{
			Success = true,
			StatusCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	new public static ApiResponse<T> Fail(int errorCode, string message)
	{
		return new ApiResponse<T>
		{
			Success = false,
			StatusCode = errorCode,
			Message = message
		};
	}

	#endregion

	public ApiResponse()
	{
		StatusCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message)
	{
		Message = message;
	}

	public ApiResponse(int errorCode, string message)
	{
		StatusCode = errorCode;
		Message = message;
	}

	public ApiResponse(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public ApiResponse(int errorCode, string message, string correlationId)
	{
		StatusCode = errorCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#region Properties

	public T Data { get; set; }

	#endregion
}

