namespace Dyvenix.App1.Common.Shared.DTOs;

public class Response
{
	#region Static

	public static Response Succeed(string correlationId = null)
	{
		return new Response
		{
			IsSuccess = true,
			StatusCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	public static Response Fail(int errorCode, string message)
	{
		return new Response
		{
			IsSuccess = false,
			StatusCode = errorCode,
			Message = message
		};
	}

	#endregion

	#region Ctors / Init

	public Response()
	{
		IsSuccess = true;
		StatusCode = 0;
		Message = "Success";
	}

	public Response(string message) : this()
	{
		Message = message;
	}

	public Response(int statusCode, string message)
	{
		StatusCode = statusCode;
		Message = message;
	}

	public Response(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public Response(int statusCode, string message, string correlationId)
	{
		StatusCode = statusCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#endregion

	#region Properties

	public bool IsSuccess { get; init; }
	public int StatusCode { get; set; }
	public string Message { get; set; }
	public string Error { get; set; }
	public string CorrelationId { get; set; }

	#endregion
}

public class Response<T> : Response
{
	#region Static

	new public static Response<T> Succeed(string correlationId = null)
	{
		return new Response<T>
		{
			IsSuccess = true,
			StatusCode = 0,
			CorrelationId = correlationId,
			Message = "Success"
		};
	}

	new public static Response<T> Fail(int errorCode, string message)
	{
		return new Response<T>
		{
			IsSuccess = false,
			StatusCode = errorCode,
			Message = message
		};
	}

	#endregion

	public Response()
	{
		StatusCode = 0;
		Message = "Success";
	}

	public Response(string message)
	{
		Message = message;
	}

	public Response(int errorCode, string message)
	{
		StatusCode = errorCode;
		Message = message;
	}

	public Response(string message, string correlationId)
	{
		Message = message;
		CorrelationId = correlationId;
	}

	public Response(int errorCode, string message, string correlationId)
	{
		StatusCode = errorCode;
		Message = message;
		CorrelationId = correlationId;
	}

	#region Properties

	public T? Data { get; set; }

	#endregion
}

