using System;
using System.Runtime.Serialization;

namespace Pipefoxe.SpriteGroup.AST
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

	protected TokenNotExpectedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
	}
}
