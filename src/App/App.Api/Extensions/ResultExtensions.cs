using Dyvenix.App1.Common.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace Dyvenix.App1.App.Api.Extensions;

public static class ResultExtensions
{
	/// <summary>
	/// Maps a Result&lt;T&gt; to an IResult for Minimal APIs.
	/// </summary>
	public static IResult ToHttpResult<T>(this Result<T> result)
	{
		if (result.IsSuccess)
			return Results.Ok(result.Value);

		return result.Error!.Kind switch
		{
			ResultErrorKind.NotFound => Results.NotFound(ToProblem(result.Error)),
			ResultErrorKind.Validation => result.Error.FieldErrors is not null
				? Results.ValidationProblem(result.Error.FieldErrors)
				: Results.BadRequest(ToProblem(result.Error)),
			ResultErrorKind.Conflict => Results.Conflict(ToProblem(result.Error)),
			ResultErrorKind.Forbidden => Results.Forbid(),
			_ => Results.Problem(result.Error.Message, statusCode: 500)
		};
	}

	/// <summary>
	/// Maps a non-generic Result to an IResult for Minimal APIs.
	/// </summary>
	public static IResult ToHttpResult(this Result result)
	{
		if (result.IsSuccess)
			return Results.Ok();

		return result.Error!.Kind switch
		{
			ResultErrorKind.NotFound => Results.NotFound(ToProblem(result.Error)),
			ResultErrorKind.Validation => result.Error.FieldErrors is not null
				? Results.ValidationProblem(result.Error.FieldErrors)
				: Results.BadRequest(ToProblem(result.Error)),
			ResultErrorKind.Conflict => Results.Conflict(ToProblem(result.Error)),
			ResultErrorKind.Forbidden => Results.Forbid(),
			_ => Results.Problem(result.Error.Message, statusCode: 500)
		};
	}

	private static object ToProblem(ResultError error) => new
	{
		type = $"https://httpstatuses.com/{GetStatusCode(error.Kind)}",
		title = error.Kind.ToString(),
		detail = error.Message
	};

	private static int GetStatusCode(ResultErrorKind kind) => kind switch
	{
		ResultErrorKind.NotFound => 404,
		ResultErrorKind.Validation => 400,
		ResultErrorKind.Conflict => 409,
		ResultErrorKind.Forbidden => 403,
		_ => 500
	};
}
