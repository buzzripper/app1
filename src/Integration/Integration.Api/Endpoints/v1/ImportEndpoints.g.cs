//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Integration.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Text;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class ImportEndpoints
{
	public static IEndpointRouteBuilder MapImportEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/integration/v1/import")
			.WithTags("Import")
			.RequireAuthorization(IntegrationPermissions.Read);
		
		group.MapGet("importmethod1", ImportMethod1)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		group.MapGet("importmethod2", ImportMethod2)
			.Produces<Guid>(StatusCodes.Status200OK);

		group.MapGet("importmethod3", ImportMethod3)
			.Produces<Guid>(StatusCodes.Status200OK);
	
		return app;
	}
	
	public static async Task<Result<string>> ImportMethod1(IImportService importService, ClaimsPrincipal user)
	{
        var sb = new StringBuilder();
        foreach (var claim in user.Claims.Select(c => $"{c.Type} = {c.Value}"))
            sb.AppendLine(claim);
        File.WriteAllText(@"D:\Active\Claims.txt", sb.ToString());

		var data = await importService.ImportMethod1();
		return Result<string>.Ok(data);
	}

	public static async Task<Result<string>> ImportMethod2(IImportService importService)
	{
		var data = await importService.ImportMethod2();
		return Result<string>.Ok(data);
	}

	public static async Task<Result<string>> ImportMethod3(IImportService importService)
	{
		var data = await importService.ImportMethod3();
		return Result<string>.Ok(data);
	}
}
