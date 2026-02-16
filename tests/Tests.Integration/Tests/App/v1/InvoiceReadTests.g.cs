//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/15/2026 7:07 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using Dyvenix.App1.Tests.Integration.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Dyvenix.App1.Tests.Integration.Fixtures;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;

namespace Dyvenix.App1.Tests.Integration.Tests.App.v1;

public class InvoiceReadTestFixture(GlobalTestFixture globalFixture) : IAsyncLifetime
{
	public GlobalTestFixture GlobalFixture { get; } = globalFixture;
	public TestDataSet DataSet { get; private set; } = default!;

	public async ValueTask InitializeAsync()
	{
		var dataManager = GlobalFixture.Services.GetRequiredService<IDataManager>();
		DataSet = await dataManager.Reset(DataSetType.Main.ToString());
	}

	public ValueTask DisposeAsync() => default;
}

[Collection(nameof(GlobalTestCollection))]
public class InvoiceReadTests : TestBase, IClassFixture<InvoiceReadTestFixture>
{
	private readonly InvoiceReadTestFixture _fixture;
	private IInvoiceService _invoiceService = default!;

	public InvoiceReadTests(GlobalTestFixture globalFixture, InvoiceReadTestFixture fixture)
		: base(globalFixture)
	{
		_fixture = fixture;
	}

	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();
		_invoiceService = _scope.ServiceProvider.GetRequiredService<IInvoiceService>();
	}

	[Fact]
	public async Task GetById_Success()
	{
		// Arrange
		var invoiceSample = _db.Invoice.First();
		var id = invoiceSample.Id;

		// Act
		var result = await _invoiceService.GetById(id);

		// Assert
		Assert.Equal(invoiceSample.Id, result.Id);
	}

	[Fact]
	public async Task GetById_NotFound()
	{
		// Arrange
		var invoiceSample = _db.Invoice.First();
		var id = invoiceSample.Id;
		var invalidId = Guid.NewGuid();

		// Act
		var result = await _invoiceService.GetById(invalidId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task QueryByMemo_Success()
	{
		// Arrange
		var invoiceSample = _db.Invoice.First(x => !string.IsNullOrWhiteSpace(x.Memo));
		var memo = invoiceSample.Memo;
		var expectedList = _db.Invoice.Where(x => x.Memo == memo).ToList();

		// Act
		var result = await _invoiceService.QueryByMemo(memo);

		// Assert
		Assert.Equal(expectedList.Count, result.Count);
	}

	[Fact]
	public async Task QueryByMemo_NoResults()
	{
		// Arrange
		var invoiceSample = _db.Invoice.First(x => !string.IsNullOrWhiteSpace(x.Memo));
		var memo = invoiceSample.Memo;
		var invalidMemo = "__invalid__";

		// Act
		var result = await _invoiceService.QueryByMemo(invalidMemo);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task GetAll_Success()
	{
		// Arrange
		var invoiceSample = _db.Invoice.First();

		// Act
		var result = await _invoiceService.GetAll();

		// Assert
		Assert.Equal(invoiceSample.Id, result.Id);
	}
}
