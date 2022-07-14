using System;

namespace Monofoxe.Engine
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class InspectableAttribute : Attribute
	{
		public readonly bool Editable = true;

		public InspectableAttribute(bool editable = true)
		{
			Editable = editable;
		}
	}
}
