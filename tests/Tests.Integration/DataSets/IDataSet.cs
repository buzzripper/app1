//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/8/2026 8:50 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Tests.Integration.Data;

namespace Dyvenix.App1.Tests.Integration.DataSets;

public interface IDataSet
{
	DataSetType DataSetType { get; }

	List<Patient> PatientList { get; set; }
	List<Invoice> InvoiceList { get; set; }
	List<AppUser> AppUserList { get; set; }
}
