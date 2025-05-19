
namespace Monofoxe.Engine.Utils
{
	public delegate void BufferedInputDelegate();

	/// <summary>
	/// 
	/// </summary>
	public class BufferedInput
	{
		/// <summary>
		/// If the input is blocked, it will not be executed right away.
		/// </summary>
		public bool Blocked = false;

		private readonly Alarm _bufferAlarm;
		private readonly BufferedInputDelegate _invokeAction;

		public BufferedInput(double bufferTime, BufferedInputDelegate invokeAction)
		{
			_bufferAlarm = new Alarm(bufferTime);
			_invokeAction = invokeAction;
		}

		public void Update()
		{
			_bufferAlarm.Update();
			
			if (_bufferAlarm.Running && !Blocked)
			{
				_bufferAlarm.Stop();
				_invokeAction();
			}
		}

		/// <summary>
		/// If input is blocked, buffers the action for later execution. 
		/// If not, executes the action instantly.
		/// </summary>
		public void Activate()
		{
			if (Blocked)
			{ 
				_bufferAlarm.Start();
			}
			else
			{ 
				_invokeAction();
			}
		}
	}
}
