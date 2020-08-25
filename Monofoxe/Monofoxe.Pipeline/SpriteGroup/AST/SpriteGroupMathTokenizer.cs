using System.IO;
using System.Text;

namespace Monofoxe.Pipeline.SpriteGroup.AST
{
  /// <summary>
  /// This class just goes char by char parsing the text
  /// works alongside <seealso cref="SpriteGroupMathParser"/> to parse an expresion
  /// </summary>
  // based on https://medium.com/@toptensoftware/writing-a-simple-math-expression-engine-in-c-d414de18d4ce
  public class SpriteGroupMathTokenizer
	{
		public enum Token
		{
			Sum = '+',
			Sub = '-',
			Mul = '*',
			Div = '/',
			OpenParenthesis = '(',
			CloseParenthesis = ')',
			Number = 0,
			ConstantTop = 1,
			ConstantBottom = 2,
			ConstantLeft = 3,
			ConstantRight = 4,
			ConstantCenter = 5,
			EOF = -1
		}

		public Token CurrentToken { get; private set; }

		public int Number { get; private set; }

		TextReader _textReader;
		char _currentChar;
		StringBuilder _builder;
		string _text;
		public SpriteGroupMathTokenizer(string text)
		{
			_builder = new StringBuilder(text);
			_builder.Replace("center", "c")
					.Replace("top", "t")
					.Replace("bottom", "b")
					.Replace("left", "l")
					.Replace("right", "r");

			_text = _builder.ToString();
			_textReader = new StringReader(_text);
			_builder.Clear();
			NextChar();
			NextToken();
		}
		/// <summary>
		/// This methods gets the next token, this also gets numeric values if it results being a number
		/// </summary>
		public void NextToken()
		{
			while (char.IsWhiteSpace(_currentChar))
			{
				NextChar();
			}
			switch (_currentChar)
			{
				case '\0':
					CurrentToken = Token.EOF;
					return;

				case '+':
					NextChar();
					CurrentToken = Token.Sum;
					return;

				case '-':
					NextChar();
					CurrentToken = Token.Sub;
					return;
				case '*':
					NextChar();
					CurrentToken = Token.Mul;
					return;

				case '/':
					NextChar();
					CurrentToken = Token.Div;
					return;
				case 't':
					NextChar();
					CurrentToken = Token.ConstantTop;
					return;

				case 'b':
					NextChar();
					CurrentToken = Token.ConstantBottom;
					return;
				case 'r':
					NextChar();
					CurrentToken = Token.ConstantRight;
					return;

				case 'l':
					NextChar();
					CurrentToken = Token.ConstantLeft;
					return;
				case 'c':
					NextChar();
					CurrentToken = Token.ConstantCenter;
					return;
				case '(':
					NextChar();
					CurrentToken = Token.OpenParenthesis;
					return;

				case ')':
					NextChar();
					CurrentToken = Token.CloseParenthesis;
					return;
			}
			if (char.IsDigit(_currentChar))
			{
				while (char.IsDigit(_currentChar))
				{
					_builder.Append(_currentChar);
					NextChar();
				}
				Number = int.Parse(_builder.ToString());
				CurrentToken = Token.Number;
				_builder.Clear();
				return;
			}
		}
		void NextChar()
		{
			int ch = _textReader.Read();
			_currentChar = ch < 0 ? '\0' : (char)ch;
		}

	}

}
