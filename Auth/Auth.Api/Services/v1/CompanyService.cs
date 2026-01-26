using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dyvenix.App1.Data.Shared.Entities;
using Dyvenix.App1.Data.Context;

namespace Dyvenix.App1.Auth.Api.v1.Services;

public interface ICompanyService
{
	Task<Guid> CreateCompany(Company company);
	Task<bool> DeleteCompany(Guid id);
	Task UpdateCompany(Company company);
}

public partial class CompanyService : ICompanyService
{
		private readonly ILogger _logger;
		private readonly App1Db _db;
		private readonly IDbContextFactory _dbContextFactory;
	
		#region Create
	
		public async Task<Guid> CreateCompany(Company company)
		{
			ArgumentNullException.ThrowIfNull(company);
	
			try {
				using var db = _dbContextFactory.CreateDbContext();
				db.Add(company);
				await db.SaveChangesAsync();
	
				return company.Id;
	
			} catch (DbUpdateConcurrencyException) {
				throw new ConcurrencyApiException("The item was modified or deleted by another user.", _logger.CorrelationId);
			}
		}
	
		#endregion
	
		#region Delete
	
		public async Task<bool> DeleteCompany(Guid id)
		{
			using var db = _dbContextFactory.CreateDbContext();
	
			var result = await db.Company.Where(a => a.Id == id).ExecuteDeleteAsync();
			return result == 1;
		}
	
		#endregion
	
		#region Update
	
		public async Task UpdateCompany(Company company)
		{
			ArgumentNullException.ThrowIfNull(company);
	
			using var db = _dbContextFactory.CreateDbContext();
	
			try {
				db.Attach(company);
				db.Entry(company).State = EntityState.Modified;
				await db.SaveChangesAsync();
	
			} catch (DbUpdateConcurrencyException) {
				throw new ConcurrencyApiException("The item was modified or deleted by another user.", _logger.CorrelationId);
			}
		}
	
		#endregion
	}
}
