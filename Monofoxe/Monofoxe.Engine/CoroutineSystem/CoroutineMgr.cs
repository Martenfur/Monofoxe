using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.CoroutineSystem
{
  public static class CoroutineMgr
	{
		internal static List<Coroutine> ActiveCoroutines = new List<Coroutine>();
		internal static List<Coroutine> IncomingCoroutines = new List<Coroutine>();
		internal static ObjectPool<Coroutine> CoroutinePool = ObjectPool.Create<Coroutine>();
		internal static Dictionary<Type, ObjectPool<Coroutine>> Dict = new Dictionary<Type, ObjectPool<Coroutine>>();

		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			var coroutine = CoroutinePool.Get();
			coroutine.Reset(routine);
			IncomingCoroutines.Add(coroutine);
			return coroutine;
		}

		private static ObjectPool<Coroutine<T>> GetPool<T>()
		{
			ObjectPool<Coroutine<T>> pool;
			if (!Dict.TryGetValue(typeof(T), out var poolBasic))
			{
				pool = ObjectPool.Create(new DefaultPooledObjectPolicy<Coroutine<T>>());
				Dict.Add(typeof(T), pool as ObjectPool<Coroutine>);
			}
			else
			{
				pool = poolBasic as ObjectPool<Coroutine<T>>;
			}

			return pool;
		}
		
		public static Coroutine<T> StartCoroutine<T>(IEnumerator routine)
		{
			var coroutine = GetPool<T>().Get();
			coroutine.Reset(routine);
			IncomingCoroutines.Add(coroutine);
			return coroutine;
		}

		public static bool WasRemoved(Coroutine coroutine) => 
			!ActiveCoroutines.Contains(coroutine) && !IncomingCoroutines.Contains(coroutine);

		public static void StopCoroutine(Coroutine coroutine)
		{
			int index = ActiveCoroutines.FindIndex((c) => c == coroutine);

			if (index != -1)
			{
				CoroutinePool.Return(ActiveCoroutines[index]);
				ActiveCoroutines.RemoveAt(index);
			}
		}
		
		public static void StopCoroutine<T>(Coroutine<T> coroutine)
		{
			int index = ActiveCoroutines.FindIndex((c) => c == coroutine);

			if (index != -1)
			{
				GetPool<T>().Return(ActiveCoroutines[index] as Coroutine<T>);
				ActiveCoroutines.RemoveAt(index);
			}
		}

		internal static void PreUpdateRoutine()
		{
			ActiveCoroutines.AddRange(IncomingCoroutines);
			IncomingCoroutines.Clear();
		}

		internal static void UpdateCoroutines()
		{
			for (var i = 0; i < ActiveCoroutines.Count; i++)
			{
				if (ActiveCoroutines[i].Update() == false)
				{
					CoroutinePool.Return(ActiveCoroutines[i]);
					ActiveCoroutines[i] = null;
				}
			}
		}

		internal static void PostUpdateRoutine() =>
			ActiveCoroutines.RemoveAll(x => x == null);
	}
}
