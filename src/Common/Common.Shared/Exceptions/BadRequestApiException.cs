namespace Dyvenix.App1.Common.Shared.Exceptions;

public class BadRequestApiException : ApiException
{
	#region Ctors / Init

	public BadRequestApiException() : base() { }

	public BadRequestApiException(string message) : base(message) { }

	public BadRequestApiException(string message, Exception innerException) : base(message, innerException) { }

	public BadRequestApiException(string message, string correlationId) : base(message, correlationId) { }

	#endregion

	protected override int GetStatusCode()
	{
		return 400;
	}
}
