namespace Dyvenix.App1.Common.Shared.Models;

public class ApiResponse
{
	#region Ctors / Init

	public ApiResponse()
	{
		StatusCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message)
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

	public int StatusCode { get; set; }
	public string Message { get; set; }
	public string CorrelationId { get; set; }

	#endregion
}

public class ApiResponse<T>
{
	public ApiResponse()
	{
		StatusCode = 0;
		Message = "Success";
	}

	public ApiResponse(string message)
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

	#region Properties

	public int StatusCode { get; set; }
	public string Message { get; set; }
	public string CorrelationId { get; set; }
	public T Data { get; set; }

	#endregion
}