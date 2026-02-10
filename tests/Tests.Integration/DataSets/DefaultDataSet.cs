////------------------------------------------------------------------------------------------------------------
//// This file was auto-generated on 2/9/2026 8:37 PM. Any changes made to it will be lost.
////------------------------------------------------------------------------------------------------------------
//using System;
//using System.Collections.Generic;
//using Dyvenix.App1.Common.Data.Shared.Entities;
//using Dyvenix.App1.Tests.Integration.Data;

//namespace Dyvenix.App1.Tests.Integration.DataSets;

//public class DefaultDataSet : IDataSet
//{
//	public DefaultDataSet()
//	{
//		PracticeList = CreatePracticeList();
//		AppUserList = CreateAppUserList();
//		PatientList = CreatePatientList();
//		InvoiceList = CreateInvoiceList();
//	}

//	#region Properties

//	public DataSetType DataSetType => DataSetType.Default;

//	public List<Practice> PracticeList { get; set; } = new List<Practice>();
//	public List<AppUser> AppUserList { get; set; } = new List<AppUser>();
//	public List<Patient> PatientList { get; set; } = new List<Patient>();
//	public List<Invoice> InvoiceList { get; set; } = new List<Invoice>();

//	#endregion

//	private List<Practice> CreatePracticeList()
//	{
//		return new List<Practice>
//		{
//			new Practice
//			{
//				Id = new Guid("43dc1d3a-01c2-4457-98d8-31724efbdfab"),
//				Name = "Practice_Name_1",
//				Patients = new List<Patient>(),
//			},
//			new Practice
//			{
//				Id = new Guid("86515da4-5f7e-49e4-8277-fe8bad6bdb41"),
//				Name = "B",
//			},
//			new Practice
//			{
//				Id = new Guid("139fd642-684a-4fe7-9880-5c9e4bcd017b"),
//				Name = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//				Patients = new List<Patient>(),
//			},
//			new Practice
//			{
//				Id = new Guid("db9345ad-8b4a-4c4e-82dd-e0b21d78af01"),
//				Name = "Test Value 4",
//			},
//			new Practice
//			{
//				Id = new Guid("e9f1749d-b408-4043-bf81-fbe690ed2b3e"),
//				Name = "Practice_Name-5!",
//				Patients = new List<Patient>(),
//			},
//		};
//	}

//	private List<AppUser> CreateAppUserList()
//	{
//		return new List<AppUser>
//		{
//			new AppUser
//			{
//				Id = new Guid("7e6b2d82-aee9-47cf-a1b9-510e402b4b2f"),
//				Username = "AppUser_Username_1",
//			},
//			new AppUser
//			{
//				Id = new Guid("051956ee-b87a-45c2-9c8d-c6f7f5e017ca"),
//				Username = "B",
//			},
//			new AppUser
//			{
//				Id = new Guid("21d902cf-9b31-4972-9c16-dd6b116d0b70"),
//				Username = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//			},
//			new AppUser
//			{
//				Id = new Guid("1e0359b4-8e71-4b4a-94c1-c970fcf65e8d"),
//				Username = "Test Value 4",
//			},
//			new AppUser
//			{
//				Id = new Guid("a67121be-ad76-40af-a4dd-a97e03da6318"),
//				Username = "AppUser_Username-5!",
//			},
//		};
//	}

