namespace Dyvenix.App1.Common.Shared.Exceptions;

public class NotFoundException : ApiException
{
	#region Ctors / Init

	public NotFoundException() : base() { }

	public NotFoundException(string message) : base(message) { }

	public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

	public NotFoundException(string message, string correlationId) : base(message, correlationId) { }

	#endregion

	protected override int GetStatusCode()
	{
		return 404;
	}
}
