//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
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

	#region Delete

	public async Task DeleteTenant(Guid id)
	{
		var rowsAffected = await _db.Tenant.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"Tenant {id} not found");
	}

	#endregion

	#region Update

	public async Task UpdateName(UpdateNameReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var tenant = new Tenant {
				Id = request.Id,
				Name = request.Name,
			};

			_db.Attach(tenant);
			_db.Entry(tenant).Property(u => u.Name).IsModified = true;

			await _db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Dto2> GetById(Guid id)
	{
		var dbQuery = _db.Tenant.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		return await dbQuery.Select(e => new Dto2(
			e.AuthMode,
			e.IsActive,
			e.CreatedAt,
			e.ExternalAuthority,
			e.ExternalClientSecret,
			e.ADDomain
		))
		.SingleOrDefaultAsync();
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<IReadOnlyList<Dto1>> GetAll()
	{
		var dbQuery = _db.Tenant.AsNoTracking();
	
		return await dbQuery.Select(e => new Dto1(
			e.Id,
			e.Name,
			e.Slug,
			e.ExternalClientId,
			e.ExternalClientSecret,
			e.ADDcHost
		))
		.ToListAsync();
	}
	
	#endregion
}
