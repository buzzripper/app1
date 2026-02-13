//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Endpoints.v1;

public static class InvoiceEndpoints
{
	public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/invoice")
			.WithTags("Invoice");
		
		// Create
		
		group.MapPut("CreateInvoice", CreateInvoice)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Delete
		
		group.MapDelete("DeleteInvoice", DeleteInvoice)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPatch("UpdateMemo", UpdateMemo)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		group.MapPatch("UpdateAmount", UpdateAmount)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Read - Single
		
		group.MapGet("GetById/{id}", GetById)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);
		
		// Read - List
		
		group.MapGet("QueryByMemo/{memo}", QueryByMemo)
			.Produces<Guid>(StatusCodes.Status200OK);
	
		return app;
	}
	
	#region Create
	
	public static async Task<IResult> CreateInvoice(IInvoiceService invoiceService, Invoice invoice)
	{
		await invoiceService.CreateInvoice(invoice);
		return Results.Ok();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<Result> DeleteInvoice(IInvoiceService invoiceService, [FromBody] DeleteReq deleteReq)
	{
		await invoiceService.DeleteInvoice(deleteReq.Id);
		return Result.Ok();
	}
	
	#endregion

	#region Updates
	
	public static async Task<Result> UpdateMemo(IInvoiceService invoiceService, [FromBody] UpdateMemoReq request)
	{
		await invoiceService.UpdateMemo(request);
		return Result.Ok();
	}
	
	
	public static async Task<Result> UpdateAmount(IInvoiceService invoiceService, [FromBody] UpdateAmountReq request)
	{
		await invoiceService.UpdateAmount(request);
		return Result.Ok();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<Result<Invoice>> GetById(IInvoiceService invoiceService, Guid id)
	{
		var invoice = await invoiceService.GetById(id);
		return Result<Invoice>.Ok(invoice);
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<Result<List<Invoice>>> QueryByMemo(IInvoiceService invoiceService, [FromRoute] string memo)
	{
		var data = await invoiceService.QueryByMemo(memo);
		return Result<List<Invoice>>.Ok(data);
	}

	#endregion
}
