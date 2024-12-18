using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
	public static async Task Main(string[] args)
	{
		// all increasing or  all decreasing
		// at least 1 and at most 3

		var reports = (await File.ReadAllLinesAsync("../../../input"))
			.Select(values => values.Split(' ').Select(int.Parse));

		Console.WriteLine(CalculateSafeLevelAmount(reports));
	}

	public static int CalculateSafeLevelAmount(IEnumerable<IEnumerable<int>> reports)
	{
		return reports
			.Select(level => new LinkedList<int>(level))
			.Count(report => IsSafe(report.EnumerateNodes())
				|| Enumerable.Range(0, report.Count)
					.Any(indexToFilter => IsSafe(new LinkedList<int>(report.Where((_, reportIndex) => indexToFilter != reportIndex))
						.EnumerateNodes())));
	}

	private static bool IsSafe(IEnumerable<LinkedListNode<int>> report)
	{
		var levels = report as LinkedListNode<int>[] ?? report.ToArray();
		var allIncreasing = levels.All(level => level.Next == null || level.Value > level.Next.Value);

		var allDecreasing = levels.All(level => level.Next == null || level.Value < level.Next.Value);

		var allInBounds = levels.All(level =>
			level.Next == null || Math.Abs(level.Value - level.Next.Value) is >= 1 and <= 3);

		return (allIncreasing || allDecreasing) && allInBounds;
	}
}

internal static class LinkedListExtensions
{
	public static IEnumerable<LinkedListNode<T>> EnumerateNodes<T>(this LinkedList<T> list)
	{
		var node = list.First;
		while (node != null)
		{
			yield return node;
			node = node.Next;
		}
	}
}
