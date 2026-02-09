//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/9/2026 10:08 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Tests.Integration.DataSets;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Tests.Integration.Data;

public interface IDataManager : IDisposable
{
	Dictionary<DataSetType, IDataSet> DataSets { get; }
	Task<IDataSet> Reset(DataSetType dataSetType);
}

public class DataManager : IDataManager
{
	private readonly App1Db _db;

	public DataManager(App1Db db)
	{
		_db = db;
		DataSets.Add(DataSetType.Default, new DefaultDataSet());
	}

	public void Dispose()
	{
		_db?.Dispose();
	}

	public Dictionary<DataSetType, IDataSet> DataSets { get; } = [];

	public async Task<IDataSet> Reset(DataSetType dataSetType)
	{
		var dataSet = this.DataSets[dataSetType];

		await DeleteAllData();
		await InsertAllData(dataSet);

		return dataSet;
	}

	private async Task DeleteAllData()
	{
		await _db.Invoice.ExecuteDeleteAsync();
		await _db.Patient.ExecuteDeleteAsync();
		await _db.AppUser.ExecuteDeleteAsync();

		await _db.SaveChangesAsync();
	}

	private async Task InsertAllData(IDataSet dataSet)
	{
		await _db.Patient.AddRangeAsync(dataSet.PatientList);
		await _db.SaveChangesAsync();
		await _db.Invoice.AddRangeAsync(dataSet.InvoiceList);
		await _db.SaveChangesAsync();
		await _db.AppUser.AddRangeAsync(dataSet.AppUserList);
		await _db.SaveChangesAsync();
	}
}
