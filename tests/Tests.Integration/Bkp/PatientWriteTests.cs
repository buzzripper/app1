//using Dyvenix.App1.App.Shared.Contracts.v1;
//using Dyvenix.App1.App.Shared.Requests.v1;
//using Dyvenix.App1.Common.Data.Shared.Entities;
//using Dyvenix.App1.Tests.Integration.Data;
//using Dyvenix.App1.Tests.Integration.DataSets;
//using Dyvenix.App1.Tests.Integration.Fixtures;

//namespace Dyvenix.App1.Tests.Integration.Tests.App;

//public sealed class PatientWriteTestFixtureOld(GlobalTestFixture globalFixture) : IAsyncLifetime
//{
//	public GlobalTestFixture GlobalFixture { get; } = globalFixture;
//	public TestDataSet DataSet { get; private set; } = default!;

//	public async ValueTask InitializeAsync()
//	{
//		var dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();
//		DataSet = await dataManager.Reset(DataSetType.Main.ToString());
//	}

//	public ValueTask DisposeAsync() => default;
//}

//[Collection(nameof(GlobalTestCollection))]
//public class PatientWriteTestsOld : TestBase, IClassFixture<PatientWriteTestFixture>
//{
//	private readonly PatientWriteTestFixture _fixture;
//	private IPatientService _patientService = default!;

//	public PatientWriteTestsOld(GlobalTestFixture globalFixture, PatientWriteTestFixture fixture)
//		: base(globalFixture)
//	{
//		_fixture = fixture;
//	}

//	public override async ValueTask InitializeAsync()
//	{
//		await base.InitializeAsync();
//		_patientService = _scope.ServiceProvider.GetRequiredService<IPatientService>();
//	}

//	[Fact]
//	public async Task CreatePatient_Success()
//	{
//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");
//		var sample = _fixture.DataSet.PatientList.First();
//		var expectedCount = _fixture.DataSet.PatientList.Count + 1;
//		var newEntity = new Patient
//		{
//			Id = Guid.NewGuid(),
//			PracticeId = sample.PracticeId,
//			FirstName = sample.FirstName,
//			LastName = sample.LastName,
//			Email = sample.Email,
//			IsActive = sample.IsActive,
//		};

//		await _patientService.CreatePatient(newEntity);

//		Assert.Equal(expectedCount, _db.Patient.Count());
//		Assert.NotNull(_db.Patient.FirstOrDefault(entity => entity.Id == newEntity.Id));
//	}

//	[Fact]
//	public async Task DeletePatient_Success()
//	{
//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");
//		var sample = _fixture.DataSet.PatientList.First();
//		var expectedCount = _fixture.DataSet.PatientList.Count - 1;

//		await _patientService.DeletePatient(sample.Id);

//		Assert.Equal(expectedCount, _db.Patient.Count());
//		Assert.Null(_db.Patient.FirstOrDefault(entity => entity.Id == sample.Id));
//	}

//	[Fact]
//	public async Task UpdatePatient_Success()
//	{
//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");
//		var sample = _fixture.DataSet.PatientList.First();
//		var updatedEntity = new Patient
//		{
//			Id = sample.Id,
//			PracticeId = sample.PracticeId,
//			RowVersion = sample.RowVersion,
//			FirstName = $"{sample.FirstName}_Updated",
//			LastName = sample.LastName,
//			Email = sample.Email,
//			IsActive = sample.IsActive,
//		};

//		await _patientService.UpdatePatient(updatedEntity);

//		var updated = _db.Patient.First(entity => entity.Id == updatedEntity.Id);
//		Assert.Equal(updatedEntity.FirstName, updated.FirstName);
//	}

//	[Fact]
//	public async Task UpdateFirstName_Success()
//	{
//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");
//		var sample = _fixture.DataSet.PatientList.First();
//		var request = new UpdateFirstNameReq();
//		request.Id = sample.Id;
//		request.RowVersion = sample.RowVersion;
//		request.FirstName = string.Empty;

//		var rowVersion = await _patientService.UpdateFirstName(request);
//		Assert.NotNull(rowVersion);

//		var updated = _db.Patient.First(entity => entity.Id == sample.Id);
//		Assert.Equal(request.FirstName, updated.FirstName);
//	}

//	[Fact]
//	public async Task UpdateLastNameAndEmail_Success()
//	{
//		if (_db == null)
//			throw new InvalidOperationException("App1Db is not available from the test fixture.");
//		var sample = _fixture.DataSet.PatientList.First();
//		var request = new UpdateLastNameAndEmailReq();
//		request.Id = sample.Id;
//		request.RowVersion = sample.RowVersion;
//		request.LastName = sample.LastName;
//		request.Email = sample.Email;

//		var rowVersion = await _patientService.UpdateLastNameAndEmail(request);
//		Assert.NotNull(rowVersion);

//		var updated = _db.Patient.First(entity => entity.Id == sample.Id);
//		Assert.Equal(request.LastName, updated.LastName);
//	}
//}
