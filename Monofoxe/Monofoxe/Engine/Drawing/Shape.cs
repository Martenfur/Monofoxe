using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Geometric shape.
	/// </summary>
	public class Shape: Drawable
	{
		public bool IsOutline;
		protected short[] indexes;

		protected static void DrawPrimitive(PrimitiveType type, VertexPositionColor[] vertices, short[] inds, int prAmount)
		{
			DrawCntrl.Batch.End();
			
			foreach(EffectPass pass in DrawCntrl.BasicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				DrawCntrl.Device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, inds, 0, prAmount);
			}
			

			if (DrawCntrl.CurrentCamera != null)		
			{DrawCntrl.Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, DrawCntrl.CurrentCamera.CreateTranslationMatrix());}
		
		}

		protected static void DrawPrimitive(PrimitiveType type, VertexBufferBinding[] buffers, short[] inds, int prAmount)
		{
			DrawCntrl.Batch.End();
			
			DrawCntrl.Device.SetVertexBuffers(buffers);

			foreach(EffectPass pass in DrawCntrl.BasicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				//DrawCntrl.Device.DrawIndexedPrimitives(type, 0, 0, buffers.Length, inds, 0, prAmount);
				var i = new DynamicVertexBuffer(DrawCntrl.Device,new VertexDeclaration(),1,BufferUsage.WriteOnly);
				
				
				
			}
			

			if (DrawCntrl.CurrentCamera != null)		
			{DrawCntrl.Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, DrawCntrl.CurrentCamera.CreateTranslationMatrix());}
		
		}

	}
}
