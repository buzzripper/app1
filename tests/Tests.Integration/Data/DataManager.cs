//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Auth.Data.Context;
using Dyvenix.Auth.Tests.Integration.DataSets;

namespace Dyvenix.Auth.Tests.Integration.Data;

public interface IDataManager : IDisposable
{
	Dictionary<string, TestDataSet> DataSets { get; }
	Task<TestDataSet> Reset(string dataSetName);
	Task Initialize();
}

public class DataManager : IDataManager
{
	private readonly AuthDbContext _db;

	public DataManager(AuthDbContext db)
	{
		_db = db;
	}

	public void Dispose()
	{
		_db?.Dispose();
	}

	public Dictionary<string, TestDataSet> DataSets { get; } = [];

	public async Task Initialize()
	{
		// Each json file is a dataset. The "Default" can optionally hold common data that should be included in all datasets.
		var testDataRootDir = Path.Combine(AppContext.BaseDirectory, "TestData");
		var defaultDataDir = Path.Combine(testDataRootDir, "Default");

		var jsonFilepaths = Directory.GetFiles(testDataRootDir, "*.json", SearchOption.TopDirectoryOnly);
		foreach (var jsonFilepath in jsonFilepaths)
		{
			var dataSetName = Path.GetFileNameWithoutExtension(jsonFilepath);
			var dataSet = await LoadDataSet(jsonFilepath);
			DataSets.Add(dataSetName, dataSet);
		}
	}

	public async Task<TestDataSet> LoadDataSet(string jsonFilepath)
	{
		try
		{
			using var reader = new StreamReader(jsonFilepath);
			var json = await reader.ReadToEndAsync();
			var dataSet = JsonSerializer.Deserialize<TestDataSet>(json);
			if (dataSet == null)
				throw new Exception($"Error attempting to load dataset file {jsonFilepath}. Returned null.");
			return dataSet;
		}
		catch (Exception ex)
		{
			throw new Exception($"Error loading dataset from file '{jsonFilepath}': {ex.Message}", ex);
		}
	}

	public async Task<TestDataSet> Reset(string dataSetName)
	 {
		var dataSet = this.DataSets[dataSetName];

		await DeleteAllData();
		await InsertAllData(dataSet);

		return dataSet;
	}

	private async Task DeleteAllData()
	{
		await _db.Tenant.ExecuteDeleteAsync();
	}

	private async Task InsertAllData(TestDataSet dataSet)
	{
		await _db.Tenant.AddRangeAsync(dataSet.TenantList);
		await _db.SaveChangesAsync();
	}
}
