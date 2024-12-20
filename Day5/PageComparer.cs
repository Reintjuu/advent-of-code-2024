using System.Collections.Generic;
using System.Linq;

namespace Day5;

public class PageComparer(IEnumerable<IEnumerable<int>> relevantRules) : IComparer<int>
{
	public int Compare(int x, int y)
	{
		var relevantRule = relevantRules
			.SingleOrDefault(pageOrderingRule =>
				pageOrderingRule.All(rule => rule == x || rule == y))
			?
			.ToArray();

		if (relevantRule == null)
		{
			return 0;
		}

		var left = relevantRule.First();
		var right = relevantRule.Last();

		if (x == left && y == right)
		{
			return -1;
		}

		if (y == left && x == right)
		{
			return 1;
		}

		// Should never happen.
		return 0;
	}
}
