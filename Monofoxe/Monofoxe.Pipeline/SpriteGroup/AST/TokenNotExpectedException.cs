using System;

namespace Monofoxe.Pipeline.SpriteGroup.AST
{
	[Serializable]
	internal class TokenNotExpectedException : Exception
	{
		public TokenNotExpectedException()
		{
		}

		public TokenNotExpectedException(string message) : base(message)
		{
		}

		public TokenNotExpectedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
