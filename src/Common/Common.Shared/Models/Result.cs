namespace Dyvenix.App1.Common.Shared.Models;

/// <summary>
/// Represents the outcome of an operation that can succeed with a value or fail with an error.
/// </summary>
public class Result<T>
{
	private Result() { }

	public bool IsSuccess { get; private init; }
	public bool IsFailure => !IsSuccess;
	public T? Data { get; private init; }
	public ResultError? Error { get; private init; }

	public static Result<T> Ok(T value) => new()
	{
		IsSuccess = true,
		Data = value
	};

	public static Result<T> NotFound(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.NotFound, message ?? "Resource not found")
	};

	public static Result<T> Validation(string message) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Validation, message)
	};

	public static Result<T> Validation(Dictionary<string, string[]> fieldErrors) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Validation, "Validation failed", fieldErrors)
	};

	public static Result<T> Conflict(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Conflict, message ?? "Resource conflict")
	};

	public static Result<T> Forbidden(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Forbidden, message ?? "Access denied")
	};

	public static Result<T> Failure(string message) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Failure, message)
	};

	/// <summary>
	/// Pattern-match on success/failure.
	/// </summary>
	public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<ResultError, TOut> onFailure)
		=> IsSuccess ? onSuccess(Data!) : onFailure(Error!);
}

/// <summary>
/// Represents the outcome of an operation that can succeed (no value) or fail with an error.
/// </summary>
public class Result
{
	private Result() { }

	public bool IsSuccess { get; private init; }
	public bool IsFailure => !IsSuccess;
	public ResultError? Error { get; private init; }

	public static Result Ok() => new() { IsSuccess = true };

	public static Result NotFound(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.NotFound, message ?? "Resource not found")
	};

	public static Result Validation(string message) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Validation, message)
	};

	public static Result Validation(Dictionary<string, string[]> fieldErrors) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Validation, "Validation failed", fieldErrors)
	};

	public static Result Conflict(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Conflict, message ?? "Resource conflict")
	};

	public static Result Forbidden(string? message = null) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Forbidden, message ?? "Access denied")
	};

	public static Result Failure(string message) => new()
	{
		IsSuccess = false,
		Error = new ResultError(ResultErrorKind.Failure, message)
	};
}

public enum ResultErrorKind
{
	Validation,
	NotFound,
	Conflict,
	Forbidden,
	Failure
}

public record ResultError(
	ResultErrorKind Kind,
	string Message,
	Dictionary<string, string[]>? FieldErrors = null
);
