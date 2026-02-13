//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/12/2026 8:04 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Services.v1;
using Dyvenix.App1.Common.Api.Extensions;
using Dyvenix.App1.Common.Api.Filters;
using Dyvenix.App1.Common.Shared.Requests;
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
	
	public static async Task<IResult> DeleteInvoice(IInvoiceService invoiceService, [FromBody] DeleteReq deleteReq)
	{
		var result = await invoiceService.DeleteInvoice(deleteReq.Id);
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
