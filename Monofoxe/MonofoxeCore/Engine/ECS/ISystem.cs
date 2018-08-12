using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	public interface ISystem
	{
		string Tag {get;}

		
		void Create(IComponent component);

		void UpdateEnd(List<IComponent> components);
		void Update(List<IComponent> components);
		void UpdateBegin(List<IComponent> components);

		void DrawBegin(List<IComponent> components);
		void Draw(List<IComponent> components);
		void DrawEnd(List<IComponent> components);

		void DrawGUI(List<IComponent> components);
	}
}
