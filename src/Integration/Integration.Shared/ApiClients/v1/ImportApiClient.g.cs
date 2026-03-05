//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.Common.Shared.ApiClients;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.ApiClients.v1;

public partial class ImportApiClient : ApiClientBase, IImportService
{
	public ImportApiClient(HttpClient httpClient) : base(httpClient)
	{
	}
	
    public async Task<string> ImportMethod1()
    {
        return await GetAsync<string>($"api/integration/v1/import/importmethod1");
    }

    public async Task<string> ImportMethod2()
    {
        return await GetAsync<string>($"api/integration/v1/import/importmethod2");
    }

    public async Task<string> ImportMethod3()
    {
        return await GetAsync<string>($"api/integration/v1/import/importmethod3");
    }
}
