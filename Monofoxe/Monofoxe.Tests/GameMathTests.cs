using Monofoxe.Engine.Utils;
using Moq;
using NUnit.Framework;

namespace Tests
{
	public class GameMathTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Lerp_Lerp_ReturnsCorrectValue()
		{
			var a = It.IsAny<double>();
			var b = It.IsAny<double>();
			var value = It.IsInRange<double>(0, 1, Range.Inclusive);
			
			var result = GameMath.Lerp(a, b, value);

			Assert.AreEqual(a + (b - a) * value, result);
		}
		
		// TODO: Add MOAR.
	}
}