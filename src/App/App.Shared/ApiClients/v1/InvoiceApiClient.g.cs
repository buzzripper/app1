//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 5:02 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public partial class InvoiceApiClient : ApiClientBase, IInvoiceService
{
	public InvoiceApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task CreateInvoice(Invoice invoice)
	{
		ArgumentNullException.ThrowIfNull(invoice);
	
		await PostAsync("api/App/v1/Invoice/CreateInvoice", invoice);
	}
	
	#endregion
	
	#region Delete
	
	public async Task DeleteInvoice(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/App/v1/Invoice/DeleteInvoice", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task UpdateMemo(UpdateMemoReq request)
	{
		await PatchAsync($"api/App/v1/Invoice/UpdateMemo", request);
	}
	
	public async Task UpdateAmount(UpdateAmountReq request)
	{
		await PatchAsync($"api/App/v1/Invoice/UpdateAmount", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<Invoice> GetById(Guid id)
	{
		return await GetAsync<Invoice>($"api/App/v1/Invoice/GetById/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<List<Invoice>> QueryByMemo(string memo)
	{
		return await GetAsync<List<Invoice>>($"api/App/v1/Invoice/QueryByMemo/{memo}");
	}

	#endregion
}
