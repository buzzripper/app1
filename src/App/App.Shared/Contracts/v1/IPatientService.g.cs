//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Extensions;

namespace Dyvenix.App1.App.Shared.Contracts.v1;

public interface IPatientService
{
	Task CreatePatient(Patient patient);
	Task DeletePatient(Guid id);
	Task<byte[]> UpdatePatient(Patient patient);
	Task<byte[]> UpdateFirstName(UpdateFirstNameReq request);
	Task<byte[]> UpdateLastNameAndEmail(UpdateLastNameAndEmailReq request);
	Task<Patient> GetById(Guid id);
	Task<Patient> GetByEmail(string email);
	Task<Patient> GetByIdWithInvoices(Guid id);
	Task<ListPage<Patient>> GetAllPaging(GetAllPagingReq request);
	Task<ListPage<Patient>> SearchByLastNamePaging(SearchByLastNamePagingReq request);
	Task<List<Patient>> SearchByLastNameSorting(SearchByLastNameSortingReq request);
	Task<ListPage<Patient>> SearchByLastNamePagingSorting(SearchByLastNamePagingSortingReq request);
	Task<List<Patient>> GetAllSorting(GetAllSortingReq request);
	Task<List<Patient>> SearchByLastEmailOpt(SearchByLastEmailOptReq request);
	Task<List<Patient>> SearchByEmail(SearchByEmailReq request);
	Task<List<Patient>> GetActive();
	Task<ListPage<Patient>> GetAllPagingSorting(GetAllPagingSortingReq request);
	Task<List<Patient>> SearchActiveLastName(string lastName);
}
