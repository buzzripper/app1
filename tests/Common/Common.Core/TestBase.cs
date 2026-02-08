namespace Dyvenix.App1.Tests.Common.Core;

public class TestBase
{
	#region Fields

	private static readonly Random _random = new Random();

	#endregion

	#region Methods

	public static string RndStr(int maxLength)
	{
		if (maxLength > 0 && maxLength <= 36)
		{
			return Guid.NewGuid().ToString().Substring(0, maxLength);
		}
		else
		{
			return Guid.NewGuid().ToString();
		}
	}

	public int RndInt()
	{
		return _random.Next();
	}

	public int RndInt(int min)
	{
		return _random.Next(min);
	}

	public int RndInt(int min, int max)
	{
		return _random.Next(min, max + 1);
	}

	public bool RndBool()
	{
		return _random.Next(2) == 0;
	}

	public long RndLong()
	{
		return _random.Next();
	}

	public long RndLong(int min)
	{
		return _random.Next(min);
	}

	public long RndLong(int min, int max)
	{
		return _random.Next(min, max + 1);
	}

	public double RndDbl(double min, double max)
	{
		return min + _random.NextDouble() * (max - min);
	}

	public static T RndEnum<T>() where T : Enum
	{
		var values = Enum.GetValues(typeof(T));
		return (T)values.GetValue(_random.Next(values.Length))!;
	}

	#endregion
}
