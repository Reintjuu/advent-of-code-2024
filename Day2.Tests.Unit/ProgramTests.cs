namespace Day2.Tests.Unit;

public class ProgramTests
{
	[Fact]
	public void ShouldReturn4ForTestSet()
	{
		Assert.Equal(4,
			Program.CalculateSafeLevelAmount([
				[9, 6, 4, 2, 1],
				[1, 2, 7, 8, 9],
				[9, 7, 6, 2, 1],
				[1, 3, 2, 4, 5],
				[8, 6, 4, 4, 1],
				[1, 3, 6, 7, 9]
			]));
	}
}
