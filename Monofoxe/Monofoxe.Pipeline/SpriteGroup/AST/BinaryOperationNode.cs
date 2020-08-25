using System;

namespace Monofoxe.Pipeline.SpriteGroup.AST
{
	class BinaryOperationNode : Node
	{

		public BinaryOperationNode(Node lhs, Node rhs, Func<int, int, int> op)
		{
			_lhs = lhs;
			_rhs = rhs;
			_op = op;
		}

		Node _lhs;
		Node _rhs;
		Func<int, int, int> _op;

		public override int Eval()
		{

			var lhsVal = _lhs.Eval();
			var rhsVal = _rhs.Eval();

			var result = _op(lhsVal, rhsVal);
			return result;
		}
	}
}

