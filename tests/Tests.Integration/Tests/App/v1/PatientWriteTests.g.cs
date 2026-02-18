//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/18/2026 7:27 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.App.v1;

public class PatientWriteTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;

	public ValueTask InitializeAsync() => default;

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class PatientWriteTests : TestBase, IClassFixture<PatientWriteTestFixture>
{
	private readonly PatientWriteTestFixture _fixture;
	private IPatientService _patientService = default!;

	public PatientWriteTests(GlobalTestFixture globalFixture, PatientWriteTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		// Reset database before each write test for isolation
		var dataManager = _scope.ServiceProvider.GetRequiredService<IDataManager>();
		await dataManager.Reset(DataSetType.Main.ToString());
		_patientService = _scope.ServiceProvider.GetRequiredService<IPatientService>();
	}

	[Fact]
	public async Task Create_Success()
	{
		// Arrange
		var newEntity = new Patient();
		newEntity.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.LastName = "Test_LastName_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.Email = "Test_Email_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.IsActive = true;
		newEntity.PracticeId = Guid.NewGuid();

		// Act
		var rowVersion = await _patientService.CreatePatient(newEntity);

		// Assert
		Assert.NotNull(rowVersion);
		Assert.NotEmpty(rowVersion);
		Assert.NotEqual(Guid.Empty, newEntity.Id);
	}

	[Fact]
	public async Task Create_ValidationError_MissingRequired()
	{
		// Arrange
		var newEntity = new Patient();
		newEntity.FirstName = string.Empty; // Required field left empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.CreatePatient(newEntity));
	}

	[Fact]
	public async Task Update_Success()
	{
		// Arrange
		var existing = _db.Patient.First();
		var entityToUpdate = await _patientService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		var originalValue = entityToUpdate.FirstName;
		entityToUpdate.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		var rowVersion = await _patientService.UpdatePatient(entityToUpdate);

		// Assert
		Assert.NotNull(rowVersion);
		Assert.NotEmpty(rowVersion);
		var updated = _db.Patient.First(x => x.Id == entityToUpdate.Id);
		Assert.NotNull(updated);
		Assert.NotEqual(originalValue, updated.FirstName);
	}

	[Fact]
	public async Task Update_NotFound()
	{
		// Arrange
		var entityToUpdate = new Patient();
		entityToUpdate.Id = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdatePatient(entityToUpdate));
	}

	[Fact]
	public async Task Update_ValidationError()
	{
		// Arrange
		var existing = _db.Patient.First();
		var entityToUpdate = await _patientService.GetById(existing.Id);
		Assert.NotNull(entityToUpdate);
		entityToUpdate.FirstName = string.Empty; // Clear required field

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdatePatient(entityToUpdate));
	}

	[Fact]
	public async Task Update_RowVersionConflict()
	{
		// Arrange
		var existing = _db.Patient.First();
		var entityOne = await _patientService.GetById(existing.Id);
		var entityTwo = await _patientService.GetById(existing.Id);
		Assert.NotNull(entityOne);
		Assert.NotNull(entityTwo);
		entityOne.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);
		entityTwo.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		_ = await _patientService.UpdatePatient(entityOne);

		// Assert
		await Assert.ThrowsAsync<ConcurrencyException>(async () => await _patientService.UpdatePatient(entityTwo));
	}

	[Fact]
	public async Task Update_RowVersionSuccess()
	{
		// Arrange
		var existing = _db.Patient.First();
		var entityOne = await _patientService.GetById(existing.Id);
		var entityTwo = await _patientService.GetById(existing.Id);
		Assert.NotNull(entityOne);
		Assert.NotNull(entityTwo);
		entityOne.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);
		entityTwo.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		var rowVersion = await _patientService.UpdatePatient(entityOne);
		Assert.NotNull(rowVersion);
		Assert.NotEmpty(rowVersion);
		entityTwo.RowVersion = rowVersion;
		await _patientService.UpdatePatient(entityTwo);

		// Assert
		var updated = _db.Patient.First(x => x.Id == entityTwo.Id);
		Assert.NotNull(updated);
		Assert.Equal(entityTwo.FirstName, updated.FirstName);
	}

	[Fact]
	public async Task UpdateFirstName_Success()
	{
		// Arrange
		var existing = _db.Patient.First();
		var request = new UpdateFirstNameReq();
		request.Id = existing.Id;
		request.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		var rowVersion = await _patientService.UpdateFirstName(request);

		// Assert
		Assert.NotNull(rowVersion);
		Assert.NotEmpty(rowVersion);
	}

	[Fact]
	public async Task UpdateFirstName_NotFound()
	{
		// Arrange
		var request = new UpdateFirstNameReq();
		request.Id = Guid.NewGuid();
		request.FirstName = "Test_FirstName_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdateFirstName(request));
	}

	[Fact]
	public async Task UpdateFirstName_ValidationError()
	{
		// Arrange
		var existing = _db.Patient.First();
		var request = new UpdateFirstNameReq();
		request.Id = existing.Id;
		request.FirstName = string.Empty; // Required field empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdateFirstName(request));
	}

	[Fact]
	public async Task UpdateLastNameAndEmail_Success()
	{
		// Arrange
		var existing = _db.Patient.First();
		var request = new UpdateLastNameAndEmailReq();
		request.Id = existing.Id;
		request.LastName = "Test_LastName_" + Guid.NewGuid().ToString().Substring(0, 8);
		request.Email = "Test_Email_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		var rowVersion = await _patientService.UpdateLastNameAndEmail(request);

		// Assert
		Assert.NotNull(rowVersion);
		Assert.NotEmpty(rowVersion);
	}

	[Fact]
	public async Task UpdateLastNameAndEmail_NotFound()
	{
		// Arrange
		var request = new UpdateLastNameAndEmailReq();
		request.Id = Guid.NewGuid();
		request.LastName = "Test_LastName_" + Guid.NewGuid().ToString().Substring(0, 8);
		request.Email = "Test_Email_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdateLastNameAndEmail(request));
	}

	[Fact]
	public async Task UpdateLastNameAndEmail_ValidationError()
	{
		// Arrange
		var existing = _db.Patient.First();
		var request = new UpdateLastNameAndEmailReq();
		request.Id = existing.Id;
		request.LastName = string.Empty; // Required field empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.UpdateLastNameAndEmail(request));
	}

	[Fact]
	public async Task Delete_Success()
	{
		// Arrange
		var existing = _db.Patient.First();
		var idToDelete = existing.Id;

		// Act
		await _patientService.DeletePatient(idToDelete);

		// Assert
		var deleted = await _patientService.GetById(idToDelete);
		Assert.Null(deleted);
	}

	[Fact]
	public async Task Delete_NotFound()
	{
		// Arrange
		var invalidId = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _patientService.DeletePatient(invalidId));
	}
}
