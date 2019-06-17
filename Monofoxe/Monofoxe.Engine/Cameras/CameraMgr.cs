using System.Collections.Generic;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.Cameras
{
	/// <summary>
	/// Manages camera priorities.
	/// </summary>
	public static class CameraMgr
	{
		/// <summary>
		/// List of all cameras.
		/// </summary>
		public static IReadOnlyCollection<Camera> Cameras => _cameras.ToList();

		private static SafeSortedList<Camera> _cameras = new SafeSortedList<Camera>(x => x.Priority);

		/// <summary>
		/// Removes camera from list and adds it again, taking in account its proirity.
		/// </summary>
		internal static void UpdateCameraPriority(Camera camera)
		{
			_cameras.Remove(camera);
			_cameras.Add(camera);
		}

		internal static void RemoveCamera(Camera camera) =>
			_cameras.Remove(camera);

	}
}
