//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.App.Shared.Dtos;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.App.Shared.Contracts.v1;
using Dyvenix.App1.App.Shared.Requests.v1;
using Dyvenix.App1.Common.Shared.Extensions;
using Dyvenix.App1.Common.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.Contracts.v1;

public interface IClientService
{
	Task<byte[]> CreateClient(CreateClientReq request);
	Task<byte[]> UpdateClient(UpdateClientReq request);
	Task<byte[]> UpdateClientBaseUrl(UpdateClientBaseUrlReq request);
	Task DeleteClient(Guid id);
	Task<ClientDto> GetClientById(Guid id);
	Task<ClientDto> GetClientByKey(string key);
	Task<IReadOnlyList<ClientLookupDto>> GetAllClientLookupItems(GetAllClientLookupItemsReq request);
	Task<IReadOnlyList<ClientRouteDto>> GetAllClientRoutes();
	Task<IReadOnlyList<ClientDto>> GetAllClients(GetAllClientsReq request);
	Task<ListPage<ClientLookupDto>> SearchClientsByName(SearchClientsByNameReq request);
}
