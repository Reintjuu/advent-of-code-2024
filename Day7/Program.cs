namespace Day7;

public class Program
{
	static async Task Main(string[] args)
	{
		Console.WriteLine(Calculate(await File.ReadAllLinesAsync("../../../input")));
	}

	static long Add(long a, long b)
	{
		return a + b;
	}

	static long Multiply(long a, long b)
	{
		return a * b;
	}

	public static long Calculate(string[] input)
	{
		Func<long, long, long>[] operations = [Add, Multiply];
		return input
			.Select(values =>
			{
				var resultAndInput = values.Split(": ");
				return (result: long.Parse(resultAndInput[0]), input: resultAndInput[1].Split(' ').Select(long.Parse).ToList());
			})
			.Where((tuple =>
			{
				var (result, values) = tuple;
				return Enumerable
					.Range(0, values.Count - 1)
					.Select(_ => operations)
					.CartesianProduct()
					.Any(c =>
					{
						var currentOperations = c.ToList();
						var index = 0;
						return result
							== values
								.Aggregate((a, b) =>
								{
									var currentResult = currentOperations[index](a, b);
									index++;
									return currentResult;
								});
					});
			}))
			.Sum(tuple => tuple.result);
	}
}

public static class Extensions
{
	public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(
		this IEnumerable<IEnumerable<T>> sequences)
	{
		IEnumerable<IEnumerable<T>> emptyProduct = [[]];
		return sequences.Aggregate(emptyProduct,
			(accumulator, sequence) => accumulator
				.SelectMany(_ => sequence, (accumulatorSequence, item) => accumulatorSequence.Concat([item])));
	}
}
