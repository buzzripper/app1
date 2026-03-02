//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
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
	Task<byte[]> Create(CreateReq request);
	Task<ClientDto> GetById(Guid id);
	Task<ClientDto> GetByKey(string key);
	Task<IReadOnlyList<ClientOptionDto>> GetAllClientOptions(GetAllClientOptionsReq request);
	Task<IReadOnlyList<ClientRouteDto>> GetAllRoutes();
}
