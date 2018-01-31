using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Monofoxe.Engine
{
	
	/// <summary>
	/// Parent class of every in-game object.
	/// </summary>
	public class GameObj
	{
		/// <summary>
		/// Depth of Draw event. Objects with the lowest depth draw the last.
		/// </summary>
		public int Depth = 0;
	
		/// <summary>
		/// If false, Update and Draw events won't be executed.
		/// </summary>
		public bool Active = true;

		public GameObj()
		{
			Objects.AddObject(this);
		}

		/// <summary>
		/// Begin of the update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdateBegin() {}		

		/// <summary>
		/// Update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdate() {}
		
		/// <summary>
		/// End of the update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdateEnd() {}

		///////////////////////////////////////////////////////////////////

		/// <summary>
		/// Begin of the update at every frame.
		/// </summary>
		public void UpdateBegin() {}		

		
		/// <summary>
		/// Update at every frame.
		/// </summary>
		public virtual void Update() {}
		
		
		/// <summary>
		/// End of the update at every frame.
		/// </summary>
		public virtual void UpdateEnd() {}

		///////////////////////////////////////////////////////////////////

		/// <summary>
		/// Begin of the draw event.
		/// </summary>
		public virtual void DrawBegin() {}		

		
		/// <summary>
		/// Draw event.
		/// </summary>
		public virtual void Draw() {}
		
		
		/// <summary>
		/// End of the draw event.
		/// </summary>
		public virtual void DrawEnd() {}


		/// <summary>
		///	Drawing on a GUI layer. 
		/// </summary>
		public virtual void DrawGUI() {}
	}
}
