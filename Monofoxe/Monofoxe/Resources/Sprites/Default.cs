// Template tags: 
// Default - Name of current group.
// <sprite_name> - Name of each sprite.
// <sprite_hash_name> - Hash name of each sprite.

using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;

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
			Loaded = true;
			var graphicsPath = GameCntrl.ContentDir + '/' + GameCntrl.GraphicsDir +  '/' + _groupName;
			var sprites = _content.Load<Dictionary<string, Sprite>>(graphicsPath);
			
			#region sprite_constructors
			
			Scene5Bkg = sprites["outro/scene5_bkg"];
			Scene3Bkg = sprites["intro/scene3_bkg"];
			Scene5BkgStrip2 = sprites["outro/scene5_bkg_strip2"];
			Scene4Bkg = sprites["intro/scene4_bkg"];
			Scene1TreesBkgLayer1 = sprites["outro/scene1_trees_bkg_layer1"];
			Scene3TreeLeft = sprites["outro/scene3_tree_left"];
			Scene3TreeRight = sprites["outro/scene3_tree_right"];
			Scene1TreesBkgLayer3 = sprites["outro/scene1_trees_bkg_layer3"];
			Scene1KnightBody = sprites["outro/scene1_knight_body"];
			Scene2Knight = sprites["outro/scene2_knight"];
			Scene1Knight = sprites["intro/scene1_knight"];
			ClericBench = sprites["cleric_bench"];
			Boss = sprites["Stuff/boss"];
			TestKitten = sprites["test kitten"];
			Scene4BucketShadow = sprites["intro/scene4_bucket_shadow"];
			DemonFire = sprites["demon_fire"];
			Barrel = sprites["barrel"];
			Scene1KnightFaceStrip3 = sprites["outro/scene1_knight_face_strip3"];
			Cleric = sprites["cleric"];
			Ded1 = sprites["ded1"];
			Ded1Dmg = sprites["ded1_dmg"];
			Demon = sprites["demon"];
			DemonDmg = sprites["demon_dmg"];
			Bro = sprites["Stuff/bro"];
			BroDmg = sprites["Stuff/bro_dmg"];
			Chiggin = sprites["chiggin"];
			Cabbage1Smashed = sprites["cabbage1_smashed"];
			Cabbage2Smashed = sprites["cabbage2_smashed"];
			Boulder1 = sprites["Stuff/boulder1"];
			Cabbage3Smashed = sprites["cabbage3_smashed"];
			Boulder2 = sprites["Stuff/boulder2"];
			Cabbage2 = sprites["cabbage2"];
			Cabbage0 = sprites["cabbage_0"];
			Boulder3 = sprites["Stuff/boulder3"];
			Bottle = sprites["Stuff/bottle"];
			Cabbage3 = sprites["cabbage3"];
			Bucket = sprites["bucket"];
			ClericHat = sprites["cleric_hat"];
			Sc5KnightSword = sprites["outro/sc5_knight_sword"];
			Basket = sprites["Stuff/basket"];
			Scene4Knight = sprites["outro/scene4_knight"];
			BroWoolhat = sprites["bro_woolhat"];
			Bench = sprites["Stuff/bench"];
			BirdieLegs = sprites["Stuff/birdie_legs"];
			BirdieBody = sprites["Stuff/birdie_body"];
			SpriteFont = sprites["sprite_font"];
			BubbleStrip2 = sprites["bubble_strip2"];
			BenchShadow = sprites["Stuff/bench_shadow"];
			BroHat = sprites["Stuff/bro_hat"];
			BarrelParts = sprites["barrel_parts"];
			BroGlasses = sprites["Stuff/bro_glasses"];
			BroHair = sprites["Stuff/bro_hair"];
			BirdieWing = sprites["Stuff/birdie_wing"];
			AnotherFont = sprites["another_font"];
			Scene2Bkg = sprites["outro/scene2_bkg"];
			Scene4Bkg_1 = sprites["outro/scene4_bkg"];
			Scene2 = sprites["intro/scene2"];
			Scene4Knight_1 = sprites["intro/scene4_knight"];
			Scene3Bkg_1 = sprites["outro/scene3_bkg"];
			Scene1TreesBkgLayer2 = sprites["outro/scene1_trees_bkg_layer2"];
			Scene3Sword = sprites["intro/scene3_sword"];
			Scene2Reflection = sprites["outro/scene2_reflection"];
			Scene4Bucket = sprites["intro/scene4_bucket"];
			Scene3Ground = sprites["outro/scene3_ground"];
			Scene3Hill = sprites["outro/scene3_hill"];
			BstGam = sprites["Textures/bst_gam"];
			
			#endregion sprite_constructors
		}
		
		public static void Unload()
		{
			_content.Unload();
			Loaded = false;
		}
	}
}
