namespace Dyvenix.App1.Common.Core.Exceptions;

public class NotFoundApiException : ApiException
{
	#region Ctors / Init

	public NotFoundApiException() : base() { }

	public NotFoundApiException(string message) : base(message) { }

	public NotFoundApiException(string message, Exception innerException) : base(message, innerException) { }

	public NotFoundApiException(string message, string correlationId) : base(message, correlationId) { }

	#endregion

	protected override int GetStatusCode()
	{
		return 404;
	}
}
