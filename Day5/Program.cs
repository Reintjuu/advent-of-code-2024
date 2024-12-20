using Day5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
	public static async Task Main(string[] args)
	{
		var input = await File.ReadAllTextAsync("../../../input");

		Console.WriteLine(DetermineCorrectPageOrdering(input));
	}

	public static (int correct, int incorrect) DetermineCorrectPageOrdering(string input)
	{
		var rulesAndPagesToProduce = input
			.Trim()
			.Split("\n\n");
		var pageOrderingRules = rulesAndPagesToProduce
			.First()
			.Split('\n')
			.Select(values => values.Split('|').Select(int.Parse));
		var pagesToProduceInEachUpdate = rulesAndPagesToProduce
			.Last()
			.Split('\n')
			.Select(values => values
				.Split(',')
				.Select(int.Parse)
				.ToArray())
			.ToArray();

		var pagesToProduceAccordingToRules = pagesToProduceInEachUpdate
			.Where(pagesToProduce => AllAccordingToRules(pagesToProduce, pageOrderingRules))
			.ToArray();

		var pagesNotAccordingToRules = pagesToProduceInEachUpdate
			.Except(pagesToProduceAccordingToRules)
			.Select(pagesToSort =>
			{
				var relevantRules = GetRelevantRules(pageOrderingRules, pagesToSort).ToArray();
				PageComparer pageComparer = new(relevantRules);

				return pagesToSort.Order(pageComparer).ToArray();
			})
			.ToArray();


		return (
			correct: pagesToProduceAccordingToRules.Sum(result => result[result.Length / 2]),
			incorrect: pagesNotAccordingToRules.Sum(result => result[result.Length / 2]));
	}

	private static bool AllAccordingToRules(
		int[] pagesToProduce,
		IEnumerable<IEnumerable<int>> pageOrderingRules)
	{
		var enumeratedPagesToProduce = pagesToProduce.ToArray();
		var relevantRules = GetRelevantRules(pageOrderingRules, enumeratedPagesToProduce).ToArray();

		var allAccordingToRules = enumeratedPagesToProduce
			.Select((basePage, i) => enumeratedPagesToProduce
				.Select((pageToCheck, j) =>
				{
					if (i == j)
					{
						return true;
					}

					var relevantRule = relevantRules
						.SingleOrDefault(pageOrderingRule =>
							pageOrderingRule.All(rule => rule == basePage || rule == pageToCheck))
						?
						.ToArray();

					if (relevantRule == null)
					{
						return true;
					}

					var left = relevantRule.First();
					var right = relevantRule.Last();

					return (i < j && basePage == left && pageToCheck == right)
						|| (j < i && basePage == right && pageToCheck == left);
				})
				.All(isAccordingToRules => isAccordingToRules))
			.All(areAccordingToRules => areAccordingToRules);

		return allAccordingToRules;
	}

	private static IEnumerable<IEnumerable<int>> GetRelevantRules(
		IEnumerable<IEnumerable<int>> pageOrderingRules,
		int[] pagesToProduce)
	{
		return pageOrderingRules
			.Where(pageOrderingRule => pageOrderingRule
				.All(rule => pagesToProduce.Any(pageToProduce => pageToProduce == rule)));
	}
}
