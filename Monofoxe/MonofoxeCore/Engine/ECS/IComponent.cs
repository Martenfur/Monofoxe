using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine;

namespace Monofoxe.Engine.ECS
{
	public interface IComponent
	{
		string Tag {get;}
		Entity Owner {get; set;}

	}
}