//	private List<Patient> CreatePatientList()
//	{
//		return new List<Patient>
//		{
//			new Patient
//			{
//				Id = new Guid("e6542b09-527e-489e-99aa-953abe9ec3ae"),
//				PracticeId = new Guid("43dc1d3a-01c2-4457-98d8-31724efbdfab"),
//				FirstName = "Patient_FirstName_1",
//				LastName = "Patient_LastName_1",
//				IsActive = true,
//				Email = "Patient_Email_1",
//				Invoices = new List<Invoice>(),
//			},
//			new Patient
//			{
//				Id = new Guid("742dae0d-62cb-4796-9910-1d28ea54b8f4"),
//				PracticeId = new Guid("86515da4-5f7e-49e4-8277-fe8bad6bdb41"),
//				FirstName = "B",
//				LastName = "B",
//				IsActive = false,
//				Email = "B",
//			},
//			new Patient
//			{
//				Id = new Guid("742953da-c8ac-4523-ae76-f82cfd530fcc"),
//				PracticeId = new Guid("139fd642-684a-4fe7-9880-5c9e4bcd017b"),
//				FirstName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//				LastName = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//				IsActive = true,
//				Email = null,
//				Invoices = new List<Invoice>(),
//			},
//			new Patient
//			{
//				Id = new Guid("51c44055-fd94-4b67-83fb-f0adb25ba505"),
//				PracticeId = new Guid("db9345ad-8b4a-4c4e-82dd-e0b21d78af01"),
//				FirstName = "Test Value 4",
//				LastName = "Test Value 4",
//				IsActive = false,
//				Email = "Test Value 4",
//			},
//			new Patient
//			{
//				Id = new Guid("aaa100ac-2034-4fb6-bfa0-045bbc382b2b"),
//				PracticeId = new Guid("e9f1749d-b408-4043-bf81-fbe690ed2b3e"),
//				FirstName = "Patient_FirstName-5!",
//				LastName = "Patient_LastName-5!",
//				IsActive = true,
//				Email = "Patient_Email-5!",
//				Invoices = new List<Invoice>(),
//			},
//			new Patient
//			{
//				Id = new Guid("507bde1e-5d60-421b-b277-36a962f29f4b"),
//				PracticeId = new Guid("43dc1d3a-01c2-4457-98d8-31724efbdfab"),
//				FirstName = "Patient_FirstName_6",
//				LastName = "Patient_LastName_6",
//				IsActive = false,
//				Email = null,
//			},
//		};
//	}

//	private List<Invoice> CreateInvoiceList()
//	{
//		return new List<Invoice>
//		{
//			new Invoice
//			{
//				Id = new Guid("0f8611a6-fcd8-45a5-bdcb-892cd3d8167b"),
//				PersonId = new Guid("1f5c1eac-e53c-4b90-960d-f0a32d22e822"),
//				Amount = 10.01m,
//				Memo = "Invoice_Memo_1",
//			},
//			new Invoice
//			{
//				Id = new Guid("4423eecf-9e83-4c79-9270-8dbd7b008184"),
//				PersonId = new Guid("24b79187-7cc9-4ed3-bf97-d6e587cd70d0"),
//				Amount = 20.02m,
//				Memo = "B",
//			},
//			new Invoice
//			{
//				Id = new Guid("0b2dbff0-454d-45a4-a032-033e172779ab"),
//				PersonId = new Guid("0c56a03a-6d8b-4105-a220-1ba661e18dbf"),
//				Amount = 30.03m,
//				Memo = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
//			},
//			new Invoice
//			{
//				Id = new Guid("c12e5609-1e97-4e9e-80b8-7b4103c63608"),
//				PersonId = new Guid("830af366-1d8f-47b4-bf6a-790423bb8021"),
//				Amount = 40.04m,
//				Memo = "Test Value 4",
//			},
//			new Invoice
//			{
//				Id = new Guid("2269cedb-2de7-4039-ad96-951ad8db31ea"),
//				PersonId = new Guid("622fde4b-320b-4b67-a937-055e8b2a7a74"),
//				Amount = 50.05m,
//				Memo = "Invoice_Memo-5!",
//			},
//			new Invoice
//			{
//				Id = new Guid("2d1d3268-034d-4330-9ddc-a992ffa43215"),
//				PersonId = new Guid("da3f5245-ed6d-4d85-b0b9-e8d0638f56df"),
//				Amount = 60.06m,
//				Memo = "Invoice_Memo_6",
//			},
//		};
//	}
//}
