namespace App1.System.Shared.DTOs;

public class ApiResponseDyv
{
	#region Static

	public static ApiResponse Succeed(string correlationId = null)
	{
		return new ApiResponse
		{
			Success = true,
			ErrorCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	public static ApiResponse Fail(int errorCode, string message)
	{
		return new ApiResponse
		{
			Success = false,
			ErrorCode = errorCode,
			Message = message
		};
	}

	#endregion

	#region Ctors / Init

	public ApiResponse()
	{
		Success = true;
		ErrorCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message) : this()
	{
		Message = message;
	}

	public ApiResponse(int statusCode, string message)
	{
		ErrorCode = statusCode;
		Message = message;
	}

	public ApiResponse(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public ApiResponse(int statusCode, string message, string correlationId)
	{
		ErrorCode = statusCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#endregion

	#region Properties

	public bool Success { get; set; }
	public int ErrorCode { get; set; }
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
			ErrorCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	new public static ApiResponse<T> Fail(int errorCode, string message)
	{
		return new ApiResponse<T>
		{
			Success = false,
			ErrorCode = errorCode,
			Message = message
		};
	}

	#endregion

	public ApiResponse()
	{
		ErrorCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message)
	{
		Message = message;
	}

	public ApiResponse(int errorCode, string message)
	{
		ErrorCode = errorCode;
		Message = message;
	}

	public ApiResponse(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public ApiResponse(int errorCode, string message, string correlationId)
	{
		ErrorCode = errorCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#region Properties

	public T Data { get; set; }

	#endregion
}

