using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using System.Collections.Generic;
using Monofoxe.Playground.GraphicsDemo;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;


namespace Monofoxe.Playground.Interface
{
	public class SceneSwitcher : Entity
	{
		public List<SceneFactory> Factories = new List<SceneFactory>
		{
			new GraphicsDemoFactory(),

		};

		public int CurrentSceneID {get; private set;} = 0;
		public Scene CurrentScene => Factories[CurrentSceneID].Scene;
		public SceneFactory CurrentFactory => Factories[CurrentSceneID];


		int _barHeight = 64;
		Color _barColor = Color.Black * 0.5f;
		Color _textColor = Color.White;

		public SceneSwitcher(Layer layer) : base(layer)
		{
			
		}


		public override void Draw()
		{
			var canvasSize = GameMgr.WindowManager.CanvasSize;

			GraphicsMgr.CurrentColor = _barColor;
			RectangleShape.Draw(new Vector2(0, canvasSize.Y - _barHeight), canvasSize, false);

			GraphicsMgr.CurrentColor = _textColor;
			Text.CurrentFont = Resources.Fonts.Arial;
			Text.HorAlign = TextAlign.Left;
			Text.VerAlign = TextAlign.Top;
			Text.Draw("fps: " + GameMgr.Fps, new Vector2(16, 16));

			//Resources.Sprites.Default.Monofoxe.Draw(new Vector2(400, 300), Resources.Sprites.Default.Monofoxe.Origin);
		}


		public void NextScene()
		{
			Factories[CurrentSceneID].DestroyScene();

			CurrentSceneID += 1;
			if (CurrentSceneID >= Factories.Count)
			{
				CurrentSceneID = 0;
			}

			Factories[CurrentSceneID].CreateScene();
		}


		public void PreviousScene()
		{
			Factories[CurrentSceneID].DestroyScene();

			CurrentSceneID -= 1;
			if (CurrentSceneID < 0)
			{
				CurrentSceneID = Factories.Count - 1;
			}

			Factories[CurrentSceneID].CreateScene();
		}


		public void RestartScene() =>
			Factories[CurrentSceneID].RestartScene();

	}
}
