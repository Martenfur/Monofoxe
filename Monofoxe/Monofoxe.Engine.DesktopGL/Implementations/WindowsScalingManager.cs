using Microsoft.Xna.Framework;
using Monofoxe.Engine.Abstractions;
using Monofoxe.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Monofoxe.Engine.DesktopGL.Implementations
{
	internal class WindowsScalingManager : IScalingManager
	{
		public event Action<float> OnScaleChanged;

		public int MinSupportedScale => 1;
		public int MaxSupportedScale => 2;

		private float _dpi = -1;

		public float GetScale()
		{
			if (_overridingScale)
			{
				return _scaleOverride;
			}

			return (int)Math.Min(MaxSupportedScale, Math.Max(MinSupportedScale, GetDpi() / 96f));
		}

		public float GetDpi()
		{
			if (_dpi <= 0)
			{
				Refresh();
			}
			return _dpi;
		}


		public void Refresh()
		{
			var oldDpi = _dpi;

			var _dpiList = new List<int>();
			var _boundsList = new List<System.Drawing.Rectangle>();

			// There is no good way of determining the dpi pf the current display.
			// So, we are taking the dpi data from ALL the available diaplys and then checking
			// the window position against each screen's bounds to emulate the dpi. Holy shit. 
			foreach (var screen in System.Windows.Forms.Screen.AllScreens)
			{
				GetDpi(screen, DpiType.Effective, out var dpiX, out _);
				_dpiList.Add((int)dpiX);
				_boundsList.Add(screen.Bounds);
			}

			var windowPos = Monofoxe.Engine.GameMgr.WindowManager.WindowPosision.ToVector2()
				+ Monofoxe.Engine.GameMgr.WindowManager.CanvasSize / 2;

			for (var i = 0; i < _dpiList.Count; i += 1)
			{
				var p = new Vector2(_boundsList[i].X, _boundsList[i].Y);
				var s = new Vector2(_boundsList[i].Width, _boundsList[i].Height);
				if (GameMath.PointInRectangle(windowPos, p, p + s))
				{
					_dpi = _dpiList[i];
					return;
				}
			}
			_dpi = _dpiList[0];

			if (oldDpi != -1 && _dpi != oldDpi)
			{
				OnScaleChanged?.Invoke(GetScale());
			}
		}


		private bool _overridingScale = false;
		private float _scaleOverride = 1;

		public void OverrideScale(float scale)
		{
			var oldScale = GetScale();
			_overridingScale = true;
			_scaleOverride = Math.Min(MaxSupportedScale, Math.Max(MinSupportedScale, scale));

			if (GetScale() != oldScale)
			{
				OnScaleChanged?.Invoke(GetScale());
			}
		}


		public void ResetScaleOverride()
		{
			var oldScale = GetScale();

			_overridingScale = false;
			if (GetScale() != oldScale)
			{
				OnScaleChanged?.Invoke(GetScale());
			}
		}


		#region Native magic.

		public static void GetDpi(System.Windows.Forms.Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
		{
			var v = Environment.OSVersion;
			if (v.Version.Major == 6 && v.Version.Minor == 1)
			{
				// Windows 7 doesn't have Shcore.dll. Nice.
				dpiX = 96;
				dpiY = 96;
			}
			else
			{
				var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
				var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);

				GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
			}
		}

		//https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
		[DllImport("User32.dll")]
		private static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

		//https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
		[DllImport("Shcore.dll")]
		private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

		public enum DpiType
		{
			Effective = 0,
			Angular = 1,
			Raw = 2,
		}
		#endregion
	}
}
