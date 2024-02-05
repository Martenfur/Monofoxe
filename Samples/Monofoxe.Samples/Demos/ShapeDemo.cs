using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using System.Diagnostics;

namespace Monofoxe.Samples.Demos
{
	public class ShapeDemo : Entity
	{
		
		Color _mainColor = Color.White;
		HsvColor _secondaryColor = new HsvColor(new Color(34, 65, 250));
		
		public ShapeDemo(Layer layer) : base(layer)
		{

		}

		public override void Draw()
		{
			base.Draw();

			_secondaryColor.H += TimeKeeper.Global.Time(360 / 4f);
			if (_secondaryColor.H >= 360f)
			{ 
				_secondaryColor.H -= 360;
			}
			Debug.WriteLine(_secondaryColor.H);

			// This code shows how to draw shapes using static methods and instanced objects.
			
			var startingPosition = new Vector2(100, 100);
			var position = startingPosition;
			var spacing = 100;

			// Circles.
			GraphicsMgr.CurrentColor = _mainColor; // Setting current color. It's active for all shapes and sprites.
			CircleShape.Draw(position, 24, ShapeFill.Solid); // Filled circle.

			GraphicsMgr.CurrentColor = _secondaryColor.ToColor();
			CircleShape.Draw(position, 32, ShapeFill.Outline); // Outline.


			position += Vector2.UnitX * spacing;


			CircleShape.CircleVerticesCount = 8; // Changing the amount of circle vertices.

			GraphicsMgr.CurrentColor = _mainColor; 
			CircleShape.Draw(position, 24, ShapeFill.Solid); 

			GraphicsMgr.CurrentColor = _secondaryColor.ToColor();
			CircleShape.Draw(position, 32, ShapeFill.Outline);

			CircleShape.CircleVerticesCount = 32;
			// Circles.


			position += Vector2.UnitY * spacing;
			position.X = startingPosition.X;


			// Rectangles.
			
			// You can draw rectangle using its top left and bottom right point...
			RectangleShape.Draw(position - Vector2.One * 24, position + Vector2.One * 24, ShapeFill.Solid);

			GraphicsMgr.CurrentColor = _mainColor;
			// ...or its center position and size!
			RectangleShape.DrawBySize(position, Vector2.One * 64, ShapeFill.Outline);

			position += Vector2.UnitX * spacing;

			RectangleShape.Draw( // We can also manually set colors for each vertex.
				position - Vector2.One * 24, 
				position + Vector2.One * 24,
				ShapeFill.Solid, 
				_mainColor, 
				_mainColor, 
				_mainColor, 
				_secondaryColor.ToColor()
			);

			RectangleShape.DrawBySize(
				position, 
				Vector2.One * 64,
				ShapeFill.Outline,
				_mainColor,
				_secondaryColor.ToColor(),
				_mainColor,
				_mainColor
			);
			// Rectangles.


			position += Vector2.UnitY * spacing;
			position.X = startingPosition.X;


			// Triangles.

			GraphicsMgr.CurrentColor = _mainColor;

			TriangleShape.Draw(
				position + new Vector2(-24, -24), 
				position + new Vector2(24, -24), 
				position + new Vector2(24, 24),
				ShapeFill.Solid
			);
			
			// Be aware of culling. This triangle, for example, will be culled.
			// You can disable culling, if you don't want to deal with it.
			TriangleShape.Draw(new Vector2(-24, -24), new Vector2(24, 24), new Vector2(24, -24), ShapeFill.Solid);

			// Triangles.



			// Lines.

			position += Vector2.UnitX * spacing;
			LineShape.Draw(position - Vector2.One * 24, position + Vector2.One * 24);

			position += Vector2.UnitX * spacing / 2f;
			ThickLineShape.Draw(position - Vector2.One * 24, position + Vector2.One * 24, 5);

			// Lines.


		}

	}
}
