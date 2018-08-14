using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Basic system interface. 
	/// </summary>
	public interface ISystem
	{
		/// <summary>
		/// Identifying tag. 
		/// 
		/// NOTE: Tags for different systems don't really have to be unique.
		/// Systems will only process components with matching tags.
		/// So, different systems with same tags will process same sets of components.
		/// </summary>
		string Tag {get;}
		
		// TODO: Add per-system deactivation, if needed.

		/// <summary>
		/// Create event is called right after new component is created.
		/// 
		/// NOTE: This event won't be called right after component creation.
		/// It will be called in the very beginning of next step, so keep this in mind.
		/// However, you can speed up this process by calling 
		/// </summary>
		void Create(Component component);
		void Destroy(Component component);	
		
		void Update(List<Component> components);
		void Draw(List<Component> components);
	}
	
	public interface ISystemExtEvents
	{
		void UpdateEnd(List<Component> components);
		void UpdateBegin(List<Component> components);

		void DrawBegin(List<Component> components);
		void DrawEnd(List<Component> components);
	}

	public interface ISystemDrawGUIEvents
	{
		void DrawGUI(List<Component> components);
	}

	public interface ISystemFixedUpdateEvents
	{
		void FixedUpdateEnd(List<Component> components);
		void FixedUpdate(List<Component> components);
		void FixedUpdateBegin(List<Component> components);
	}


}
