using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Monofoxe.Utils;
using Resources.Sprites;
using Resources;
using Monofoxe.Engine.Audio;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Systems;
using Monofoxe.ECSTest.Components;


namespace Monofoxe.Test
{
	public class AudioTester : Entity
	{
		float lowpass = 1f;

		Sound snd1;
		Sound snd2;
		Sound snd3;

		FMOD.ChannelGroup group;//new FMOD.ChannelGroup((IntPtr)0);

		public AudioTester() : base(Layer.Get("default"))
		{
			snd1 = AudioMgr.LoadStreamedSound("Music/m_mission", FMOD.MODE._3D);
			snd2 = AudioMgr.LoadStreamedSound("Music/m_peace");
			snd3 = AudioMgr.LoadSound("Sounds/punch", FMOD.MODE._3D);
			
			AudioMgr.ListenerCount = 1;
			AudioMgr.SetListenerPosition(new Vector2(256, 256), 0);

			group = AudioMgr.CreateChannelGroup("group");
			
		}

		public override void Update()
		{
			snd1.Set3DAttributes(Input.ScreenMousePos, Vector2.Zero);
			snd1.Set3DMinMaxDistance(100, 300);

			if (Input.CheckButtonPress(Buttons.A))
			{
				snd1.Play(group);
			}
			if (Input.CheckButtonPress(Buttons.S))
			{
				snd2.Play(group);
			}
			if (Input.CheckButton(Buttons.D))
			{
				if (!snd3.IsPlaying)
				{
					snd3.Play(group);
					snd3.Channel?.setReverbProperties(0, lowpass);
				}
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

				//snd1.LowPass = lowpass;
			}
			if (Input.CheckButton(Buttons.W))
			{
				lowpass -= 0.1f;
				if (lowpass < 0.1f)
				{
					lowpass = 0.1f;
				}
				//snd1.LowPass = lowpass;
				group.setLowPassGain(lowpass);
			}

		}
	}
}
