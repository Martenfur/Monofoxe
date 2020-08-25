using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
	public class AngleTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Constructor_CreatingWithBigDegreeValue_WrapsAround()
		{
			var degrees = It.IsAny<double>();

			var a = new Angle(degrees + 360);
			
			Assert.AreEqual(a.Degrees, degrees);
		}

		[Test]
		public void Constructor_CreatingWithNegativeDegreeValue_WrapsAround()
		{
			var degrees = It.IsAny<double>();

			var a = new Angle(degrees - 360);

			Assert.AreEqual(degrees, a.Degrees);
		}

		[Test]
		public void Constructor_CreatingWithVector2_ConvertsToCorrectDegreeValue()
		{
			var vector = new Vector2(0, -1);
			var expected = 270;

			var a = new Angle(vector);

			Assert.AreEqual(expected, a.Degrees);
		}

		[Test]
		public void Constructor_CreatingWithTwoPoints_ConvertsToCorrectDegreeValue()
		{
			var pt1 = new Vector2(0, 32);
			var pt2 = new Vector2(0, 31);
			var expected = 270;

			var a = new Angle(pt1, pt2);

			Assert.AreEqual(expected, a.Degrees);
		}

		[Test]
		public void Degrees_AccessingRadians_ConvertsToRadians()
		{
			var angle = 90;
			var expected = Math.PI / 2.0;

			var a = new Angle(angle);

			Assert.AreEqual(expected, a.Radians);
		}

		[Test]
		public void Radians_AccessingDegrees_ConvertsToDegrees()
		{
			var radians = Math.PI / 2.0;
			var expected = 90;

			var a = Angle.FromRadians(radians);

			Assert.AreEqual(expected, a.Degrees);
		}


		[Test]
		public void Difference_Counterclockwise_FindsShortestAngle()
		{
			var a1 = new Angle(0);
			var a2 = new Angle(90);

			var result = a1.Difference(a2);

			Assert.AreEqual(-90, result);
		}

		[Test]
		public void Difference_Clockwise_FindsShortestAngle()
		{
			var a1 = new Angle(0);
			var a2 = new Angle(270);

			var result = a1.Difference(a2);

			Assert.AreEqual(90, result);
		}


		[Test]
		public void Addition_AddingTwoAngles_WrapsAround()
		{
			var a1 = new Angle(180);
			var a2 = new Angle(270);

			var result = a1 + a2;
			
			Assert.Less(result.Degrees, 360);
		}

		[Test]
		public void Addition_AddingTwoAngles_ReturnsCorrectResult()
		{
			var degrees1 = It.IsAny<double>();
			var degrees2 = It.IsAny<double>();
			var a1 = new Angle(degrees1);
			var a2 = new Angle(degrees2);

			var result = a1 + a2;

			Assert.AreEqual(degrees1 + degrees2, result.Degrees);
		}


		[Test]
		public void Subtraction_SubtractingTwoAngles_WrapsAround()
		{
			var a1 = new Angle(180);
			var a2 = new Angle(270);

			var result = a1 - a2;

			Assert.Greater(result.Degrees, 0);
		}


		[Test]
		public void Subtraction_SubtractingTwoAngles_ReturnsCorrectResult()
		{
			var degrees1 = It.IsAny<double>();
			var degrees2 = It.IsAny<double>();
			var a1 = new Angle(degrees1);
			var a2 = new Angle(degrees2);

			var result = a1 - a2;

			Assert.AreEqual(degrees1 - degrees2, result.Degrees);
		}

		[Test]
		public void Lerp_InterpolateAngles_ReturnsCorrectResult()
		{
			var degrees1 = It.IsAny<double>();
			var degrees2 = It.IsAny<double>();
			var a1 = new Angle(degrees1);
			var a2 = new Angle(degrees2);
			var value = It.IsInRange<double>(0, 1, Moq.Range.Inclusive);

			var result = Angle.Lerp(a1, a2, value);

			var lerp = degrees1 + (degrees2 - degrees1) * value;
			Assert.AreEqual(lerp, result.Degrees);
		}

		[Test]
		public void Lerp_InterpolateNegativeAngles_ReturnsCorrectResult()
		{
			var degrees1 = 0;
			var degrees2 = 90;
			var a1 = new Angle(degrees1);
			var a2 = new Angle(degrees2);
			var value = 0.5;

			var result = Angle.Lerp(a1, a2, value);

			var lerp = new Angle(GameMath.Lerp(degrees1, degrees2, value));
			Assert.AreEqual(lerp.Degrees, result.Degrees);
		}

		[Test]
		public void Lerp_InterpolateNegativeAngles_ReturnsCorrectResult1()
		{
			var degrees1 = 0.0;
			var degrees2 = -90.0;
			var a1 = new Angle(degrees1);
			var a2 = new Angle(degrees2);
			var value = 0.5;

			var result = Angle.Lerp(a1, a2, value);

			var lerp = new Angle(GameMath.Lerp(degrees1, degrees2, value));
			Assert.AreEqual(lerp.Degrees, result.Degrees);
		}

	}
}
