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
using Dyvenix.App1.App.Shared.Authorization;
using Dyvenix.App1.App.Shared.Dtos;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class ClientEndpoints
{
	public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/client")
			.WithTags("Client");
		
		// Update
		
		group.MapPost("CreateClient", CreateClient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPatch("UpdateClient", UpdateClient)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPatch("UpdateClientBaseUrl", UpdateClientBaseUrl)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Delete
		
		group.MapDelete("DeleteClient", DeleteClient)
			.Produces<Guid>(StatusCodes.Status200OK);
		
		// Read - Single
		
		group.MapGet("GetClientById/{id}", GetClientById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetClientByKey/{key}", GetClientByKey)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - List
		
		group.MapPost("GetAllClientLookupItems", GetAllClientLookupItems)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapGet("GetAllClientRoutes", GetAllClientRoutes)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("GetAllClients", GetAllClients)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPost("SearchClientsByName", SearchClientsByName)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
	
		return app;
	}
	
	#region Delete
	
	public static async Task<Result> DeleteClient(IClientService clientService, [FromBody] DeleteReq deleteReq)
	{
		await clientService.DeleteClient(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result<byte[]>> CreateClient(IClientService clientService, [FromBody] CreateClientReq request)
	{
		var rowVersion = await clientService.CreateClient(request);
		return Result<byte[]>.Ok(rowVersion);
	}
	
	
	public static async Task<Result<byte[]>> UpdateClient(IClientService clientService, [FromBody] UpdateClientReq request)
	{
		var rowVersion = await clientService.UpdateClient(request);
		return Result<byte[]>.Ok(rowVersion);
	}
	
	
	public static async Task<Result<byte[]>> UpdateClientBaseUrl(IClientService clientService, [FromBody] UpdateClientBaseUrlReq request)
	{
		var rowVersion = await clientService.UpdateClientBaseUrl(request);
		return Result<byte[]>.Ok(rowVersion);
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<ClientDto>> GetClientById(IClientService clientService, Guid id)
	{
		var clientDto = await clientService.GetClientById(id);
		return Result<ClientDto>.Ok(clientDto);
	}
	
	public static async Task<Result<ClientDto>> GetClientByKey(IClientService clientService, string key)
	{
		var clientDto = await clientService.GetClientByKey(key);
		return Result<ClientDto>.Ok(clientDto);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<IReadOnlyList<ClientLookupDto>>> GetAllClientLookupItems(IClientService clientService, GetAllClientLookupItemsReq getAllClientLookupItemsReq)
	{
		var data = await clientService.GetAllClientLookupItems(getAllClientLookupItemsReq);
		return Result<IReadOnlyList<ClientLookupDto>>.Ok(data);
	}
	
	public static async Task<Result<IReadOnlyList<ClientRouteDto>>> GetAllClientRoutes(IClientService clientService)
	{
		var data = await clientService.GetAllClientRoutes();
		return Result<IReadOnlyList<ClientRouteDto>>.Ok(data);
	}
	
	public static async Task<Result<IReadOnlyList<ClientDto>>> GetAllClients(IClientService clientService, GetAllClientsReq getAllClientsReq)
	{
		var data = await clientService.GetAllClients(getAllClientsReq);
		return Result<IReadOnlyList<ClientDto>>.Ok(data);
	}
	
	public static async Task<Result<ListPage<ClientLookupDto>>> SearchClientsByName(IClientService clientService, SearchClientsByNameReq searchClientsByNameReq)
	{
		var data = await clientService.SearchClientsByName(searchClientsByNameReq);
		return Result<ListPage<ClientLookupDto>>.Ok(data);
	}

	#endregion
}
