using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Effect extensions.
	/// </summary>
	public static class EffectExtensions
	{
		/// <summary>
		/// Sets world, view and projection for an effect.
		/// 
		/// NOTE: Effect should actually have "World", "View" and "Projection" parameters.
		/// </summary>
		public static void SetWorldViewProjection(this Effect effect, Matrix world, Matrix view, Matrix projection)
		{/*
			effect.Parameters["World"].SetValue(world);
			effect.Parameters["View"].SetValue(view);
			effect.Parameters["Projection"].SetValue(projection);
			*/
			//TODO: Remove.
		}

	}
}
