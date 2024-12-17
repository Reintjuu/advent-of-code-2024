public class Program
{
	public static async Task Main()
	{
		var input = await File.ReadAllTextAsync("../../../input");

		string[] rowContent = input.Trim().Split('\n');

		int columns = rowContent[0].Length;
		int rows = rowContent.Length;
		char[,] grid = new char[columns, rows];

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				grid[y, x] = rowContent[y][x];
			}
		}

		// Assignment 1.
		Console.WriteLine(MatchCrosswordXmas(grid, rows, columns).Count(isMatch => isMatch));
		// Assignment 2.
		Console.WriteLine(MatchXMas(grid, rows, columns).Count(isMatch => isMatch));
	}

	private static IEnumerable<bool> MatchXMas(char[,] grid, int rows, int columns)
	{
		var word = new[] { 'M', 'A', 'S' };
		Func<int,int,int,bool> noMatch = (yToCheck, xToCheck, offset) => grid[yToCheck, xToCheck] != word[offset];

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				if (grid[y, x] != 'A')
				{
					continue;
				}

				// diagonal
				yield return IsMatchingXMas(y, x).Any(match => match);
			}
		}

		IEnumerable<bool> IsMatchingXMas(int y, int x)
		{
			// M M
			//  A
			// S S
			yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - 1 + offset, (xx, offset) => xx - 1 + offset, noMatch)
				&& IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - 1 + offset, (xx, offset) => xx + 1 - offset, noMatch);

			// S M
			//  A
			// S M
			yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - 1 + offset, (xx, offset) => xx + 1 - offset, noMatch)
				&& IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + 1 - offset, (xx, offset) => xx + 1 - offset, noMatch);

			// S S
			//  A
			// M M
			yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + 1 - offset, (xx, offset) => xx - 1 + offset, noMatch)
				&& IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + 1 - offset, (xx, offset) => xx + 1 - offset, noMatch);

			// M S
			//  A
			// M S
			yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - 1 + offset, (xx, offset) => xx - 1 + offset, noMatch)
				&& IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + 1 - offset, (xx, offset) => xx - 1 + offset, noMatch);
		}
	}

	private static IEnumerable<bool> MatchCrosswordXmas(char[,] grid, int rows, int columns)
	{
		var word = new[] { 'X', 'M', 'A', 'S' };
		Func<int,int,int,bool> noMatch = (yToCheck, xToCheck, offset) => grid[yToCheck, xToCheck] != word[offset];

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				// horizontal
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + offset, (xx, _) => xx, noMatch);
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - offset, (xx, _) => xx, noMatch);

				// vertical
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, _) => yy, (xx, offset) => xx + offset, noMatch);
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, _) => yy, (xx, offset) => xx - offset, noMatch);

				// diagonal
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + offset, (xx, offset) => xx + offset, noMatch);
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy + offset, (xx, offset) => xx - offset, noMatch);
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - offset, (xx, offset) => xx - offset, noMatch);
				yield return IsMatchingWord(word.Length, rows, columns, y, x, (yy, offset) => yy - offset, (xx, offset) => xx + offset, noMatch);
			}
		}
	}

	static bool IsMatchingWord(
		int reach,
		int rows, int columns,
		int y, int x,
		Func<int, int, int> yOperation,
		Func<int, int, int> xOperation,
		Func<int, int, int, bool> noMatch)
	{
		for (int letterIndex = 0; letterIndex < reach; letterIndex++)
		{
			var yToCheck = yOperation(y, letterIndex);
			var xToCheck = xOperation(x, letterIndex);

			// Out of bounds.
			if (yToCheck < 0 || yToCheck >= rows || xToCheck < 0 || xToCheck >= columns)
			{
				return false;
			}

			if (noMatch(yToCheck, xToCheck, letterIndex))
			{
				return false;
			}
		}

		return true;
	}
}
