using System;

namespace Monofoxe.Pipeline.SpriteGroup.AST
{
  class UnaryOperationNode : Node
	{
		public UnaryOperationNode(Node num, Func<int, int> op)
		{
			_num = num;
			_op = op;
		}
		Node _num;
		Func<int, int> _op;

		public override int Eval()
		{
			int num = _num.Eval();

			return _op(num);
		}
	}
}
