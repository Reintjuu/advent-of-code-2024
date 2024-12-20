using System.Text;

public class Program
{
	public static async Task Main(string[] args)
	{
		var input = await File.ReadAllTextAsync("../../../input");

		Console.WriteLine(Result(input));
	}

	public static int Result(string input)
	{
		string[] rowContent = input.Trim().Split('\n');

		var rows = rowContent.Length;
		var columns = rowContent[0].Length;
		var grid = new char[rows, columns];

		(int y, int x) currentGuardPosition = (0, 0);
		for (var y = 0; y < rows; y++)
		{
			for (var x = 0; x < columns; x++)
			{
				if (rowContent[y][x] == '^')
				{
					currentGuardPosition = (y, x);
				}

				grid[y, x] = rowContent[y][x];
			}
		}

		Dictionary<char, char> nextDirectionMap = new()
		{
			['^'] = '>',
			['>'] = 'v',
			['v'] = '<',
			['<'] = '^'
		};

		var totalDistinctPositions = 0;
		while (true)
		{
			var currentGuardDirection = grid[currentGuardPosition.y, currentGuardPosition.x];
			var guardTranslation = currentGuardDirection switch
			{
				'^' => (y: -1, x: 0),
				'>' => (y: 0, x: 1),
				'v' => (y: 1, x: 0),
				'<' => (y: 0, x: -1),
				_ => throw new ArgumentOutOfRangeException()
			};

			var newGuardPosition = (
				y: currentGuardPosition.y + guardTranslation.y,
				x: currentGuardPosition.x + guardTranslation.x);

			// Out of bounds.
			if (newGuardPosition.y < 0 || newGuardPosition.y >= rows || newGuardPosition.x < 0 || newGuardPosition.x >= columns)
			{
				totalDistinctPositions++;
				break;
			}

			var tileContent = grid[newGuardPosition.y, newGuardPosition.x];

			switch (tileContent)
			{
				// Box
				case '#':
					grid[currentGuardPosition.y, currentGuardPosition.x] = nextDirectionMap[currentGuardDirection];
					continue;
				case '.':
					totalDistinctPositions++;
					break;
			}

			grid[currentGuardPosition.y, currentGuardPosition.x] = 'X';
			grid[newGuardPosition.y, newGuardPosition.x] = currentGuardDirection;
			currentGuardPosition = newGuardPosition;

			ShowGrid(grid, rows, columns);
		}

		return totalDistinctPositions;
	}

	private static void ShowGrid(char[,] grid, int rows, int columns)
	{
		StringBuilder sb = new();
		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				sb.Append(grid[y, x]);
			}

			sb.AppendLine();
		}

		sb.AppendLine();

		Console.WriteLine(sb.ToString());
	}
}
