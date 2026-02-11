//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 4:33 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public interface IInvoiceApiClient
{
	Task<Guid> CreateInvoice(Invoice invoice);
	Task<bool> DeleteInvoice(Guid id);
	Task<byte[]> UpdateMemo(UpdateMemoReq request);
	Task<Invoice> GetById(Guid id);
	Task<List<Invoice>> QueryByMemo(string memo);
}

public partial class InvoiceApiClient : ApiClientBase, IInvoiceApiClient
{
	public InvoiceApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task<Guid> CreateInvoice(Invoice invoice)
	{
		ArgumentNullException.ThrowIfNull(invoice);
	
		return await PostAsync<Guid>("api/v1/Invoice/CreateInvoice", invoice);
	}
	
	#endregion
	
	#region Delete
	
	public async Task<bool> DeleteInvoice(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		return await DeleteAsync<bool>($"api/v1/Invoice/DeleteInvoice", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> UpdateMemo(UpdateMemoReq request)
	{
		return await PatchAsync<byte[]>($"api/v1/Invoice/UpdateMemo", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<Invoice> GetById(Guid id)
	{
		return await GetAsync<Invoice>($"api/v1/Invoice/GetById/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<List<Invoice>> QueryByMemo(string memo)
	{
		return await GetAsync<List<Invoice>>($"api/v1/Invoice/QueryByMemo/{memo}");
	}

	#endregion
}
