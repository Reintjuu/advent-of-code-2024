using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var pairs = (await File.ReadAllLinesAsync("../../../input"))
	.Select(values => values.Split("   ").Select(int.Parse));

var pairArray = pairs as IEnumerable<int>[] ?? pairs.ToArray();

var first = pairArray.Select(pair => pair.First()).Order().ToArray();
var last = pairArray.Select(pair => pair.Last()).Order().ToArray();

var sumOfSortedDifference = first
	.Zip(last, (firstNumber, secondNumber) => Math.Abs(firstNumber - secondNumber))
	.Sum();

Console.WriteLine(sumOfSortedDifference);

var sumOfOccurenceMultiplication = first
	.Select(value => value * last.Count(lastValue => lastValue == value))
	.Sum();

Console.WriteLine(sumOfOccurenceMultiplication);
