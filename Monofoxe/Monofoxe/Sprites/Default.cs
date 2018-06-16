// Template tags: 
// Default - Name of current group.
// <sprite_name> - Name of each sprite.
// <sprite_hash_name> - Hash name of each sprite.

using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprites
{
	public static class Default
	{
		#region sprites
		public static Sprite Scene5Bkg;
		public static Sprite Scene3Bkg;
		public static Sprite Scene5BkgStrip2;
		public static Sprite Scene4Bkg;
		public static Sprite Scene1TreesBkgLayer1;
		public static Sprite Scene3TreeLeft;
		public static Sprite Scene3TreeRight;
		public static Sprite Scene1TreesBkgLayer3;
		public static Sprite Scene1KnightBody;
		public static Sprite Scene2Knight;
		public static Sprite Scene1Knight;
		public static Sprite ClericBench;
		public static Sprite Boss;
		public static Sprite TestKitten;
		public static Sprite Scene4BucketShadow;
		public static Sprite DemonFire;
		public static Sprite Barrel;
		public static Sprite Scene1KnightFaceStrip3;
		public static Sprite Cleric;
		public static Sprite Ded1;
		public static Sprite Ded1Dmg;
		public static Sprite Demon;
		public static Sprite DemonDmg;
		public static Sprite Bro;
		public static Sprite BroDmg;
		public static Sprite Chiggin;
		public static Sprite Cabbage1Smashed;
		public static Sprite Cabbage2Smashed;
		public static Sprite Boulder1;
		public static Sprite Cabbage3Smashed;
		public static Sprite Boulder2;
		public static Sprite Cabbage2;
		public static Sprite Cabbage0;
		public static Sprite Boulder3;
		public static Sprite Bottle;
		public static Sprite Cabbage3;
		public static Sprite Bucket;
		public static Sprite ClericHat;
		public static Sprite Sc5KnightSword;
		public static Sprite Basket;
		public static Sprite Scene4Knight;
		public static Sprite BroWoolhat;
		public static Sprite Bench;
		public static Sprite BirdieLegs;
		public static Sprite BirdieBody;
		public static Sprite SpriteFont;
		public static Sprite BubbleStrip2;
		public static Sprite BenchShadow;
		public static Sprite BroHat;
		public static Sprite BarrelParts;
		public static Sprite BroGlasses;
		public static Sprite BroHair;
		public static Sprite BirdieWing;
		public static Sprite AnotherFont;
		public static Sprite Scene2Bkg;
		public static Sprite Scene4Bkg_1;
		public static Sprite Scene2;
		public static Sprite Scene4Knight_1;
		public static Sprite Scene3Bkg_1;
		public static Sprite Scene1TreesBkgLayer2;
		public static Sprite Scene3Sword;
		public static Sprite Scene2Reflection;
		public static Sprite Scene4Bucket;
		public static Sprite Scene3Ground;
		public static Sprite Scene3Hill;
		public static Sprite BstGam;
		#endregion sprites
		
		private static string _groupName = "Default";
		private static ContentManager _content = new ContentManager(GameCntrl.Game.Services);
		
		public static bool Loaded = false;
		
		public static void Load()
		{
			var sprites = new Dictionary<string, Frame[]>();
			
			var i = 0;
			var graphicsPath = GameCntrl.ContentDir + '/' + GameCntrl.GraphicsDir +  '/' + _groupName + '_';
			
			Loaded = true;
			
			// Loading all atlasses.
			while(true)
			{
				try
				{
					var atlassSprites = _content.Load<Dictionary<string, Frame[]>>(graphicsPath + i);
					sprites = sprites.Concat(atlassSprites).ToDictionary(x => x.Key, x => x.Value);
				}
				catch(Exception) // If content file doesn't exist, this means we've loaded all atlasses.
				{
					break;
				}
				
				i += 1;
			}
			// Loading all atlasses.
			
			#region sprite_constructors
			
			Scene5Bkg = new Sprite(sprites["outro/scene5_bkg"]);
			Scene3Bkg = new Sprite(sprites["intro/scene3_bkg"]);
			Scene5BkgStrip2 = new Sprite(sprites["outro/scene5_bkg_strip2"]);
			Scene4Bkg = new Sprite(sprites["intro/scene4_bkg"]);
			Scene1TreesBkgLayer1 = new Sprite(sprites["outro/scene1_trees_bkg_layer1"]);
			Scene3TreeLeft = new Sprite(sprites["outro/scene3_tree_left"]);
			Scene3TreeRight = new Sprite(sprites["outro/scene3_tree_right"]);
			Scene1TreesBkgLayer3 = new Sprite(sprites["outro/scene1_trees_bkg_layer3"]);
			Scene1KnightBody = new Sprite(sprites["outro/scene1_knight_body"]);
			Scene2Knight = new Sprite(sprites["outro/scene2_knight"]);
			Scene1Knight = new Sprite(sprites["intro/scene1_knight"]);
			ClericBench = new Sprite(sprites["cleric_bench"]);
			Boss = new Sprite(sprites["Stuff/boss"]);
			TestKitten = new Sprite(sprites["test kitten"]);
			Scene4BucketShadow = new Sprite(sprites["intro/scene4_bucket_shadow"]);
			DemonFire = new Sprite(sprites["demon_fire"]);
			Barrel = new Sprite(sprites["barrel"]);
			Scene1KnightFaceStrip3 = new Sprite(sprites["outro/scene1_knight_face_strip3"]);
			Cleric = new Sprite(sprites["cleric"]);
			Ded1 = new Sprite(sprites["ded1"]);
			Ded1Dmg = new Sprite(sprites["ded1_dmg"]);
			Demon = new Sprite(sprites["demon"]);
			DemonDmg = new Sprite(sprites["demon_dmg"]);
			Bro = new Sprite(sprites["Stuff/bro"]);
			BroDmg = new Sprite(sprites["Stuff/bro_dmg"]);
			Chiggin = new Sprite(sprites["chiggin"]);
			Cabbage1Smashed = new Sprite(sprites["cabbage1_smashed"]);
			Cabbage2Smashed = new Sprite(sprites["cabbage2_smashed"]);
			Boulder1 = new Sprite(sprites["Stuff/boulder1"]);
			Cabbage3Smashed = new Sprite(sprites["cabbage3_smashed"]);
			Boulder2 = new Sprite(sprites["Stuff/boulder2"]);
			Cabbage2 = new Sprite(sprites["cabbage2"]);
			Cabbage0 = new Sprite(sprites["cabbage_0"]);
			Boulder3 = new Sprite(sprites["Stuff/boulder3"]);
			Bottle = new Sprite(sprites["Stuff/bottle"]);
			Cabbage3 = new Sprite(sprites["cabbage3"]);
			Bucket = new Sprite(sprites["bucket"]);
			ClericHat = new Sprite(sprites["cleric_hat"]);
			Sc5KnightSword = new Sprite(sprites["outro/sc5_knight_sword"]);
			Basket = new Sprite(sprites["Stuff/basket"]);
			Scene4Knight = new Sprite(sprites["outro/scene4_knight"]);
			BroWoolhat = new Sprite(sprites["bro_woolhat"]);
			Bench = new Sprite(sprites["Stuff/bench"]);
			BirdieLegs = new Sprite(sprites["Stuff/birdie_legs"]);
			BirdieBody = new Sprite(sprites["Stuff/birdie_body"]);
			SpriteFont = new Sprite(sprites["sprite_font"]);
			BubbleStrip2 = new Sprite(sprites["bubble_strip2"]);
			BenchShadow = new Sprite(sprites["Stuff/bench_shadow"]);
			BroHat = new Sprite(sprites["Stuff/bro_hat"]);
			BarrelParts = new Sprite(sprites["barrel_parts"]);
			BroGlasses = new Sprite(sprites["Stuff/bro_glasses"]);
			BroHair = new Sprite(sprites["Stuff/bro_hair"]);
			BirdieWing = new Sprite(sprites["Stuff/birdie_wing"]);
			AnotherFont = new Sprite(sprites["another_font"]);
			Scene2Bkg = new Sprite(sprites["outro/scene2_bkg"]);
			Scene4Bkg_1 = new Sprite(sprites["outro/scene4_bkg"]);
			Scene2 = new Sprite(sprites["intro/scene2"]);
			Scene4Knight_1 = new Sprite(sprites["intro/scene4_knight"]);
			Scene3Bkg_1 = new Sprite(sprites["outro/scene3_bkg"]);
			Scene1TreesBkgLayer2 = new Sprite(sprites["outro/scene1_trees_bkg_layer2"]);
			Scene3Sword = new Sprite(sprites["intro/scene3_sword"]);
			Scene2Reflection = new Sprite(sprites["outro/scene2_reflection"]);
			Scene4Bucket = new Sprite(sprites["intro/scene4_bucket"]);
			Scene3Ground = new Sprite(sprites["outro/scene3_ground"]);
			Scene3Hill = new Sprite(sprites["outro/scene3_hill"]);
			BstGam = new Sprite(sprites["Textures/bst_gam"]);
			
			#endregion sprite_constructors
		}
		
		public static void Unload()
		{
			_content.Unload();
			Loaded = false;
		}
	}
}
