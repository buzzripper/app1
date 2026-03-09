//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/8/2026 11:54 PM. Any changes made to it will be lost.
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
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.Contracts.v1;

public interface IClientService
{
	Task Delete(Guid id);
	Task<byte[]> CreateClient(CreateClientReq request);
	Task<byte[]> UpdateClient(UpdateClientReq request);
	Task<byte[]> UpdateClientBaseUrl(UpdateClientBaseUrlReq request);
	Task<ClientDto> GetClientById(Guid id);
	Task<ClientDto> GetClientByKey(string key);
	Task<IReadOnlyList<ClientOptionDto>> GetAllClientLookupItems(GetAllClientLookupItemsReq request);
	Task<IReadOnlyList<ClientRouteDto>> GetAllRoutes();
	Task<IReadOnlyList<ClientDto>> GetAllClients(GetAllClientsReq request);
}
