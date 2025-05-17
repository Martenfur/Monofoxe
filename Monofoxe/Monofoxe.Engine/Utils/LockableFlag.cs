using System;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// A bool flag that can be locked by multiple sources. 
	/// </summary>
	public class LockableFlag
	{
		/// <summary>
		/// Is true if locked by at least one source.
		/// </summary>
		public bool Locked => _lockingSources != 0;

		private int _lockingSources = 0;

		/// <summary>
		/// Adds a lock to the flag. Locked flag's value is false.
		/// </summary>
		public void AddLock() =>
			_lockingSources += 1;

		/// <summary>
		/// Removes a lock from the flag. Flag will only become true if all locks are removed.
		/// </summary>
		public void RemoveLock()
		{
			_lockingSources -= 1;
			if (_lockingSources < 0)
			{
				throw new InvalidOperationException("Cannot unblock an already unblocked flag!");
			}
		}


		public static bool operator !(LockableFlag a) => !a.Locked;
		public static bool operator ==(LockableFlag a, bool b) => a.Locked == b;
		public static bool operator ==(bool a, LockableFlag b) => b.Locked == a;
		public static bool operator !=(LockableFlag a, bool b) => a.Locked != b;
		public static bool operator !=(bool a, LockableFlag b) => b.Locked != a;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (ReferenceEquals(obj, null))
			{
				return false;
			}

			return false;
		}

		public override int GetHashCode() =>
			Locked.GetHashCode();
	}
}
