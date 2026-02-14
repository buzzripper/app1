//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 11:12 AM. Any changes made to it will be lost.
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
	
		await PostAsync("api/v1/Patient/CreatePatient", patient);
	}
	
	#endregion
	
	#region Delete
	
	public async Task DeletePatient(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		await DeleteAsync<bool>($"api/v1/Patient/DeletePatient", deleteReq);
	}
	
	#endregion

	#region Updates
	
	public async Task<byte[]> UpdatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);
		return await PutAsync<byte[]>("api/v1/Patient/UpdatePatient", patient);
	}
	
	public async Task<byte[]> UpdateFirstName(UpdateFirstNameReq request)
	{
		return await PatchAsync<byte[]>($"api/v1/Patient/UpdateFirstName", request);
	}
	
	public async Task<byte[]> UpdateLastNameAndEmail(UpdateLastNameAndEmailReq request)
	{
		return await PatchAsync<byte[]>($"api/v1/Patient/UpdateLastNameAndEmail", request);
	}

	#endregion

	#region Read Methods - Single
	
	public async Task<Patient> GetById(Guid id)
	{
		return await GetAsync<Patient>($"api/v1/Patient/GetById/{id}");
	}
	
	public async Task<Patient> GetByEmail(string email)
	{
		return await GetAsync<Patient>($"api/v1/Patient/GetByEmail/{email}");
	}
	
	public async Task<Patient> GetByIdWithInvoices(Guid id)
	{
		return await GetAsync<Patient>($"api/v1/Patient/GetByIdWithInvoices/{id}");
	}

	#endregion

	#region Read Methods - List
	
	public async Task<ListPage<Patient>> GetAllPaging(GetAllPagingReq request)
	{
		return await GetAsync<ListPage<Patient>>($"api/v1/Patient/GetAllPaging");
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePaging(SearchByLastNamePagingReq request)
	{
		return await GetAsync<ListPage<Patient>>($"api/v1/Patient/SearchByLastNamePaging");
	}
	
	public async Task<List<Patient>> SearchByLastNameSorting(SearchByLastNameSortingReq request)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastNameSorting");
	}
	
	public async Task<ListPage<Patient>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request)
	{
		return await GetAsync<ListPage<Patient>>($"api/v1/Patient/SearchByLastNamePagingSorting");
	}
	
	public async Task<List<Patient>> GetAllSorting(GetAllSortingReq request)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetAllSorting");
	}
	
	public async Task<List<Patient>> SearchByLastEmailOpt(SearchByLastEmailOptReq request)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastEmailOpt");
	}
	
	public async Task<List<Patient>> SearchByEmail(SearchByEmailReq request)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByEmail");
	}
	
	public async Task<List<Patient>> GetActive()
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetActive");
	}
	
	public async Task<ListPage<Patient>> GetAllPagingSorting(GetAllPagingSortingReq request)
	{
		return await GetAsync<ListPage<Patient>>($"api/v1/Patient/GetAllPagingSorting");
	}
	
	public async Task<List<Patient>> SearchActiveLastName(string lastName)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchActiveLastName/{lastName}");
	}

	#endregion
}
