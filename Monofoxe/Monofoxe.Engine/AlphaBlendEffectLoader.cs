using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Monofoxe.Engine
{
	public interface IAlphaBlendEffectLoader
	{ 
		Effect Load();
	}

	public abstract class AlphaBlendEffectLoader : IAlphaBlendEffectLoader
	{
		protected abstract string _effectName { get; }
		protected abstract Assembly _assembly { get; }

		private readonly object _lock = new object();
		
		public byte[] Bytecode
		{
			get
			{
				if (_bytecode == null)
				{
					lock (_lock)
					{
						if (_bytecode != null)
						{
							return _bytecode;
						}

						var stream = _assembly.GetManifestResourceStream(_effectName);
						using (var ms = new MemoryStream())
						{
							stream.CopyTo(ms);
							_bytecode = ms.ToArray();
						}
					}
				}

				return _bytecode;
			}
		}
		private volatile byte[] _bytecode;


		public Effect Load() =>
			new Effect(GraphicsMgr.Device, Bytecode);
	}
}
