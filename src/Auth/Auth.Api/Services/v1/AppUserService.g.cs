//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 11:12 AM. Any changes made to it will be lost.
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

namespace Dyvenix.App1.Auth.Api.Services.v1;

public partial class AppUserService : IAppUserService
{
	private readonly ILogger<AppUserService> _logger;
	private readonly App1Db _db;

	public AppUserService(App1Db db, ILogger<AppUserService> logger)
	{
		_db = db;
		_logger = logger;
	}

	#region Create

	public async Task CreateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);

		try {
			_db.Add(appUser);
			await _db.SaveChangesAsync();
			return;
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task DeleteAppUser(Guid id)
	{
		var rowsAffected = await _db.AppUser.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			throw new NotFoundException($"AppUser {id} not found");
	}

	#endregion

	#region Update

	public async Task UpdateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);

		try {
			_db.Attach(appUser);
			_db.Entry(appUser).State = EntityState.Modified;
			await _db.SaveChangesAsync();

			return;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	public async Task UpdateUsername(UpdateUsernameReq request)
	{
		ArgumentNullException.ThrowIfNull(request);

		try {
			var appUser = new AppUser {
				Id = request.Id,
				Username = request.Username,
			};

			_db.Attach(appUser);
			_db.Entry(appUser).Property(u => u.Username).IsModified = true;

			await _db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<AppUser> GetById(Guid id)
	{
		var dbQuery = _db.AppUser.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var appUser = await dbQuery.FirstOrDefaultAsync();
	
		if (appUser is null)
			throw new NotFoundException($"AppUser not found");
	
		return appUser;
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<List<AppUser>> ReqByUsername(ReqByUsernameReq request)
	{
		var dbQuery = _db.AppUser.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.Username))
			dbQuery = dbQuery.Where(x => x.Username == request.Username);
	
		var data = await dbQuery.ToListAsync();
	
		return data;
	}
	
	#endregion
}
