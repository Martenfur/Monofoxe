using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.CoroutineSystem
{
	public class Coroutine<T> : Coroutine
	{
		internal override void Reset(IEnumerator routine)
		{
			base.Reset(routine);
			OnYieldValue = null;
		}

		internal override bool Update()
		{
			if (Paused)
			{
				return true;
			}

			if (RoutinesStack.Peek().MoveNext())
			{
				if (RoutinesStack.Peek().Current is IEnumerator<T> enumerator)
				{
					RoutinesStack.Push(enumerator);
				}
				else
				{
					if (RoutinesStack.Peek().Current is T valueT)
					{
						OnYieldValue?.Invoke(this, valueT);
					}
				}
			}
			else
			{
				RoutinesStack.Pop();
				if (RoutinesStack.Count == 0)
				{
					return false;
				}
			}

			return true;
		}

		public event EventHandler<T> OnYieldValue;

		public override void Dispose()
		{
			base.Dispose();
			OnYieldValue = null;
		}
	}
}
