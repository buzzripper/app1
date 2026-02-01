using Dyvenix.App1.App.Api.Extensions;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dyvenix.App1.App.Api.Controllers;

public static class PatientEndpoints
{
	public static IEndpointRouteBuilder MapPatientEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/patient")
			.WithTags("Patient");

		group.MapGet("{id:guid}", GetById)
			.WithName("GetPatientById")
			.Produces<Patient>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		return app;
	}

	private static async Task<IResult> GetById(Guid id, IPatientService patientService)
	{
		var result = await patientService.GetById(id);
		return result.ToHttpResult();
	}
}
