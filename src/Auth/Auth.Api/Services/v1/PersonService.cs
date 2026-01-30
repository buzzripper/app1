using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;
using Dyvenix.App1.Common.Data.Shared.Entities;
using Dyvenix.App1.Common.Data;
using Dyvenix.App1.Auth.Shared.Queries.v1;
using Dyvenix.App1.Auth.Shared.DTOs.v1;

namespace Dyvenix.App1.Auth.Api.Services.v1;

public interface IPersonService
{
	Task<Guid> CreatePerson(Person person);
	Task<bool> DeletePerson(Guid id);
	Task UpdatePerson(Person person);
	Task UpdateMethod1(Guid id, string firstName);
	Task UpdateMethod2(Guid id, string lastName, string email);
	Task<Person> ReadMethod1(Guid id);
	Task<List<Person>> ReadMethod2(string firstName);
	Task<List<Person>> ReadMethod3(string lastName, int pageSize = 0, int pageOffset = 0);
	Task<EntityList<Person>>ReadMethod4(ReadMethod4Query query);
}

public partial class PersonService : IPersonService
{
	private readonly ILogger _logger;
	private readonly App1Db _db;
	private readonly IDbContextFactory<App1Db> _dbContextFactory;

	#region Create

	public async Task<Guid> CreatePerson(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);

		try {
			using var db = _dbContextFactory.CreateDbContext();
			db.Add(person);
			await db.SaveChangesAsync();

			return person.Id;

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyApiException("The item was modified or deleted by another user.");
		}
	}

	#endregion

	#region Delete

	public async Task<bool> DeletePerson(Guid id)
	{
		using var db = _dbContextFactory.CreateDbContext();

		var result = await db.Person.Where(a => a.Id == id).ExecuteDeleteAsync();
		return result == 1;
	}

	#endregion

	#region Update

	public async Task UpdatePerson(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);

		using var db = _dbContextFactory.CreateDbContext();

		try {
			db.Attach(person);
			db.Entry(person).State = EntityState.Modified;
			await db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyApiException("The item was modified or deleted by another user.");
		}
	}

	public async Task UpdateMethod1(Guid id, string firstName)
	{
		ArgumentNullException.ThrowIfNull(firstName);

		try {
			var person = new Person {
				Id = id,
				FirstName = firstName,
			};

			using var db = _dbContextFactory.CreateDbContext();
			db.Attach(person);
			db.Entry(person).Property(u => u.FirstName).IsModified = true;

			await db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyApiException("The item was modified or deleted by another user.");
		}
	}

	public async Task UpdateMethod2(Guid id, string lastName, string email)
	{
		ArgumentNullException.ThrowIfNull(lastName);
		ArgumentNullException.ThrowIfNull(email);

		try {
			var person = new Person {
				Id = id,
				LastName = lastName,
				Email = email,
			};

			using var db = _dbContextFactory.CreateDbContext();
			db.Attach(person);
			db.Entry(person).Property(u => u.LastName).IsModified = true;
			db.Entry(person).Property(u => u.Email).IsModified = true;

			await db.SaveChangesAsync();

		} catch (DbUpdateConcurrencyException) {
			throw new ConcurrencyApiException("The item was modified or deleted by another user.");
		}
	}

	#endregion
	
	#region Single Methods
	
	public async Task<Person> ReadMethod1(Guid id)
	{
		var dbQuery = _dbContextFactory.CreateDbContext().Person.AsQueryable();
	
		dbQuery = dbQuery.Where(x => x.Id == id);
	
		return await dbQuery.AsNoTracking().FirstOrDefaultAsync();
	}
	
	#endregion
	
	#region List Methods
	
	public async Task<List<Person>> ReadMethod2(string firstName)
	{
		var dbQuery = _dbContextFactory.CreateDbContext().Person.AsQueryable();
	
		if (!string.IsNullOrWhiteSpace(firstName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.FirstName, firstName));
	
		return await dbQuery.AsNoTracking().ToListAsync();
	}
	
	public async Task<List<Person>> ReadMethod3(string lastName, int pageSize = 0, int pageOffset = 0)
	{
		var dbQuery = _dbContextFactory.CreateDbContext().Person.AsQueryable();
	
		if (!string.IsNullOrWhiteSpace(lastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, lastName));
		if (pageSize > 0)
			dbQuery = dbQuery.Skip(pageOffset * pageSize).Take(pageSize);
	
		return await dbQuery.AsNoTracking().ToListAsync();
	}
	
	#endregion

	#region Query Methods

	public async Task<EntityList<Person>>ReadMethod4(ReadMethod4Query query)
	{
		var dbQuery = _dbContextFactory.CreateDbContext().Person.AsQueryable();
		var result = new EntityList<Person>();

		// Filters
		if (!string.IsNullOrWhiteSpace(query.LastName))
			dbQuery = dbQuery.Where(x => EF.Functions.Like(x.LastName, query.LastName));

		// Paging
		if (query.RecalcRowCount || query.GetRowCountOnly)
			result.TotalRowCount = dbQuery.Count();
		if (query.GetRowCountOnly)
			return result;
		if (query.PageSize > 0)
			dbQuery = dbQuery.Skip(query.PageOffset).Take(query.PageSize);

		result.Data = await dbQuery.AsNoTracking().ToListAsync();

		return result;
	}

	#endregion
}
