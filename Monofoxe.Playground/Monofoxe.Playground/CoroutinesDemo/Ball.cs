using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.Coroutines;
using System;
using System.Collections;

namespace Monofoxe.Playground.CoroutinesDemo
{
	public class Ball : Entity
	{
		private Vector2 _position;
		private Vector2 _velocity;
		private Color _color;
		private float _r;

		private static RandomExt _rng = new RandomExt();

		public Ball(Layer layer, Vector2 position) : base(layer)
		{
			_position = position;
			StartCoroutine(DestructionCountdown());
			_velocity = new Vector2(_rng.Next(-100, 100), _rng.Next(-300, -200));
			_color = new Color(_rng.Next(256), _rng.Next(256), _rng.Next(256));
			_r = _rng.Next(4, 10);
		}

		private IEnumerator DestructionCountdown()
		{
			// After waiting for 6 seconds, the entity will be destroyed.
			yield return new WaitForSeconds(6);
			DestroyEntity();
		}

		public override void Update()
		{
			base.Update();

			_velocity.Y += 1000 * (float)TimeKeeper.Global.Time();

			_position += _velocity * (float)TimeKeeper.Global.Time();
		}
		
		public override void Draw()
		{
			base.Draw();
			GraphicsMgr.CurrentColor = _color;
			CircleShape.Draw(_position, _r, false);
		}

	}
}
