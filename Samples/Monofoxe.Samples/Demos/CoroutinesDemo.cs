using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.Coroutines;
using Monofoxe.Samples.Misc;
using System.Collections;


namespace Monofoxe.Samples.Demos
{
	public class CoroutinesDemo : Entity
	{
		private Vector2 _ballSpawnerPosition = new Vector2(200, 400);
		private Vector2 _waitUntilSpawnerPosition = new Vector2(200, 200);
		private Vector2 _waitWhileSpawnerPosition = new Vector2(400, 200);
		private Vector2 _basicUpdateClockPosition = new Vector2(400, 400);
		private Vector2 _fixedUpdateClockPosition = new Vector2(600, 400);
		private Vector2 _sequencePosition = new Vector2(600, 200);

		private Angle _basicUpdateClock = Angle.Up;
		private Angle _fixedUpdateClock = Angle.Up;

		public CoroutinesDemo(Layer layer) : base(layer)
		{
			StartCoroutine(BasicUpdateClockCoroutine());
			StartCoroutine(FixedUpdateClockCoroutine());
			StartCoroutine(BallSpawnerCoroutine());
			

			StartCoroutine(WaitUntilCoroutine());
			StartCoroutine(WaitWhileCoroutine());


			StartJob(LargeJob(), 0.1f);
		}

		public override void Update()
		{
			base.Update();
			if (Input.CheckButtonPress(Buttons.N))
			{
				if (_sequence != null)
				{
					// Stopping an already running sequence.
					StopCoroutine(_sequence);
				}
				// Starting a new scripted sequence.
				_sequence = StartCoroutine(SequenceCoroutine());
			}
		}


		private IEnumerator ExampleCoroutine()
		{
			// Coroutines are special methods that can be paused or delayed.
			// COROUTINES ARE NOT ASYNC. They execute on the same Update in a component,
			// so it's thread-safe.

			var a = 0;
			
			yield return null; // yield return null means that the methods will be paused for a single frame and resumed on the next one.
			
			a += 1;

			yield return Wait.ForSeconds(1); // Method will be paused for one second and executed when the times runs out.

			// You can also put coroutines within other coroutines. Parent coroutine will be paused until the new one finishes.
			yield return BasicUpdateClockCoroutine(); 
		}

		private IEnumerator BasicUpdateClockCoroutine()
		{
			while (true)
			{
				// Return null means that the function will be resumed next frame.
				yield return null;
				_basicUpdateClock += 1;
			}
		}

		private IEnumerator FixedUpdateClockCoroutine()
		{
			while (true)
			{
				// This function will pause until the next fixed update.
				yield return Wait.ForFixedUpdate();
				_fixedUpdateClock += 1;
			}
		}

		private IEnumerator BallSpawnerCoroutine()
		{
			while (true)
			{
				for (var i = 0; i < 5; i += 1)
				{
					// 5 balls will be spawned with the interval of 0.1 seconds.
					yield return Wait.ForSeconds(0.1);
					new Ball(Layer, _ballSpawnerPosition);
				}
				// After 5 balls will have spawned, there will be a 1 second pause.
				yield return Wait.ForSeconds(1);
			}
		}

		private Coroutine _sequence;
		private int _sequenceStage = 0;
		private Color _sequenceColor = Color.White;
		private IEnumerator SequenceCoroutine()
		{
			// Coroutines can be used for complex sequrences which would
			// require a lot of alarms and complex logic otherwise.
			_sequenceStage = 0;
			_sequenceColor = Color.White;

			_sequenceStage += 1;
			yield return Wait.ForSeconds(0.5);
			_sequenceStage += 1;
			yield return Wait.ForSeconds(0.5);
			_sequenceStage += 1;
			yield return Wait.ForSeconds(3);
			_sequenceStage += 1;

			yield return SubsequenceCoroutine(); // Current coroutine will be paused until this one finishes.

			_sequenceStage = 0;

			yield break; // Yield break abandons the coroutine. The code below will not be executed.

			yield return Wait.ForSeconds(0.5);
			_sequenceStage += 1;
			yield return Wait.ForSeconds(0.5);
			_sequenceStage += 1;
			yield return Wait.ForSeconds(0.5);
			_sequenceStage += 1;
		}

