namespace Dyvenix.App1.Common.Shared.Exceptions;

public class HttpDeserializationException : Exception
{
	public HttpDeserializationException() : base() { }

	public HttpDeserializationException(string message) : base(message) { }

	public HttpDeserializationException(string message, Exception innerException) : base(message, innerException) { }
}
