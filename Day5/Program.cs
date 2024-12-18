﻿using System;
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

	public static int DetermineCorrectPageOrdering(string input)
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
			.Select(values => values.Split(',').Select(int.Parse));

		return pagesToProduceInEachUpdate.Sum(pagesToProduce =>
		{
			var relevantRules = pageOrderingRules
				.Where(pageOrderingRule => pageOrderingRule
					.All(rule => pagesToProduce.Any(pageToProduce => pageToProduce == rule)))
				.ToList();

			var enumeratedPagesToProduce = pagesToProduce.ToArray();
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

			return allAccordingToRules ? enumeratedPagesToProduce[enumeratedPagesToProduce.Length / 2] : 0;
		});
	}
}
