using Monofoxe.Engine.Abstractions;
using System.Runtime.InteropServices;

namespace Monofoxe.Engine.DesktopGL.Implementations
{
	internal class WindowsMonitorFrequencyManager : IMonitorFrequencyManager
	{
		public int GetCurrentMonitorFrequency()
		{
			var vDevMode = new DEVMODE();
			EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref vDevMode);

			return vDevMode.dmDisplayFrequency;
		}


		#region Native magic.

		private const int ENUM_CURRENT_SETTINGS = -1;

		[DllImport("user32.dll")]
		public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
		//static extern bool EnumDisplaySettingsEx(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode, uint dwFlags);

		//
		// Summary:
		//     Specifies the angle of the screen.
		public enum ScreenOrientation
		{
			//
			// Summary:
			//     The screen is oriented at 0 degrees.
			Angle0 = 0,
			//
			// Summary:
			//     The screen is oriented at 90 degrees.
			Angle90 = 1,
			//
			// Summary:
			//     The screen is oriented at 180 degrees.
			Angle180 = 2,
			//
			// Summary:
			//     The screen is oriented at 270 degrees.
			Angle270 = 3
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct DEVMODE
		{
			private const int CCHDEVICENAME = 0x20;
			private const int CCHFORMNAME = 0x20;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public string dmDeviceName;
			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public int dmFields;
			public int dmPositionX;
			public int dmPositionY;
			public ScreenOrientation dmDisplayOrientation;
			public int dmDisplayFixedOutput;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public string dmFormName;
			public short dmLogPixels;
			public int dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;
			public int dmDisplayFlags;
			public int dmDisplayFrequency;
			public int dmICMMethod;
			public int dmICMIntent;
			public int dmMediaType;
			public int dmDitherType;
			public int dmReserved1;
			public int dmReserved2;
			public int dmPanningWidth;
			public int dmPanningHeight;
		}
		#endregion
	}
}
