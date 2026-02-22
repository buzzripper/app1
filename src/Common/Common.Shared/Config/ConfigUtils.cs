namespace Dyvenix.App1.Common.Shared.Config;

public static class ConfigUtils
{
	private static readonly HashSet<Type> AllowedTypes = new()
	{
		typeof(string),
		typeof(int),
		typeof(bool)
	};

	public static T Replace<T>(string envVarName, T defaultValue)
	{
		// Validate that the type is allowed
		if (!AllowedTypes.Contains(typeof(T)))
			throw new InvalidOperationException($"Type {typeof(T).Name} is not allowed.");

		var valStr = Environment.GetEnvironmentVariable(envVarName);
		if (string.IsNullOrWhiteSpace(valStr))
			return defaultValue;

		return (T)Convert.ChangeType(valStr, typeof(T));
	}
}
