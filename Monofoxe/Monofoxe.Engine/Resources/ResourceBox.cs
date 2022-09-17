using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.Resources
{
	/// <summary>
	/// Abstract container for game resources of the specific type.
	/// Resources can be accessed by the string key.
	/// </summary>
	public abstract class ResourceBox<T> : IResourceBox, IEnumerable<KeyValuePair<string, T>>
	{
		/// <summary>
		/// Tells if the resources been loaded already.
		/// </summary>
		public bool Loaded { get; protected set; }

		public Type Type { get; private set; }

		public readonly string Name;

		public int Count => _resources.Count;

		public ResourceBox(string name)
		{
			Name = name;
			Type = typeof(T);
			ResourceHub.AddResourceBox(name, this);
		}

		/// <summary>
		/// Loads all resources.
		/// </summary>
		public abstract void Load();

		/// <summary>
		/// Unloads all resources.
		/// </summary>
		public abstract void Unload();


		private Dictionary<string, T> _resources = new Dictionary<string, T>();

		/// <summary>
		/// Returns the resource with a specific key.
		/// </summary>
		public T GetResource(string key) =>
			_resources[key];


		/// <summary>
		/// Returns the resource with a specific key.
		/// </summary>
		public bool TryGetResource(string key, out T value) =>
			_resources.TryGetValue(key, out value);


		/// <summary>
		/// Returns true, if the box contains resource with provided key.
		/// </summary>
		public bool ContainsResource(string key) =>
			_resources.ContainsKey(key);
		
		
		public void AddResource(string key, T resource) =>
			_resources.Add(key, resource);
			
		public void RemoveResource(string key) =>
			_resources.Remove(key);
		
			
		IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, T>>)_resources).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, T>>)_resources).GetEnumerator();
		}
	}
}
