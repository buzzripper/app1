//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/8/2026 4:23 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Dyvenix.App1.Common.Data.Shared.Entities;

namespace Dyvenix.App1.Common.Data;

public class DataSet
{
	#region Properties

	public List<Patient> PatientList { get; set; } = new List<Patient>();
	public List<Invoice> InvoiceList { get; set; } = new List<Invoice>();
	public List<AppUser> AppUserList { get; set; } = new List<AppUser>();

	#endregion

	public void Initialize()
	{
		PatientList = CreatePatientList();
		InvoiceList = CreateInvoiceList();
		AppUserList = CreateAppUserList();
	}

	private List<Patient> CreatePatientList()
	{
		return new List<Patient>
		{
			new Patient
			{
				Id = new Guid("00000001-0000-0000-0000-Patient00000"),
				FirstName = "Patient_FirstName_1",
				LastName = "Patient_LastName_1",
				IsActive = true,
				Email = "Patient_Email_1",
				Invoices = new List<Invoice>(),
			},
			new Patient
			{
				Id = new Guid("00000002-0000-0000-0000-Patient00000"),
				FirstName = "B",
				LastName = "B",
				IsActive = false,
				Email = "B",
			},
			new Patient
			{
				Id = new Guid("00000003-0000-0000-0000-Patient00000"),
				FirstName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
				LastName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
				IsActive = true,
				Email = null,
				Invoices = new List<Invoice>(),
			},
			new Patient
			{
				Id = new Guid("00000004-0000-0000-0000-Patient00000"),
				FirstName = "Test Value 4",
				LastName = "Test Value 4",
				IsActive = false,
				Email = "Test Value 4",
			},
			new Patient
			{
				Id = new Guid("00000005-0000-0000-0000-Patient00000"),
				FirstName = "Patient_FirstName-5!",
				LastName = "Patient_LastName-5!",
				IsActive = true,
				Email = "Patient_Email-5!",
				Invoices = new List<Invoice>(),
			},
		};
	}

	private List<Invoice> CreateInvoiceList()
	{
		return new List<Invoice>
		{
			new Invoice
			{
				Id = new Guid("00000001-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000001-0000-0000-0000-PersonId0000"),
				Amount = 10.01m,
				Memo = "Invoice_Memo_1",
			},
			new Invoice
			{
				Id = new Guid("00000002-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000002-0000-0000-0000-PersonId0000"),
				Amount = 20.02m,
				Memo = "B",
			},
			new Invoice
			{
				Id = new Guid("00000003-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000003-0000-0000-0000-PersonId0000"),
				Amount = 30.03m,
				Memo = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
			},
			new Invoice
			{
				Id = new Guid("00000004-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000001-0000-0000-0000-PersonId0000"),
				Amount = 40.04m,
				Memo = "Test Value 4",
			},
			new Invoice
			{
				Id = new Guid("00000005-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000002-0000-0000-0000-PersonId0000"),
				Amount = 50.05m,
				Memo = "Invoice_Memo-5!",
			},
			new Invoice
			{
				Id = new Guid("00000006-0000-0000-0000-Invoice00000"),
				PersonId = new Guid("00000003-0000-0000-0000-PersonId0000"),
				Amount = 60.06m,
				Memo = "Invoice_Memo_6",
			},
		};
	}

	private List<AppUser> CreateAppUserList()
	{
		return new List<AppUser>
		{
			new AppUser
			{
				Id = new Guid("00000001-0000-0000-0000-AppUser00000"),
				Username = "AppUser_Username_1",
			},
			new AppUser
			{
				Id = new Guid("00000002-0000-0000-0000-AppUser00000"),
				Username = "B",
			},
			new AppUser
			{
				Id = new Guid("00000003-0000-0000-0000-AppUser00000"),
				Username = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
			},
			new AppUser
			{
				Id = new Guid("00000004-0000-0000-0000-AppUser00000"),
				Username = "Test Value 4",
			},
			new AppUser
			{
				Id = new Guid("00000005-0000-0000-0000-AppUser00000"),
				Username = "AppUser_Username-5!",
			},
		};
	}
}
