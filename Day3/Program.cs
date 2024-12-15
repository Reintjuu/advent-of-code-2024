using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal partial class Program
{
	const string firstNumber = nameof(firstNumber);
	const string secondNumber = nameof(secondNumber);
	const string multiply = nameof(multiply);
	const string idle = nameof(idle);

	[GeneratedRegex(
		$@"mul\((?<{firstNumber}>\d{{1,3}}),(?<{secondNumber}>\d{{1,3}})\)|(?<{multiply}>do\(\))|(?<{idle}>don't\(\))")]
	private static partial Regex CorruptedMemoryScanner();

	public static async Task Main()
	{
		var input = await File.ReadAllLinesAsync("../../../input");
		var multiplying = true;
		Console.WriteLine(input
			.SelectMany(line => CorruptedMemoryScanner().Matches(line))
			.Where(match =>
			{
				if (match.Groups[multiply].Success)
				{
					multiplying = true;
					return false;
				}

				if (match.Groups[idle].Success)
				{
					multiplying = false;
					return false;
				}

				return multiplying;
			})
			.Select(match => int.Parse(match.Groups[firstNumber].Value) * int.Parse(match.Groups[secondNumber].Value))
			.Sum());
	}
}
