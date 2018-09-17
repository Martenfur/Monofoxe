using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine;

namespace Monofoxe.Utils
{
	public class TimeKeeper
	{

		public static readonly TimeKeeper Global = new TimeKeeper();

		/// <summary>
		/// If more than one, game will speed up.
		/// If less than one, game will slow down.
		/// </summary>
		public static double GlobalGameSpeedMultiplier
		{
			get => _globalGameSpeedMultiplier;
			
			set 
			{
				if (value > 0)
				{
					_globalGameSpeedMultiplier = value;
				}
			}
		}
		private static double _globalGameSpeedMultiplier = 1; 
		

		public double GameSpeedMultiplier
		{
			get => _gameSpeedMultiplier;
			
			set 
			{
				if (value > 0)
				{
					_gameSpeedMultiplier = value;
				}
			}
		}
		private double _gameSpeedMultiplier = 1; 
		


		public static double GlobalTime() => 
			GameMgr.ElapsedTime * _globalGameSpeedMultiplier;
		
		public static float GlobalTime(float val) => 
			val * (float)(GameMgr.ElapsedTime * _globalGameSpeedMultiplier);
		
		public static double GlobalTime(double val) => 
			val * GameMgr.ElapsedTime * _globalGameSpeedMultiplier;
	
		
		public double Time() => 
			GameMgr.ElapsedTime * _gameSpeedMultiplier * _globalGameSpeedMultiplier;
	
		public float Time(float val) => 
			val * (float)(GameMgr.ElapsedTime * _gameSpeedMultiplier * _globalGameSpeedMultiplier);

		public double Time(double val) => 
			val * GameMgr.ElapsedTime * _gameSpeedMultiplier * _globalGameSpeedMultiplier;
		
	}
}
