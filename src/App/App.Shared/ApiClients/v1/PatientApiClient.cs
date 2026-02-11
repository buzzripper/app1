//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 4:33 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.Requests;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public interface IPatientApiClient
{
	Task<Guid> CreatePatient(Patient patient);
	Task<bool> DeletePatient(Guid id);
	Task<byte[]> UpdatePatient(Patient patient);
	Task<byte[]> UpdateFirstName(UpdateFirstNameReq request);
	Task<byte[]> UpdateLastNameAndEmail(UpdateLastNameAndEmailReq request);
	Task<Patient> GetById(Guid id);
	Task<Patient> GetByEmail(string email);
	Task<Patient> GetByIdWithInvoices(Guid id);
	Task<List<Patient>> GetAllPaging(int pgSize = 0, int pgOffset = 0);
	Task<List<Patient>> SearchByLastNamePaging(string lastName, int pgSize = 0, int pgOffset = 0);
	Task<List<Patient>> SearchByLastNameSorting(string lastName);
	Task<List<Patient>> SearchByLastNamePagingSorting(string lastName, int pgSize = 0, int pgOffset = 0);
	Task<List<Patient>> GetAllSorting();
	Task<List<Patient>> SearchByLastEmailOpt(string lastName, string? email = null);
	Task<List<Patient>> SearchByEmail(string email);
	Task<List<Patient>> GetActive();
	Task<List<Patient>> GetAllPagingSorting(int pgSize = 0, int pgOffset = 0);
	Task<List<Patient>> SearchActiveLastName(string lastName);
}

public partial class PatientApiClient : ApiClientBase, IPatientApiClient
{
	public PatientApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
	#region Create
	
	public async Task<Guid> CreatePatient(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);
	
		return await PostAsync<Guid>("api/v1/Patient/CreatePatient", patient);
	}
	
	#endregion
	
	#region Delete
	
	public async Task<bool> DeletePatient(Guid id)
	{
		if (id == Guid.Empty)
			throw new ArgumentNullException(nameof(id));
	
		var deleteReq = new DeleteReq { Id = id };	
		return await DeleteAsync<bool>($"api/v1/Patient/DeletePatient", deleteReq);
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
	
	public async Task<List<Patient>> GetAllPaging(int pgSize = 0, int pgOffset = 0)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetAllPaging?pgSize={pgSize}&pgOffset={pgOffset}");
	}
	
	public async Task<List<Patient>> SearchByLastNamePaging(string lastName, int pgSize = 0, int pgOffset = 0)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastNamePaging/{lastName}?pgSize={pgSize}&pgOffset={pgOffset}");
	}
	
	public async Task<List<Patient>> SearchByLastNameSorting(string lastName)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastNameSorting/{lastName}");
	}
	
	public async Task<List<Patient>> SearchByLastNamePagingSorting(string lastName, int pgSize = 0, int pgOffset = 0)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastNamePagingSorting/{lastName}?pgSize={pgSize}&pgOffset={pgOffset}");
	}
	
	public async Task<List<Patient>> GetAllSorting()
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetAllSorting");
	}
	
	public async Task<List<Patient>> SearchByLastEmailOpt(string lastName, string? email = null)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByLastEmailOpt/{lastName}?email={email}");
	}
	
	public async Task<List<Patient>> SearchByEmail(string email)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchByEmail/{email}");
	}
	
	public async Task<List<Patient>> GetActive()
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetActive");
	}
	
	public async Task<List<Patient>> GetAllPagingSorting(int pgSize = 0, int pgOffset = 0)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/GetAllPagingSorting?pgSize={pgSize}&pgOffset={pgOffset}");
	}
	
	public async Task<List<Patient>> SearchActiveLastName(string lastName)
	{
		return await GetAsync<List<Patient>>($"api/v1/Patient/SearchActiveLastName/{lastName}");
	}

	#endregion
}
