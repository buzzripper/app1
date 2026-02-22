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

public class InvoiceWriteTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;

	public ValueTask InitializeAsync() => default;

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class InvoiceWriteTests : TestBase, IClassFixture<InvoiceWriteTestFixture>
{
	private readonly InvoiceWriteTestFixture _fixture;
	private IInvoiceService _invoiceService = default!;

	public InvoiceWriteTests(GlobalTestFixture globalFixture, InvoiceWriteTestFixture fixture)
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
		_invoiceService = _scope.ServiceProvider.GetRequiredService<IInvoiceService>();
	}

	[Fact]
	public async Task Create_Success()
	{
		// Arrange
		var newEntity = new Invoice();
		newEntity.PatientId = Guid.NewGuid();
		newEntity.Amount = 42.50m;
		newEntity.Memo = "Test_Memo_" + Guid.NewGuid().ToString().Substring(0, 8);
		newEntity.CategoryId = Guid.NewGuid();

		// Act
		await _invoiceService.CreateInvoice(newEntity);

		// Assert
		Assert.NotEqual(Guid.Empty, newEntity.Id);
	}

	[Fact]
	public async Task Create_ValidationError_MissingRequired()
	{
		// Arrange
		var newEntity = new Invoice();
		newEntity.Memo = string.Empty; // Required field left empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _invoiceService.CreateInvoice(newEntity));
	}

	[Fact]
	public async Task UpdateMemo_Success()
	{
		// Arrange
		var existing = _db.Invoice.First();
		var request = new UpdateMemoReq();
		request.Id = existing.Id;
		request.Memo = "Test_Memo_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act
		await _invoiceService.UpdateMemo(request);

		// Assert
	}

	[Fact]
	public async Task UpdateMemo_NotFound()
	{
		// Arrange
		var request = new UpdateMemoReq();
		request.Id = Guid.NewGuid();
		request.Memo = "Test_Memo_" + Guid.NewGuid().ToString().Substring(0, 8);

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _invoiceService.UpdateMemo(request));
	}

	[Fact]
	public async Task UpdateMemo_ValidationError()
	{
		// Arrange
		var existing = _db.Invoice.First();
		var request = new UpdateMemoReq();
		request.Id = existing.Id;
		request.Memo = string.Empty; // Required field empty

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _invoiceService.UpdateMemo(request));
	}

	[Fact]
	public async Task UpdateAmount_Success()
	{
		// Arrange
		var existing = _db.Invoice.First();
		var request = new UpdateAmountReq();
		request.Id = existing.Id;
		request.Amount = 42.50m;

		// Act
		await _invoiceService.UpdateAmount(request);

		// Assert
	}

	[Fact]
	public async Task UpdateAmount_NotFound()
	{
		// Arrange
		var request = new UpdateAmountReq();
		request.Id = Guid.NewGuid();
		request.Amount = 42.50m;

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _invoiceService.UpdateAmount(request));
	}

	[Fact]
	public async Task Delete_Success()
	{
		// Arrange
		var existing = _db.Invoice.First();
		var idToDelete = existing.Id;

		// Act
		await _invoiceService.DeleteInvoice(idToDelete);

		// Assert
		var deleted = await _invoiceService.GetById(idToDelete);
		Assert.Null(deleted);
	}

	[Fact]
	public async Task Delete_NotFound()
	{
		// Arrange
		var invalidId = Guid.NewGuid();

		// Act & Assert
		await Assert.ThrowsAnyAsync<Exception>(async () => await _invoiceService.DeleteInvoice(invalidId));
	}
}
