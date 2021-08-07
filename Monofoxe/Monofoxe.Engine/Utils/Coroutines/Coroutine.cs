using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils.Coroutines
{
	public class Coroutine : IDisposable
	{
		public bool Paused;


		internal virtual void Reset(IEnumerator routine)
		{
			RoutinesStack.Clear();
			RoutinesStack.Push(routine);
			Paused = false;
		}


		internal virtual bool Update()
		{
			if (Paused)
			{ 
				return true;
			}

			if (RoutinesStack.Peek().MoveNext())
			{
				if (RoutinesStack.Peek().Current is IEnumerator enumerator)
				{
					RoutinesStack.Push(enumerator);
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


		internal readonly Stack<IEnumerator> RoutinesStack = new Stack<IEnumerator>(1);


		public virtual void Dispose()
		{
			foreach (var routine in RoutinesStack)
			{
				routine.Reset();
			}
			RoutinesStack.Clear();
		}
	}
}
