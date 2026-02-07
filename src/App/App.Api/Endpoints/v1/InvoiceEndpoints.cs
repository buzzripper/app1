//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/7/2026 3:16 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Api.Services.v1;
using Dyvenix.App1.App.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.App.Shared.DTOs.v1;

namespace Dyvenix.App1.App.Api.Controllers.v1;

public static class InvoiceEndpoints
{
	public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/app/v1/invoice")
			.WithTags("Invoice");
		
		// Create
		
		group.MapPost("CreateInvoice", CreateInvoice)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Delete
		
		group.MapPost("DeleteInvoice", DeleteInvoice)
			.Produces<Guid>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status409Conflict);
		
		// Update
		
		group.MapPost("UpdateMemo", UpdateMemo)
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
		var result = await invoiceService.CreateInvoice(invoice);
		return result.ToHttpResult();
	}
	
	#endregion
	
	#region Delete
	
	public static async Task<IResult> DeleteInvoice(IInvoiceService invoiceService, Guid id)
	{
		var result = await invoiceService.DeleteInvoice(id);
		return result.ToHttpResult();
	}
	
	#endregion

	#region Updates
	public static async Task<IResult> UpdateMemo(IInvoiceService invoiceService, [FromBody] UpdateMemoReq request)
	{
		var result = await invoiceService.UpdateMemo(request.Id, request.Memo);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - Single
	
	public static async Task<IResult> GetById(IInvoiceService invoiceService, Guid id)
	{
		var result = await invoiceService.GetById(id);
		return result.ToHttpResult();
	}

	#endregion

	#region Read Methods - List
	
	public static async Task<IResult> QueryByMemo(IInvoiceService invoiceService, [FromRoute] string memo)
	{
		var result = await invoiceService.QueryByMemo(memo);
		return result.ToHttpResult();
	}

	#endregion
}
