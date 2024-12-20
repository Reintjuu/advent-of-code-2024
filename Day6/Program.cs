using System.Text;

public class Program
{
	private static readonly Dictionary<char, char> _nextDirectionMap = new()
	{
		['^'] = '>',
		['>'] = 'v',
		['v'] = '<',
		['<'] = '^'
	};

	public static async Task Main(string[] args)
	{
		var input = await File.ReadAllTextAsync("../../../input");

		WalkGuardAndDetermineUniquePositionsAndLoops(input);
	}

	public static void WalkGuardAndDetermineUniquePositionsAndLoops(string input)
	{
		string[] rowContent = input.Trim().Split('\n');

		var rows = rowContent.Length;
		var columns = rowContent[0].Length;
		var grid = new char[rows, columns];

		(int y, int x) startingGuardPosition = (0, 0);
		for (var y = 0; y < rows; y++)
		{
			for (var x = 0; x < columns; x++)
			{
				if (rowContent[y][x] == '^')
				{
					startingGuardPosition = (y, x);
				}

				grid[y, x] = rowContent[y][x];
			}
		}

		var gridWithNoObstacles = (char[,])grid.Clone();
		Console.WriteLine(
			$"Total distinct positions for original grid: {WalkGuard(gridWithNoObstacles, rows, columns, startingGuardPosition).totalDistinctPositions}");

		var loops = 0;
		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{
				if (grid[y, x] == '.')
				{
					var copy = (char[,])grid.Clone();
					copy[y, x] = 'O';
					if (WalkGuard(copy, rows, columns, startingGuardPosition).isLoop)
					{
						loops++;
					}
				}
			}
		}

		Console.WriteLine($"Unique obstacle places to get guard in a loop: {loops}");
	}

	private static (int totalDistinctPositions, bool isLoop) WalkGuard(
		char[,] grid, int rows, int columns, (int y, int x) currentGuardPosition)
	{
		HashSet<((int y, int x), char direction)> visited = [];

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
				case '#' or 'O':
					grid[currentGuardPosition.y, currentGuardPosition.x] = _nextDirectionMap[currentGuardDirection];
					continue;
				case '.':
					totalDistinctPositions++;
					break;
			}

			grid[currentGuardPosition.y, currentGuardPosition.x] = 'X';
			grid[newGuardPosition.y, newGuardPosition.x] = currentGuardDirection;
			currentGuardPosition = newGuardPosition;

			var visitInfo = (currentGuardPosition, currentGuardDirection);
			if (!visited.Add(visitInfo))
			{
				return (totalDistinctPositions, isLoop: true);
			}

			// ShowGrid(grid, rows, columns);
		}

		return (totalDistinctPositions, isLoop: false);
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
