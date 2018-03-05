using System;
using System.Collections.Generic;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Drawing
{
	public static class Sprites
	{
		#region sprites
		public static Sprite Barrel;
		public static Sprite BarrelParts;
		public static Sprite BroWoolhat;
		public static Sprite BstGam;
		public static Sprite BubbleStrip2;
		public static Sprite Bucket;
		public static Sprite Cabbage1;
		public static Sprite Cabbage1Smashed;
		public static Sprite Cabbage2;
		public static Sprite Cabbage2Smashed;
		public static Sprite Cabbage3;
		public static Sprite Cabbage3Smashed;
		public static Sprite Chiggin;
		public static Sprite Cleric;
		public static Sprite ClericBench;
		public static Sprite ClericHat;
		public static Sprite Ded1;
		public static Sprite Ded1Dmg;
		public static Sprite Demon;
		public static Sprite DemonDmg;
		public static Sprite DemonFire;
		public static Sprite Basket;
		public static Sprite Bench;
		public static Sprite BenchShadow;
		public static Sprite BirdieBody;
		public static Sprite BirdieLegs;
		public static Sprite BirdieWing;
		public static Sprite Boss;
		public static Sprite Bottle;
		public static Sprite Boulder1;
		public static Sprite Boulder2;
		public static Sprite Boulder3;
		public static Sprite Bro;
		public static Sprite BroDmg;
		public static Sprite BroGlasses;
		public static Sprite BroHair;
		public static Sprite BroHat;
		#endregion sprites
		
		public static void Init(Dictionary<string, Frame[]> frames)
		{
			#region sprite_constructors
			
			Barrel = new Sprite(frames["barrel.png"], 0, 0);
			BarrelParts = new Sprite(frames["barrel_parts.png"], 0, 0);
			BroWoolhat = new Sprite(frames["bro_woolhat.png"], 0, 0);
			BstGam = new Sprite(frames["bst_gam.png"], 0, 0);
			BubbleStrip2 = new Sprite(frames["bubble_strip2.png"], 0, 0);
			Bucket = new Sprite(frames["bucket.png"], 0, 0);
			Cabbage1 = new Sprite(frames["cabbage1.png"], 0, 0);
			Cabbage1Smashed = new Sprite(frames["cabbage1_smashed.png"], 0, 0);
			Cabbage2 = new Sprite(frames["cabbage2.png"], 0, 0);
			Cabbage2Smashed = new Sprite(frames["cabbage2_smashed.png"], 0, 0);
			Cabbage3 = new Sprite(frames["cabbage3.png"], 0, 0);
			Cabbage3Smashed = new Sprite(frames["cabbage3_smashed.png"], 0, 0);
			Chiggin = new Sprite(frames["chiggin.png"], 0, 0);
			Cleric = new Sprite(frames["cleric.png"], 0, 0);
			ClericBench = new Sprite(frames["cleric_bench.png"], 0, 0);
			ClericHat = new Sprite(frames["cleric_hat.png"], 0, 0);
			Ded1 = new Sprite(frames["ded1.png"], 0, 0);
			Ded1Dmg = new Sprite(frames["ded1_dmg.png"], 0, 0);
			Demon = new Sprite(frames["demon.png"], 0, 0);
			DemonDmg = new Sprite(frames["demon_dmg.png"], 0, 0);
			DemonFire = new Sprite(frames["demon_fire.png"], 32, 32);
			Basket = new Sprite(frames["Stuff/basket.png"], 0, 0);
			Bench = new Sprite(frames["Stuff/bench.png"], 0, 0);
			BenchShadow = new Sprite(frames["Stuff/bench_shadow.png"], 0, 0);
			BirdieBody = new Sprite(frames["Stuff/birdie_body.png"], 0, 0);
			BirdieLegs = new Sprite(frames["Stuff/birdie_legs.png"], 0, 0);
			BirdieWing = new Sprite(frames["Stuff/birdie_wing.png"], 0, 0);
			Boss = new Sprite(frames["Stuff/boss.png"], 0, 0);
			Bottle = new Sprite(frames["Stuff/bottle.png"], 0, 0);
			Boulder1 = new Sprite(frames["Stuff/boulder1.png"], 0, 0);
			Boulder2 = new Sprite(frames["Stuff/boulder2.png"], 0, 0);
			Boulder3 = new Sprite(frames["Stuff/boulder3.png"], 0, 0);
			Bro = new Sprite(frames["Stuff/bro.png"], 0, 0);
			BroDmg = new Sprite(frames["Stuff/bro_dmg.png"], 0, 0);
			BroGlasses = new Sprite(frames["Stuff/bro_glasses.png"], 0, 0);
			BroHair = new Sprite(frames["Stuff/bro_hair.png"], 0, 0);
			BroHat = new Sprite(frames["Stuff/bro_hat.png"], 0, 0);
			
			#endregion sprite_constructors
		}
	}
}
