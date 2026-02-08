using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Tests.Common.Core.Data;

public interface IDataManager : IDisposable
{
	Dictionary<DataSetType, DataSet> DataSets { get; }
	Task<DataSet> ResetDataSet(DataSetType dataSetType);
}

public class DataManager : IDataManager
{
	#region Fields

	private readonly Db _db;

	#endregion

	#region Ctors / Init

	public DataManager(IDbContextFactory dbContextFactory)
	{
		_db = dbContextFactory.CreateDbContext();

		DataSets.Add(DataSetType.Default, new DefaultDataGenerator().CreateDataSet());
	}

	public void Dispose()
	{
		_db?.Dispose();
	}

	#endregion

	public Dictionary<DataSetType, DataSet> DataSets { get; } = [];

	#region Methods

	public async Task<DataSet> ResetDataSet(DataSetType dataSetType)
	{
		var dataSet = this.DataSets[dataSetType];

		// Clear out existing data and add test data
		await DeleteAllData();

		await _db.AppUser.AddRangeAsync(dataSet.AppUser);
		await _db.SaveChangesAsync();

		return dataSet;
	}

	private async Task DeleteAllData()
	{
		await _db.AccessClaim.ExecuteDeleteAsync();
		await _db.AppUser.ExecuteDeleteAsync();

		await _db.SaveChangesAsync();
	}

	#endregion
}
