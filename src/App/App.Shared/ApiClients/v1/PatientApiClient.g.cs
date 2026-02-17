//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/16/2026 9:37 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public partial class PatientApiClient : ApiClientBase, IPatientService
{
	public PatientApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task CreatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);
	
		await PostAsync("api/App/v1/Patient/CreatePatient", patient);
	}
	
	#endregion
	
	#region Delete
	
	public async Task DeletePatient(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/App/v1/Patient/DeletePatient", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> UpdatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);
		return await PutAsync<byte[]>("api/App/v1/Patient/UpdatePatient", patient);
	}
	
	public async Task<byte[]> UpdateFirstName(UpdateFirstNameReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Patient/UpdateFirstName", request);
	}
	
	public async Task<byte[]> UpdateLastNameAndEmail(UpdateLastNameAndEmailReq request)
	{
		return await PatchAsync<byte[]>($"api/App/v1/Patient/UpdateLastNameAndEmail", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<Patient> GetById(Guid id)
	{
		return await GetAsync<Patient>($"api/App/v1/Patient/GetById/{id}");
	}
	
	public async Task<Patient> GetByEmail(string email)
	{
		return await GetAsync<Patient>($"api/App/v1/Patient/GetByEmail/{email}");
	}
	
	public async Task<Patient> GetByIdWithInvoices(Guid id)
	{
		return await GetAsync<Patient>($"api/App/v1/Patient/GetByIdWithInvoices/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<ListPage<Patient>> GetAllPaging(GetAllPagingReq request)
	{
		return await PostAsync<ListPage<Patient>>($"api/App/v1/Patient/GetAllPaging", request);
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePaging(SearchByLastNamePagingReq request)
	{
		return await PostAsync<ListPage<Patient>>($"api/App/v1/Patient/SearchByLastNamePaging", request);
	}
	
	public async Task<List<Patient>> SearchByLastNameSorting(SearchByLastNameSortingReq request)
	{
		return await PostAsync<List<Patient>>($"api/App/v1/Patient/SearchByLastNameSorting", request);
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request)
	{
		return await PostAsync<ListPage<Patient>>($"api/App/v1/Patient/SearchByLastNamePagingSorting", request);
	}
	
	public async Task<List<Patient>> GetAllSorting(GetAllSortingReq request)
	{
		return await PostAsync<List<Patient>>($"api/App/v1/Patient/GetAllSorting", request);
	}
	
	public async Task<List<Patient>> SearchByLastEmailOpt(SearchByLastEmailOptReq request)
	{
		return await PostAsync<List<Patient>>($"api/App/v1/Patient/SearchByLastEmailOpt", request);
	}
	
	public async Task<List<Patient>> SearchByEmail(SearchByEmailReq request)
	{
		return await PostAsync<List<Patient>>($"api/App/v1/Patient/SearchByEmail", request);
	}
	
	public async Task<List<Patient>> GetActive()
	{
		return await GetAsync<List<Patient>>($"api/App/v1/Patient/GetActive");
	}
	
	public async Task<ListPage<Patient>> GetAllPagingSorting(GetAllPagingSortingReq request)
	{
		return await PostAsync<ListPage<Patient>>($"api/App/v1/Patient/GetAllPagingSorting", request);
	}
	
	public async Task<List<Patient>> SearchActiveLastName(string lastName)
	{
		return await GetAsync<List<Patient>>($"api/App/v1/Patient/SearchActiveLastName/{lastName}");
	}

	#endregion
}
