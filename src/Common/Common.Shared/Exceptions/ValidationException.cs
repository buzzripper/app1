namespace Dyvenix.App1.Common.Shared.Exceptions;

public class ValidationException : Exception
{
	public ValidationException(string message, Dictionary<string, string[]> fieldErrors)
		: base(message)
	{
		this.FieldErrors = fieldErrors;
	}

	public Dictionary<string, string[]>? FieldErrors;
}
