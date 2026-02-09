//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/9/2026 10:08 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Auth.Shared.Requests.v1;

namespace Dyvenix.App1.Auth.Services.v1;

public interface IAppUserService
{
	Task<Result<Guid>> CreateAppUser(AppUser appUser);
	Task<Result> DeleteAppUser(Guid id);
	Task<Result> UpdateAppUser(AppUser appUser);
	Task<Result> UpdateUsername(Guid id, string username);
	Task<Result<AppUser>> GetById(Guid id);
	Task<Result<List<AppUser>>> ReqByUsername(ReqByUsernameReq request);
}

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

	public async Task<Result<Guid>> CreateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);

		try {
			_db.Add(appUser);
			await _db.SaveChangesAsync();

			return Result<Guid>.Ok(appUser.Id);

		} catch (DbUpdateConcurrencyException) {
			return Result<Guid>.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task<Result> DeleteAppUser(Guid id)
	{
		var rowsAffected = await _db.AppUser.Where(a => a.Id == id).ExecuteDeleteAsync();

		if (rowsAffected == 0)
			return Result.NotFound($"AppUser {id} not found");

		return Result.Ok();
	}

	#endregion

	#region Update

	public async Task<Result> UpdateAppUser(AppUser appUser)
	{
		ArgumentNullException.ThrowIfNull(appUser);

		try {
			_db.Attach(appUser);
			_db.Entry(appUser).State = EntityState.Modified;
			await _db.SaveChangesAsync();

		return Result.Ok();
		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	public async Task<Result> UpdateUsername(Guid id, string username)
	{
		ArgumentNullException.ThrowIfNull(username);

		try {
			var appUser = new AppUser {
				Id = id,
				Username = username,
			};

			_db.Attach(appUser);
			_db.Entry(appUser).Property(u => u.Username).IsModified = true;

			await _db.SaveChangesAsync();

			return Result.Ok();

		} catch (DbUpdateConcurrencyException) {
			return Result.Conflict("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Read - Single
	
	public async Task<Result<AppUser>> GetById(Guid id)
	{
		var dbQuery = _db.AppUser.AsNoTracking();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		var appUser = await dbQuery.FirstOrDefaultAsync();
	
		if (appUser is null)
			return Result<AppUser>.NotFound($"AppUser not found");
	
		return Result<AppUser>.Ok(appUser);
	}
	
	#endregion
	
	#region Read - List
	
	public async Task<Result<List<AppUser>>> ReqByUsername(ReqByUsernameReq request)
	{
		var dbQuery = _db.AppUser.AsNoTracking();
	
		if (!string.IsNullOrWhiteSpace(request.Username))
			dbQuery = dbQuery.Where(x => x.Username == request.Username);
	
		var data = await dbQuery.ToListAsync();
	
		return Result<List<AppUser>>.Ok(data);
	}
	
	#endregion
}
