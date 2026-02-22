
public class TestUtils
{
	private readonly Random _random = new Random();

	public int Rnd(int x, int y = int.MaxValue)
	{
		if (x > y)
			throw new ArgumentException("x must be less than or equal to y");

		var yVal = y < int.MaxValue ? y : int.MaxValue - 1; // 1st arg is inclusive but 2nd arg is exclusive

		return _random.Next(x, yVal);
	}

	public bool PctChance(int x)
	{
		return _random.Next(0, 100) > x;
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

	public T RndEnum<T>() where T : Enum
	{
		var values = Enum.GetValues(typeof(T));
		return (T)values.GetValue(_random.Next(values.Length))!;
	}

}

