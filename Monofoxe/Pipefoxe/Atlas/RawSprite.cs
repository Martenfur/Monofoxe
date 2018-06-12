using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pipefoxe.Atlas
{
	class RawSprite
	{
		public string Name = "NONE";
		public int FramesH = 1;
		public int FramesV = 1;
		public Point Offset = new Point(0, 0);
		public Image Texture; 
	}
}
