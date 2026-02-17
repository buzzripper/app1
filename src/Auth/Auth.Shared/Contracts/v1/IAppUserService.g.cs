//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/16/2026 9:37 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Shared.Contracts.v1;

public interface IAppUserService
{
	Task CreateAppUser(AppUser appUser);
	Task DeleteAppUser(Guid id);
	Task UpdateAppUser(AppUser appUser);
	Task UpdateUsername(UpdateUsernameReq request);
	Task<AppUser> GetById(Guid id);
	Task<List<AppUser>> ReqByUsername(ReqByUsernameReq request);
}
