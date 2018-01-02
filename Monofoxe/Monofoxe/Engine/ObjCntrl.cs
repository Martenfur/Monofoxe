using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;



namespace Monofoxe.Engine
{
	public class ObjCntrl
	{
		
		private static List<GameObj> _gameObjects = new List<GameObj>();

		private static double _fixedUpdateAl = 0;

		public static void Update(GameTime gameTime)
		{
			// Fixed updates.
			_fixedUpdateAl += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateAl >= GameCntrl.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateAl / GameCntrl.FixedUpdateRate); // In case of lags.
				_fixedUpdateAl -= GameCntrl.FixedUpdateRate * overflow;	
				
				foreach(GameObj obj in _gameObjects)
				{obj.FixedUpdateBegin();}

				foreach(GameObj obj in _gameObjects)
				{obj.FixedUpdate();}

				foreach(GameObj obj in _gameObjects)
				{obj.FixedUpdateEnd();}
			}
			// Fixed updates.

			// Normal updates.
			foreach(GameObj obj in _gameObjects)
			{obj.UpdateBegin();}

			foreach(GameObj obj in _gameObjects)
			{obj.Update();}

			foreach(GameObj obj in _gameObjects)
			{obj.UpdateEnd();}
			// Normal updates.

		}
		
		public static void Draw()
		{
			foreach(GameObj obj in _gameObjects)
			{obj.DrawBegin();}

			foreach(GameObj obj in _gameObjects)
			{obj.Draw();}

			foreach(GameObj obj in _gameObjects)
			{obj.DrawEnd();}
		}

		public static void AddObject(GameObj obj)
		{_gameObjects.Add(obj);}

	}
}
