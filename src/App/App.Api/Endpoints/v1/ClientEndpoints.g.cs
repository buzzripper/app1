//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.App.Api.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Dtos;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class ClientEndpoints
{
	public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/client")
			.WithTags("Client");
		
		// Delete
		
		group.MapDelete("Delete", Delete)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPost("Create", Create)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetByKey/{key}", GetByKey)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - List
		
		group.MapPost("GetAllClientOptions", GetAllClientOptions)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetAllRoutes", GetAllRoutes)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
	
		return app;
	}
	
	#region Delete
	
	public static async Task<Result> Delete(IClientService clientService, [FromBody] DeleteReq deleteReq)
	{
		await clientService.Delete(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result<byte[]>> Create(IClientService clientService, [FromBody] CreateReq request)
	{
		var rowVersion = await clientService.Create(request);
		return Result<byte[]>.Ok(rowVersion);
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<ClientDto>> GetById(IClientService clientService, Guid id)
	{
		var clientDto = await clientService.GetById(id);
		return Result<ClientDto>.Ok(clientDto);
	}
	
	public static async Task<Result<ClientDto>> GetByKey(IClientService clientService, string key)
	{
		var clientDto = await clientService.GetByKey(key);
		return Result<ClientDto>.Ok(clientDto);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<IReadOnlyList<ClientOptionDto>>> GetAllClientOptions(IClientService clientService, GetAllClientOptionsReq getAllClientOptionsReq)
	{
		var data = await clientService.GetAllClientOptions(getAllClientOptionsReq);
		return Result<IReadOnlyList<ClientOptionDto>>.Ok(data);
	}
	
	public static async Task<Result<IReadOnlyList<ClientRouteDto>>> GetAllRoutes(IClientService clientService)
	{
		var data = await clientService.GetAllRoutes();
		return Result<IReadOnlyList<ClientRouteDto>>.Ok(data);
	}

	#endregion
}
