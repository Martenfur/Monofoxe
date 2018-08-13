using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/*
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

		void FixedUpdateEnd(List<IComponent> components);
		void FixedUpdate(List<IComponent> components);
		void FixedUpdateBegin(List<IComponent> components);

		void Destroy(IComponent component);
		
	}
	*/
	public interface ISystem
	{
		string Tag {get;}

		void Create(IComponent component);
		void Destroy(IComponent component);	
		
		void Update(List<IComponent> components);
		void Draw(List<IComponent> components);
	}
	
	public interface ISystemExtEvents
	{
		void UpdateEnd(List<IComponent> components);
		void UpdateBegin(List<IComponent> components);

		void DrawBegin(List<IComponent> components);
		void DrawEnd(List<IComponent> components);
	}

	public interface ISystemDrawGUIEvents
	{
		void DrawGUI(List<IComponent> components);
	}

	public interface ISystemFixedUpdateEvents
	{
		void FixedUpdateEnd(List<IComponent> components);
		void FixedUpdate(List<IComponent> components);
		void FixedUpdateBegin(List<IComponent> components);
	}


}
