//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface ITenantService
{
	Task DeleteTenant(Guid id);
	Task UpdateName(UpdateNameReq request);
	Task<Dto2> GetById(Guid id);
	Task<IReadOnlyList<Dto1>> GetAll();
}
