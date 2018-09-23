using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.SceneSystem
{
	public static class LayerMgr
	{
		/// <summary>
		/// List of all existing layers.
		/// </summary>
		public static IReadOnlyCollection<Layer> Layers => _layers;

		/// <summary>
		/// List of all existing layers.
		/// </summary>
		private static List<Layer> _layers = new List<Layer>();
		

		/// <summary>
		/// Creates new layer with given name.
		/// </summary>
		public static Layer CreateLayer(string name, int depth = 0)
		{
			if (LayerExists(name))
			{
				throw(new Exception("Layer with such name already exists!"));
			}
			
			return new Layer(name, depth);
		}

		/// <summary>
		/// Destroys given layer.
		/// </summary>
		public static void DestroyLayer(Layer layer)
		{
			if (_layers.Remove(layer))
			{
				foreach(var entity in layer._entities)
				{
					EntityMgr.DestroyEntity(entity);
				}
			}
		}

		/// <summary>
		/// Destroys layer with given name.
		/// </summary>
		public static void DestroyLayer(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					 _layers.Remove(layer);
					foreach(var entity in layer._entities)
					{
						EntityMgr.DestroyEntity(entity);
					}
				}
			}
		}


		/// <summary>
		/// Returns layer with given name.
		/// </summary>
		public static Layer GetLayer(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					return layer;
				}
			}
			return null;
		}


		/// <summary>
		/// Returns true, if there is a layer with given name. 
		/// </summary>
		public static bool LayerExists(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Executes Draw, DrawBegin and DrawEnd events.
		/// </summary>
		internal static void CallDrawEvents()
		{
			foreach(var layer in _layers)
			{
				if (!layer.IsGUI)
				{
					SystemMgr.Draw(layer._depthSortedComponents);
					foreach(var entity in layer._depthSortedEntities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.Draw();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Executes Draw GUI events.
		/// </summary>
		internal static void CallDrawGUIEvents()
		{
			foreach(var layer in _layers)
			{
				if (layer.IsGUI)
				{
					SystemMgr.Draw(layer._depthSortedComponents);
					foreach(var entity in layer._depthSortedEntities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.Draw();
						}
					}
				}
			}
		}



		/// <summary>
		/// Removes layer from list and adds it again, taking in account its proirity.
		/// </summary>
		internal static void UpdateLayerPriority(Layer layer)
		{
			_layers.Remove(layer);
			for(var i = 0; i < _layers.Count; i += 1)
			{
				if (layer.Priority > _layers[i].Priority)
				{
					_layers.Insert(i, layer);
					return;
				}
			}
			_layers.Add(layer);
		}



		/// <summary>
		/// Returns info about layers.
		/// </summary>
		public static string __GetLayerInfo()
		{
			var str = new StringBuilder();

			foreach(var layer in _layers)
			{
				str.Append(
					layer.Name + 
					"; Priority: " + layer.Priority + 
					"; is GUI: " + layer.IsGUI + 
					"; Ent: " + layer._entities.Count +
					Environment.NewLine
				);
			}

			return str.ToString();
		}


	}
}
