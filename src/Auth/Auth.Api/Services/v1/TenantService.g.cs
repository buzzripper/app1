//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/28/2026 11:36 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Auth.Data.Context;
using Dyvenix.App1.Auth.Shared.DTOs;
using Dyvenix.App1.Auth.Data.Entities;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Auth.Shared.Contracts.v1;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Api.Services.v1;

public partial class TenantService : ITenantService
{
	private readonly ILogger<TenantService> _logger;
	private readonly AuthDbContext _db;

	public TenantService(AuthDbContext db, ILogger<TenantService> logger)
	{
		_db = db;
		_logger = logger;
	}
}
