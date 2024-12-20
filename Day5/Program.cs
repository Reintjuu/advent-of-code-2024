using Day5;
using System;
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
			.Select(values => values
				.Split('|')
				.Select(int.Parse));
		var pagesToProduceInEachUpdate = rulesAndPagesToProduce
			.Last()
			.Split('\n')
			.Select(values => values
				.Split(',')
				.Select(int.Parse)
				.ToArray())
			.ToArray();

		var allPagesSortedAccordingToRules = pagesToProduceInEachUpdate
			.Select(pagesToSort =>
			{
				var relevantRules = pageOrderingRules
					.Where(pageOrderingRule => pageOrderingRule
						.All(rule => pagesToSort.Any(page => page == rule)));

				return pagesToSort
					.Order(new PageComparer(relevantRules))
					.ToArray();
			})
			.ToArray();

		return (
			correct: allPagesSortedAccordingToRules
				.Where(pagesSortedAccordingToRules => pagesToProduceInEachUpdate.Any(pagesSortedAccordingToRules.SequenceEqual))
				.Sum(result => result[result.Length / 2]),
			incorrect: allPagesSortedAccordingToRules
				.Where(pagesSortedAccordingToRules => !pagesToProduceInEachUpdate.Any(pagesSortedAccordingToRules.SequenceEqual))
				.Sum(result => result[result.Length / 2]));
	}
}
