using Dyvenix.App1.Common.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dyvenix.App1.Common.Data;

public sealed class AuditingInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(
		DbContextEventData eventData,
		InterceptionResult<int> result)
	{
		ApplyAuditValues(eventData.Context);
		return result;
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		ApplyAuditValues(eventData.Context);
		return ValueTask.FromResult(result);
	}

	private void ApplyAuditValues(DbContext? dbContext)
	{
		if (dbContext is null)
			return;

		dbContext.ChangeTracker.DetectChanges();

		var utcNow = DateTime.UtcNow;
		var userId = currentUser.UserId;

		foreach (var entry in dbContext.ChangeTracker.Entries<IAuditable>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedUtc = utcNow;
				entry.Entity.CreatedByUserId = userId;
				entry.Entity.ModifiedUtc = utcNow;
				entry.Entity.ModifiedByUserId = userId;
			}
			else if (entry.State == EntityState.Modified)
			{
				entry.Entity.ModifiedUtc = utcNow;
				entry.Entity.ModifiedByUserId = userId;

				entry.Property(x => x.CreatedUtc).IsModified = false;
				entry.Property(x => x.CreatedByUserId).IsModified = false;
			}
		}
	}
}
