//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/9/2026 10:08 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Tests.Integration.Data;

namespace Dyvenix.App1.Tests.Integration.DataSets;

public class DefaultDataSet : IDataSet
{
	public DefaultDataSet()
	{
		PatientList = CreatePatientList();
		InvoiceList = CreateInvoiceList();
		AppUserList = CreateAppUserList();
	}

	#region Properties

	public DataSetType DataSetType => DataSetType.Default;

	public List<Patient> PatientList { get; set; } = new List<Patient>();
	public List<Invoice> InvoiceList { get; set; } = new List<Invoice>();
	public List<AppUser> AppUserList { get; set; } = new List<AppUser>();

	#endregion

	private List<Patient> CreatePatientList()
	{
		return new List<Patient>
		{
			new Patient
			{
				Id = new Guid("fad0151b-7554-4cde-9e7b-a1c09249e781"),
				FirstName = "Patient_FirstName_1",
				LastName = "Patient_LastName_1",
				IsActive = true,
				Email = "Patient_Email_1",
				Invoices = new List<Invoice>(),
			},
			new Patient
			{
				Id = new Guid("525419a0-9e60-47cb-a973-0ebbd6249591"),
				FirstName = "B",
				LastName = "B",
				IsActive = false,
				Email = "B",
			},
			new Patient
			{
				Id = new Guid("d449d572-0fa1-4fbb-91b2-67f874f88f23"),
				FirstName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
				LastName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
				IsActive = true,
				Email = null,
				Invoices = new List<Invoice>(),
			},
			new Patient
			{
				Id = new Guid("f9c7b2cb-e2c2-44dd-bc76-a4907420f8fe"),
				FirstName = "Test Value 4",
				LastName = "Test Value 4",
				IsActive = false,
				Email = "Test Value 4",
			},
			new Patient
			{
				Id = new Guid("1b191e5d-9b23-4e0f-a71d-eff6c2213694"),
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
				Id = new Guid("42836d84-454d-45b4-91ed-b646d1062fc0"),
				PersonId = new Guid("4222a803-ffec-4adb-ab08-762e71007e97"),
				Amount = 10.01m,
				Memo = "Invoice_Memo_1",
			},
			new Invoice
			{
				Id = new Guid("31566056-3ae9-4856-acf7-afa09468c004"),
				PersonId = new Guid("b0066b01-203e-4a59-a1eb-42883c428c75"),
				Amount = 20.02m,
				Memo = "B",
			},
			new Invoice
			{
				Id = new Guid("ccb859ac-5c51-4514-a245-971f4a8f9924"),
				PersonId = new Guid("19e76c2c-e56a-46b6-896d-75ac4f0a0079"),
				Amount = 30.03m,
				Memo = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
			},
			new Invoice
			{
				Id = new Guid("74f672d0-cb2e-4595-9b2f-27373c2502ce"),
				PersonId = new Guid("9487d454-730d-4612-a52d-f88d40f9d2be"),
				Amount = 40.04m,
				Memo = "Test Value 4",
			},
			new Invoice
			{
				Id = new Guid("3df90061-b034-4a67-83f5-0f9223484623"),
				PersonId = new Guid("c4853c3c-0090-4db1-8547-e4cd97bad04d"),
				Amount = 50.05m,
				Memo = "Invoice_Memo-5!",
			},
			new Invoice
			{
				Id = new Guid("a2c6100a-4374-4789-9d03-7c584b8ee690"),
				PersonId = new Guid("83806628-0942-4e6a-86f5-df92adc1e96f"),
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
				Id = new Guid("e5807700-b4d8-4e7d-b6e2-8ef24a2fa8f5"),
				Username = "AppUser_Username_1",
			},
			new AppUser
			{
				Id = new Guid("25b9f2a6-4d6c-4613-98d9-839ed97dfda9"),
				Username = "B",
			},
			new AppUser
			{
				Id = new Guid("6bf286ad-3865-46af-9f4d-19ee0a3cfb0a"),
				Username = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
			},
			new AppUser
			{
				Id = new Guid("b5a461f4-a4d7-43c7-b841-905d1c26925f"),
				Username = "Test Value 4",
			},
			new AppUser
			{
				Id = new Guid("5e7c5e6d-ffd3-4563-a915-1c3229f43c19"),
				Username = "AppUser_Username-5!",
			},
		};
	}
}
