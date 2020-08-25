using Microsoft.Xna.Framework;
using Monofoxe.Pipeline.SpriteGroup.AST;
using System;
using static Monofoxe.Pipeline.SpriteGroup.AST.SpriteGroupMathTokenizer;

namespace Monofoxe.Pipeline.SpriteGroup
{
	/// <summary>
	/// This class parses the expresion and returns the number that corresponds to it
	/// Might be slow since it uses recursion to parse the expresion
	/// Not benchmarked yet
	/// </summary>
	//  Based on the code by Brad Robinson in this Medium Article https://medium.com/@toptensoftware/writing-a-simple-math-expression-engine-in-c-d414de18d4ce
	public static class SpriteGroupMathParser
	{

		private static SpriteGroupMathTokenizer _tokenizer;
		private static int ConstantLeft;
		private static int ConstantRight;
		private static int ConstantTop;
		private static int ConstantBottom;
		private static int ConstantCenter;

		/// <summary>
		/// Parses the offset and resolves the math expressions
		/// </summary>
		/// <param name="textX">first string to parse </param>
		/// <param name="textY">second string to parse</param>
		/// <param name="constantLeft">value of the "Left" keyword constant</param>
		/// <param name="constantRight">value of the "Right" keyword constant</param>
		/// <param name="constantTop">value of the "Top" keyword constant</param>
		/// <param name="constantBottom">value of the "Bottom" keyword constant</param>
		/// <param name="constantCenter">value of the component"Center" keyword constant</param>
		/// <returns>A point with the parsed X and Y positions</returns>
		public static Point Parse(
			string textX,
			string textY,
			int constantLeft,
			int constantRight,
			int constantTop,
			int constantBottom,
			Point constantCenter
		)
		{
			Point res = new Point();
			ConstantBottom = constantBottom;
			ConstantLeft = constantLeft;
			ConstantRight = constantRight;
			ConstantTop = constantTop;
			ConstantCenter = constantCenter.X;

			_tokenizer = new SpriteGroupMathTokenizer(textX);
			var expr1 = ParseAddSubstract();
			res.X = expr1.Eval();
			if (_tokenizer.CurrentToken != Token.EOF)
			{
				throw new TokenNotExpectedException("Unexpected characters at end of expression");
			}

			ConstantCenter = constantCenter.Y;
			_tokenizer = new SpriteGroupMathTokenizer(textY);
			var expr2 = ParseAddSubstract();
			res.Y = expr2.Eval();
			if (_tokenizer.CurrentToken != Token.EOF)
			{
				throw new TokenNotExpectedException("Unexpected characters at end of expression");
			}
			return res;
		}

		/// <summary>
		/// Handles Unary expresions, this let us negate expresions (even be redundant like 10 + +1)
		/// It reuses the <seealso cref="Token.Sub"/> and <seealso cref="Token.Sum"/> to handle it
		/// </summary>
		/// <returns>The Unary expresion reduced</returns>
		private static Node ParseUnary()
		{
			switch (_tokenizer.CurrentToken)
			{
				case Token.Sum:
				{
					_tokenizer.NextToken();
					return ParseUnary();
				}
				case Token.Sub:
				{
					_tokenizer.NextToken();
					
					Node rhs = ParseUnary(); // recursive methods are fun, this let us be as redundant as we want

					return new UnaryOperationNode(rhs, a => -a);
				}
			}
			return ParseLeaf();
		}

		/// <summary>
		/// Handles sums and substraction, it follows PEMDAS by asking if their left hand side and right hand side are multiplication
		/// </summary>
		/// <returns></returns>
		private static Node ParseAddSubstract()
		{
			Node lhs = ParseMultiplyDivide();

			while (true)
			{
				Func<int, int, int> op = null;
				switch (_tokenizer.CurrentToken)
				{
					case Token.Sum:
					{
						op = (a, b) => a + b;
						break;
					}
					case Token.Sub:
					{
						op = (a, b) => a - b;
						break;
					}
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
		private static Node ParseMultiplyDivide()
		{
			Node lhs = ParseUnary();

			while (true)
			{
				Func<int, int, int> op = null;
				switch (_tokenizer.CurrentToken)
				{
					case Token.Mul:
					{
						op = (a, b) => a * b;
						break;
					}
					case Token.Div:
					{
						op = (a, b) => a / b;
						break;
					}
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
		private static Node ParseLeaf()
		{
			switch (_tokenizer.CurrentToken)
			{
				case Token.Number:
				{
					_tokenizer.NextToken();
					return new NumberNode(_tokenizer.Number);
				}
				case Token.OpenParenthesis:
				{
					_tokenizer.NextToken();
					Node node = ParseAddSubstract();

					if (_tokenizer.CurrentToken != Token.CloseParenthesis)
					{
						throw new Exception("Missing Close Parenthesis");
					}
					_tokenizer.NextToken();

					return node;
				}
				case Token.ConstantBottom:
				{
					_tokenizer.NextToken();
					return new NumberNode(ConstantBottom);
				}
				case Token.ConstantTop:
				{
					_tokenizer.NextToken();
					return new NumberNode(ConstantTop);
				}
				case Token.ConstantRight:
				{
					_tokenizer.NextToken();
					return new NumberNode(ConstantRight);
				}
				case Token.ConstantLeft:
				{
					_tokenizer.NextToken();
					return new NumberNode(ConstantLeft);
				}
				case Token.ConstantCenter:
				{
					_tokenizer.NextToken();
					return new NumberNode(ConstantCenter);
				}
				default:
				{
					throw new TokenNotExpectedException($"Unexpected token: {_tokenizer.CurrentToken}");
				}
			}
		}

	}

}

