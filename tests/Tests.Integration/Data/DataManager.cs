//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.App.Api.Context;
using Dyvenix.App1.Tests.Integration.DataSets;

namespace Dyvenix.App1.Tests.Integration.Data;

public interface IDataManager : IDisposable
{
	Dictionary<string, TestDataSet> DataSets { get; }
	Task<TestDataSet> Reset(string dataSetName);
	Task Initialize();
}

public class DataManager : IDataManager
{
	private readonly App1Db _db;

	public DataManager(App1Db db)
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
		await _db.Client.ExecuteDeleteAsync();
	}

	private async Task InsertAllData(TestDataSet dataSet)
	{
		await _db.Client.AddRangeAsync(dataSet.ClientList);
		await _db.SaveChangesAsync();
	}
}
