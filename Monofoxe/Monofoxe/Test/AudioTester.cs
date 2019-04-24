using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Monofoxe.Engine.Utils;
using Resources.Sprites;
using Resources;
using ChaiFoxes.FMODAudio;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.ECSTest.Systems;
using Monofoxe.ECSTest.Components;
using System.Runtime.InteropServices;

namespace Monofoxe.Test
{
	public class AudioTester : Entity
	{
		float lowpass = 1f;

		Sound snd1;
		Sound snd2;
		Sound snd3;

		Listener3D listener;

		FMOD.ChannelGroup group;//new FMOD.ChannelGroup((IntPtr)0);

		public AudioTester() : base(SceneMgr.GetScene("default")["default"])
		{
			snd1 = AudioMgr.LoadStreamedSound("Music/m_mission.ogg");
			snd1.Is3D = true;
			snd1.MinDistance3D = 100;
			snd1.MaxDistance3D = 300;
			snd2 = AudioMgr.LoadStreamedSound("Music/m_peace.ogg");
			snd3 = AudioMgr.LoadSound("Sounds/punch.wav");
			snd3.Is3D = true;

			listener = Listener3D.Create();
			listener.Position3D = new Vector3(256, 256, 0);

			group = AudioMgr.CreateChannelGroup("group");
			
		}

		public override void Update()
		{
			listener.Position3D = Input.ScreenMousePosition.ToVector3();
			
			if (Input.CheckButtonPress(Buttons.A))
			{
				snd1.Play(group);
				
			}
			if (Input.CheckButtonPress(Buttons.S))
			{
				snd2.Play(group);
			}

			if (Input.CheckButtonPress(Buttons.O))
			{
				group.stop();
			}

			if (Input.CheckButton(Buttons.Q))
			{
				lowpass += 0.1f;
				if (lowpass > 1000)
				{
					lowpass = 1;
				}	
				group.setLowPassGain(lowpass);
			}
			if (Input.CheckButton(Buttons.W))
			{
				lowpass -= 0.1f;
				if (lowpass < 0.1f)
				{
					lowpass = 0.1f;
				}
				group.setLowPassGain(lowpass);
			}

		}
	}
}
