using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.Common.Shared.Extensions;

public static class ListTExtensions
{
	public static EntityList<T>? ToEntityList<T>(this List<T> list) where T : class, new()
	{
		return new EntityList<T>(list);
	}
}
