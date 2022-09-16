using System;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Gradually rotates the angle towards the target.
	/// </summary>
	public class SlowRotator
	{
		public TimeKeeper Time = TimeKeeper.Global;

		public Angle Direction;

		/// <summary>
		/// The speed with which rotator will rotate. Measured in deg/sec.
		/// </summary>
		public double RotationSpeed;

		public SlowRotator(double rotationSpeed)
		{
			RotationSpeed = rotationSpeed;
		}

		public void Update(Angle targetDirection)
		{
			var delta = Direction.Difference(targetDirection);

			var frameTurnSpeed = Time.Time(RotationSpeed);

			if (Math.Abs(delta) > frameTurnSpeed)
			{
				Direction -= frameTurnSpeed * Math.Sign(delta);
			}
			else
			{
				Direction = targetDirection;
			}
		}
	}
}
