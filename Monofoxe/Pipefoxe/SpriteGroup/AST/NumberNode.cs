namespace Pipefoxe.SpriteGroup.AST
{
	class NumberNode : Node
	{
		public NumberNode(int number)
		{
			_number = number;
		}

		int _number;

		public override int Eval()
		{
			return _number;
		}
	}
}

