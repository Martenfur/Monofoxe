namespace Monofoxe.Test
{
	/// <summary>
	/// Don't forget to describe your classes, fields and methods.
	/// Of course, not all of them will actually need a description.
	/// </summary>
	public class Codestyle : Style // Opening brackets start from a new line.
	{
		// We are using tabs, not spaces.

		private int _privateField;

		protected int _protectedField;
		
		public int PublicField;

		public void Method()
		{
			var localVariable = 0;

			if (true) // Brackets are present even if the body is one line long.
			{
				localVariable = 1;
			}

			LongLongMethod( // Don't let method calls get too long.
				_privateField, 
				_protectedField, 
				PublicField, 
				localVariable, 
				true
			);

		}


		private void LongLongMethod(
			int someVeryLongName, 
			int someOtherSuperLongName, 
			int veryFluffyFoxes, 
			int stuff,
			bool boop
		)
		{
			// Do stuff.
		}

	}
}