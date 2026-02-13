namespace Dyvenix.App1.Common.Shared.Exceptions;

public class ConcurrencyException : ApiException
{
	#region Ctors / Init

	public ConcurrencyException() : base() { }

	public ConcurrencyException(string message) : base(message) { }

	public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }

	public ConcurrencyException(string message, string correlationId) : base(message, correlationId) { }

	public ConcurrencyException(string message, string correlationId, Exception innerException) : base(message, correlationId, innerException) { }

	#endregion

	protected override int GetStatusCode()
	{
		return 409;
	}
}