		private IEnumerator SubsequenceCoroutine()
		{
			for (var i = 0; i < 10; i += 1)
			{
				_sequenceColor.G -= 20;
				_sequenceColor.B -= 20;
				_sequenceStage = 0;
				yield return Wait.ForSeconds(0.1);
				_sequenceStage += 1;
				yield return Wait.ForSeconds(0.1);
				_sequenceStage += 1;
				yield return Wait.ForSeconds(0.1);
				_sequenceStage += 1;
				yield return Wait.ForSeconds(0.1);
			}
		}

		private IEnumerator WaitUntilCoroutine()
		{
			while (true)
			{
				yield return Wait.Until(() => Input.CheckButton(Buttons.B));
				new Ball(Layer, _waitUntilSpawnerPosition);
			}
		}

		private IEnumerator WaitWhileCoroutine()
		{
			while (true)
			{
				yield return Wait.While(() => Input.CheckButton(Buttons.B));
				new Ball(Layer, _waitWhileSpawnerPosition);
			}
		}


		private int _jobCounter = 0;
		private IEnumerator LargeJob()
		{
			// Jobs are similar to coroutines, but instead of scheduling execution
			// to the next frame every time, they have a time budget. 
			// If there is enough time left, yield return null will immediately return
			// back the the job in the same frame.
			// Jobs are well-suited for large tasks that should happen in the background, like pathfinding, map loading, etc.
			// NOTE: DO NOT use Wait methods, they are incompatible with jobs!!!
			yield return SubLargeJob();
			yield return SubLargeJob();
			yield return SubLargeJob();
		}

		private IEnumerator SubLargeJob()
		{
			for (var k = 0; k < 10000000; k += 1)
			{
				_jobCounter += 1; 
				yield return null;
			}
		}


		public override void Draw()
		{
			base.Draw();

			if (Input.CheckButtonPress(Buttons.D))
			{
				Enabled = !Enabled;
			}

			DrawClocks();
			DrawSequence();

			GraphicsMgr.CurrentColor = Color.White;
			Text.HorAlign = TextAlign.Center;
			Text.Draw("ball spawner", _ballSpawnerPosition + new Vector2(0.5f, -64));
			Text.Draw("basic update", _basicUpdateClockPosition + new Vector2(0.0f, -64));
			Text.Draw("fixed update", _fixedUpdateClockPosition + new Vector2(0.5f, -64));

			Text.Draw("press B to spawn", _waitUntilSpawnerPosition + new Vector2(0.0f, -64));
			Text.Draw("press B to stop", _waitWhileSpawnerPosition + new Vector2(0.5f, -64));
			Text.Draw("press N to start sequence", _sequencePosition + new Vector2(0.5f, -64));

			Text.HorAlign = TextAlign.Left;
			Text.Draw("Job progress: " + _jobCounter, new Vector2(32, 32));
		}

		private void DrawClocks()
		{
			var clockSize = 32;

			CircleShape.Draw(_basicUpdateClockPosition, 32, ShapeFill.Outline);
			LineShape.Draw(
				_basicUpdateClockPosition,
				_basicUpdateClockPosition + _basicUpdateClock.ToVector2() * clockSize
			);
			CircleShape.Draw(_fixedUpdateClockPosition, 32, ShapeFill.Outline);
			LineShape.Draw(
				_fixedUpdateClockPosition,
				_fixedUpdateClockPosition + _fixedUpdateClock.ToVector2() * clockSize
			);
		}


		private void DrawSequence()
		{
			GraphicsMgr.CurrentColor = _sequenceColor;
			if (_sequenceStage == 1)
			{
				CircleShape.Draw(_sequencePosition, 32, ShapeFill.Solid);
			}
			if (_sequenceStage == 2)
			{
				RectangleShape.DrawBySize(_sequencePosition, Vector2.One * 32, ShapeFill.Solid);
			}
			if (_sequenceStage == 3)
			{
				TriangleShape.Draw(
					_sequencePosition - Vector2.UnitY * 32,
					_sequencePosition + Vector2.UnitX * 32,
					_sequencePosition - Vector2.UnitX * 32,
					ShapeFill.Solid
				);
			}
		}

	}
}
