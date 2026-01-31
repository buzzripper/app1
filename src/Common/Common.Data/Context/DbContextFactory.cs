//using Dyvenix.App1.Common.Data.Config;
//using Microsoft.EntityFrameworkCore;

//namespace Dyvenix.App1.Common.Data.Context;

//public interface IDbContextFactory<T>
//{
//	T CreateDbContext();
//}

//public class DbContextFactory<T> : IDbContextFactory<T> where T : DbContext, new()
//{
//	protected readonly DataConfig _dataConfig;

//	public DbContextFactory(DataConfig dataConfig)
//	{
//		_dataConfig = dataConfig;
//	}

//	public T CreateDbContext()
//	{
//		var b = new DbContextOptionsBuilder<T>();
//		b.UseSqlServer(_dataConfig.ConnectionString);

//		return new T(b.Options);
//	}
//}