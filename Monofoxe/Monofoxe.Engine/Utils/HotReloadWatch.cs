using System;

// Magic sauce that binds this class to the hot reload event handler.
[assembly: System.Reflection.Metadata.MetadataUpdateHandler(typeof(Monofoxe.Engine.Utils.HotReloadWatch))]

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Watcher that provides event handlers for code hot reloading.
	/// </summary>
	public class HotReloadWatch
	{
		// One of the rare cases where sigletons are actually a necessary evil. :(
		// I blame the weird hot reloader api.
		public static HotReloadWatch Instance { get; } = new HotReloadWatch();

		private HotReloadWatch() { }


		/// <summary>
		/// Gets called on hot reload AFTER the code has been updated and BEFORE OnHotReload event.
		/// Type[] parameter contains the set of types that were affected by the metadata update. 
		/// If it's null, any type may have been updated.
		/// </summary>
		public event Action<Type[]> OnHotReloadBegin;

		/// <summary>
		/// Gets called on hot reload AFTER the code has been updated and AFTER OnHotReload event.
		/// Type[] parameter contains the set of types that were affected by the metadata update. 
		/// If it's null, any type may have been updated.
		/// </summary>
		public event Action<Type[]> OnHotReload;


		/// <summary>
		/// A special method that gets called right after hot reload.
		/// See: https://learn.microsoft.com/en-us/dotnet/api/system.reflection.metadata.metadataupdatehandlerattribute?view=net-7.0
		/// </summary>
		private static void ClearCache(Type[] types) =>
			Instance.OnHotReloadBegin?.Invoke(types);

		/// <summary>
		/// A special method that gets called right after hot reload and after all ClearCache methods have finished.
		/// See: https://learn.microsoft.com/en-us/dotnet/api/system.reflection.metadata.metadataupdatehandlerattribute?view=net-7.0
		/// </summary>
		private static void UpdateApplication(Type[] types) =>
			Instance.OnHotReload?.Invoke(types);
	}
}
