using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
	public static async Task Main()
	{
		var input = await File.ReadAllTextAsync("../../../input");

		string[] rowContent = input.Trim().Split('\n');

		var rows = rowContent.Length;
		var columns = rowContent[0].Length;
		var grid = new char[rows, columns];

		for (var y = 0; y < rows; y++)
		{
			for (var x = 0; x < columns; x++)
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
		Func<int, int, int, bool> noMatch = (yToCheck, xToCheck, offset) => grid[yToCheck, xToCheck] != word[offset];

		for (var y = 0; y < rows; y++)
		{
			for (var x = 0; x < columns; x++)
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
			yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - 1 + o, (xx, o) => xx - 1 + o, noMatch)
				&& IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - 1 + o, (xx, o) => xx + 1 - o, noMatch);

			// S M
			//  A
			// S M
			yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - 1 + o, (xx, o) => xx + 1 - o, noMatch)
				&& IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + 1 - o, (xx, o) => xx + 1 - o, noMatch);

			// S S
			//  A
			// M M
			yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + 1 - o, (xx, o) => xx - 1 + o, noMatch)
				&& IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + 1 - o, (xx, o) => xx + 1 - o, noMatch);

			// M S
			//  A
			// M S
			yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - 1 + o, (xx, o) => xx - 1 + o, noMatch)
				&& IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + 1 - o, (xx, o) => xx - 1 + o, noMatch);
		}
	}

	private static IEnumerable<bool> MatchCrosswordXmas(char[,] grid, int rows, int columns)
	{
		var word = new[] { 'X', 'M', 'A', 'S' };
		Func<int, int, int, bool> noMatch = (yToCheck, xToCheck, offset) => grid[yToCheck, xToCheck] != word[offset];

		for (var y = 0; y < rows; y++)
		{
			for (var x = 0; x < columns; x++)
			{
				// horizontal
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, offset) => yy + offset, (xx, _) => xx, noMatch);
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, offset) => yy - offset, (xx, _) => xx, noMatch);

				// vertical
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, _) => yy, (xx, offset) => xx + offset, noMatch);
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, _) => yy, (xx, offset) => xx - offset, noMatch);

				// diagonal
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + o, (xx, o) => xx + o, noMatch);
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy + o, (xx, o) => xx - o, noMatch);
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - o, (xx, o) => xx - o, noMatch);
				yield return IsMatch(word.Length, rows, columns, y, x, (yy, o) => yy - o, (xx, o) => xx + o, noMatch);
			}
		}
	}

	private static bool IsMatch(
		int reach,
		int rows, int columns,
		int y, int x,
		Func<int, int, int> yOperation,
		Func<int, int, int> xOperation,
		Func<int, int, int, bool> noMatch)
	{
		for (var letterIndex = 0; letterIndex < reach; letterIndex++)
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
