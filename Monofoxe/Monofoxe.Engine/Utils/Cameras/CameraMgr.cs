using System.Collections.Generic;


namespace Monofoxe.Engine.Utils.Cameras
{

	public static class CameraMgr
	{
		/// <summary>
		/// List of all cameras.
		/// </summary>
		public static IReadOnlyCollection<Camera> Cameras => _cameras;

		private static List<Camera> _cameras = new List<Camera>();

		/// <summary>
		/// Removes layer from list and adds it again, taking in account its proirity.
		/// </summary>
		internal static void UpdateCameraPriority(Camera camera)
		{
			_cameras.Remove(camera);
			for(var i = 0; i < _cameras.Count; i += 1)
			{
				if (camera.Priority > _cameras[i].Priority)
				{
					_cameras.Insert(i, camera);
					return;
				}
			}
			_cameras.Add(camera); // Adding camera at the end, if it has lowest priority.
		}

		internal static void RemoveCamera(Camera camera) =>
			_cameras.Remove(camera);

	}
}
