using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Pipefoxe.SpriteGroup.SpriteGroupMathTokenizer;

namespace Pipefoxe.SpriteGroup
{
	public class SpriteGroupMathParser
	{

		public SpriteGroupMathParser(RawSprite sprite)
		{
			_rawSprite = sprite;
		}

		SpriteGroupMathTokenizer _tokenizer;
		RawSprite _rawSprite;
		bool _yAxis = false;
		public int Parse(string text, bool yAxis = false)
		{
			_tokenizer = new SpriteGroupMathTokenizer(text);
			_yAxis = yAxis;
			var expr = ParseAddSubstract();

			if(_tokenizer.CurrentToken != Token.EOF)
			{
				throw new Exception("Unexpected characters at end of expression");
			}
			return expr.Eval();
		}
		Node ParseUnary()
		{
			switch (_tokenizer.CurrentToken)
			{
				case Token.Sum:
					_tokenizer.NextToken();
					return ParseUnary();
				case Token.Sub:
					_tokenizer.NextToken();

					Node rhs = ParseUnary();

					return new UnaryOperationNode(rhs, a => -a);
			}
			return ParseLeaf();
		}
		Node ParseAddSubstract()
		{
			Node lhs = ParseMultiplyDivide();

			while (true)
			{
				Func<int, int, int> op = null;
				switch (_tokenizer.CurrentToken)
				{
					case Token.Sum:
						op = (a, b) => a + b;
						break;
					case Token.Sub:
						op = (a, b) => a - b;
						break;
				}
				if(op == null)
				{
					return lhs;
				}
				_tokenizer.NextToken();

				Node rhs = ParseMultiplyDivide();
				lhs = new BinaryOperationNode(lhs, rhs, op);
			}
		
		}
		Node ParseMultiplyDivide()
		{
			Node lhs = ParseUnary();

			while (true)
			{
				Func<int, int, int> op = null;
				switch (_tokenizer.CurrentToken)
				{
					case Token.Mul:
						op = (a, b) => a * b;
						break;
					case Token.Div:
						op = (a, b) => a / b;
						break;
				}
				if (op == null)
				{
					return lhs;
				}
				_tokenizer.NextToken();

				Node rhs = ParseUnary();
				lhs = new BinaryOperationNode(lhs, rhs, op);
			}
		}
		Node ParseLeaf()
		{
			switch (_tokenizer.CurrentToken)
			{
				case Token.Number:
					_tokenizer.NextToken();
					return new NumberNode(_tokenizer.Number);
				case Token.OpenParenthesis:
					_tokenizer.NextToken();
					Node node = ParseAddSubstract();

					if (_tokenizer.CurrentToken != Token.CloseParenthesis) { 
					throw new Exception("Missing Close Parenthesis");
			     }
					_tokenizer.NextToken();

					return node;
				case Token.ConstantBottom:
					_tokenizer.NextToken();
					return new NumberNode(_rawSprite.RawTexture.Height / _rawSprite.FramesV);
				case Token.ConstantTop:
					_tokenizer.NextToken();
					return new NumberNode(0);
				case Token.ConstantRight:
					_tokenizer.NextToken();
					return new NumberNode(_rawSprite.RawTexture.Width / _rawSprite.FramesH);
				case Token.ConstantLeft:
					_tokenizer.NextToken();
					return new NumberNode(0);
				case Token.ConstantCenter:
					_tokenizer.NextToken();
					if (_yAxis)
					{
						return new NumberNode(_rawSprite.RawTexture.Height / _rawSprite.FramesV / 2);
					}
					return new NumberNode(_rawSprite.RawTexture.Width / _rawSprite.FramesH / 2);
				default:
					throw new Exception($"Unexpect token: {_tokenizer.CurrentToken}");
			}
		}
		
	}
	abstract class Node
	{
		public abstract int Eval();
	}
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

