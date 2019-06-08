using System;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;


namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// I'm a sneaky foxe.
	/// 
	/// Sneaked into your game.
	/// 
	/// Stole your entities.
	/// 
	/// Slept on your bad architechture.
	/// </summary>
	public sealed class Foxe : Entity
	{
		private static bool _awaken = false;

		private Alarm _stealAlarm;

		private double _minStealTime = 5;
		private double _maxStealTime = 20;

		private RandomExt _rng;

		private Foxe(Layer layer) : base(layer)
		{
			_stealAlarm = new Alarm();

			_rng = new RandomExt();

			_stealAlarm.Set(_rng.NextDouble(_minStealTime, _maxStealTime));
		}

		public override void Update()
		{
			if (_stealAlarm.Update())
			{
				var chosenScene = SceneMgr.Scenes[_rng.Next(SceneMgr.Scenes.Count)];
				var chosenLayer = chosenScene.Layers[_rng.Next(SceneMgr.Scenes.Count)];
				var chosenEntity = chosenLayer.Entities[_rng.Next(chosenLayer.Entities.Count)];

				if (chosenEntity != this)
				{
					chosenEntity.Enabled = false;
					chosenEntity.Visible = false;
					chosenEntity.Layer = Layer;
				}

				_stealAlarm.Set(_rng.NextDouble(_minStealTime, _maxStealTime));
			}
		}

		public static Entity WakeUp()
		{
			if (_awaken)
			{
				return null;
			}

			Console.WriteLine("You woke up the foxe and she immediately sneaked out of sight.");
			Console.WriteLine("Maybe it wasn't such a good idea?");

			_awaken = true;
			
			if (!SceneMgr.TryGetScene("foxeDen", out Scene foxeDen))
			{
				foxeDen = SceneMgr.CreateScene("foxeDen");
			}

			if (!foxeDen.TryGetLayer("foxeHole", out Layer foxeRoom))
			{
				foxeRoom = foxeDen.CreateLayer("foxeHole");
			}

			new Foxe(foxeRoom);
			
			return new Entity(SceneMgr.GetScene("default")["default"], "You have been bamboozled.");
		}

	}
}
