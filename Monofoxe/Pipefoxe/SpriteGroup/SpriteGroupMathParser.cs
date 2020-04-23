using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Pipefoxe.SpriteGroup.SpriteGroupMathTokenizer;

namespace Pipefoxe.SpriteGroup
{
	/// <summary>
	/// This class parse the expresion and returns the number that corresponds to it
	/// Might be slow since it uses recursion to parse the expresion
	/// Not benchmarked yet
	/// </summary>
	//  Based on the code by Brad Robinson in this Medium Article https://medium.com/@toptensoftware/writing-a-simple-math-expression-engine-in-c-d414de18d4ce
	public class SpriteGroupMathParser
	{

		public SpriteGroupMathParser(RawSprite sprite)
		{
			_rawSprite = sprite; // Maybe this should not be here but hey... Fox are sneaky 
		}

		SpriteGroupMathTokenizer _tokenizer;
		RawSprite _rawSprite;
		bool _yAxis = false; // Yes, constant center made me do this, why, because I am awesome

		public int Parse(string text, bool yAxis = false)
		{
			_tokenizer = new SpriteGroupMathTokenizer(text);
			_yAxis = yAxis;
			var expr = ParseAddSubstract();

			if (_tokenizer.CurrentToken != Token.EOF)
			{
				throw new Exception("Unexpected characters at end of expression");
			}
			return expr.Eval();
		}
		/// <summary>
		/// Handles Unary expresions, this let us negate expresions (even be redundant like 10 + +1)
		/// It reuses the <seealso cref="Token.Sub"/> and <seealso cref="Token.Sum"/> to handle it
		/// </summary>
		/// <returns>The Unary expresion reduced</returns>
		Node ParseUnary()
		{
			switch (_tokenizer.CurrentToken)
			{
				case Token.Sum:
					_tokenizer.NextToken();
					return ParseUnary();
				case Token.Sub:
					_tokenizer.NextToken();

					Node rhs = ParseUnary(); // recursive methods are fun, this let us be as redundant as we want

					return new UnaryOperationNode(rhs, a => -a);
			}
			return ParseLeaf();
		}
		/// <summary>
		/// Handles sums and substraction, it follows PEMDAS by asking if their left hand side and right hand side are multiplication
		/// </summary>
		/// <returns></returns>
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
				if (op == null)
				{
					return lhs;
				}
				_tokenizer.NextToken();

				Node rhs = ParseMultiplyDivide();
				lhs = new BinaryOperationNode(lhs, rhs, op);
			}

		}
		/// <summary>
		/// Resolves Multiplication and division in PEMDAS order, let <see cref="ParseUnary"/> resolve negatives and such expresions
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// This recognize our terminal elements, like number and constants, also reduce parenthesis expresions
		/// </summary>
		/// <returns>Terminal element</returns>
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

					if (_tokenizer.CurrentToken != Token.CloseParenthesis)
					{
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
					throw new Exception($"Unexpect token: {_tokenizer.CurrentToken}"); // TODO: Make a SyntaxException or something similar
			}
		}

	}
	#region NodeClasses

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
	#endregion
}

