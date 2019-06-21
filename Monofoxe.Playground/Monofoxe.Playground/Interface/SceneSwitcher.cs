using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using System.Collections.Generic;
using Monofoxe.Playground.GraphicsDemo;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
using System;

namespace Monofoxe.Playground.Interface
{
	public class SceneSwitcher : Entity
	{
		public List<SceneFactory> Factories = new List<SceneFactory>
		{
			new SpriteDemoFactory(),
			new ShapeDemoFactory(),
		};

		public int CurrentSceneID {get; private set;} = 0;
		public Scene CurrentScene => CurrentFactory.Scene;
		public SceneFactory CurrentFactory => Factories[CurrentSceneID];


		int _barHeight = 64;
		Color _barColor = Color.Black * 0.5f;
		Color _textColor = Color.White;

		Vector2 _indent = new Vector2(8, 4);

		const Buttons _nextSceneButton = Buttons.E;
		const Buttons _prevSceneButton = Buttons.Q;
		const Buttons _restartButton = Buttons.R;
		const Buttons _toggleUIButton = Buttons.T;

		CameraController _cameraController;

		public SceneSwitcher(Layer layer, CameraController cameraController) : base(layer)
		{
			_cameraController = cameraController;
		}

		public override void Update()
		{
			if (Input.CheckButtonPress(_toggleUIButton))
			{
				Visible = !Visible;
			}

			if (Input.CheckButtonPress(_restartButton))
			{
				RestartScene();
			}

			if (Input.CheckButtonPress(_nextSceneButton))
			{
				NextScene();
			}

			if (Input.CheckButtonPress(_prevSceneButton))
			{
				PreviousScene();
			}



		}

		public override void Draw()
		{
		
			var canvasSize = GameMgr.WindowManager.CanvasSize;

			// Bottom bar.
			GraphicsMgr.AddTransformMatrix(Matrix.CreateTranslation(new Vector3(0, canvasSize.Y - _barHeight, 0)));

			GraphicsMgr.CurrentColor = _barColor;
			RectangleShape.Draw(Vector2.Zero, canvasSize, false);

			GraphicsMgr.CurrentColor = _textColor;
			Text.CurrentFont = Resources.Fonts.Arial;
			Text.HorAlign = TextAlign.Left;
			Text.VerAlign = TextAlign.Top;
			Text.Draw(
				"fps: " + GameMgr.Fps
				+ " | Current scene: " + CurrentScene.Name
				+ Environment.NewLine
				+ _prevSceneButton + "/" + _nextSceneButton + " - change scene, "
				+ _restartButton + " - restart current scene, " 
				+ _toggleUIButton + " - toggle UI"
				+ Environment.NewLine
				+ CameraController.UpButton + "/"
				+ CameraController.DownButton + "/"
				+ CameraController.LeftButton + "/"
				+ CameraController.RightButton + " - move camera, "
				+ CameraController.ZoomInButton + "/" + CameraController.ZoomOutButton + " - zoom, "
				+ CameraController.RotateLeftButton + "/" + CameraController.RotateRightButton + " - rotate"
				,
				_indent
			);

			GraphicsMgr.ResetTransformMatrix();
			// Bottom bar.
		}


		public void NextScene()
		{
			CurrentFactory.DestroyScene();

			CurrentSceneID += 1;
			if (CurrentSceneID >= Factories.Count)
			{
				CurrentSceneID = 0;
			}

			CurrentFactory.CreateScene();

			_cameraController.Reset();
		}


		public void PreviousScene()
		{
			CurrentFactory.DestroyScene();

			CurrentSceneID -= 1;
			if (CurrentSceneID < 0)
			{
				CurrentSceneID = Factories.Count - 1;
			}

			CurrentFactory.CreateScene();

			_cameraController.Reset();
		}


		public void RestartScene()
		{
			CurrentFactory.RestartScene();
			_cameraController.Reset();
		}

	}
}
