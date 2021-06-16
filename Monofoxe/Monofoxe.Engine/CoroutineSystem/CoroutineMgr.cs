using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.ObjectPool;

namespace Monofoxe.Engine.CoroutineSystem
{
	public static class CoroutineMgr
	{
		internal static List<Coroutine> activeCoroutines = new List<Coroutine>();
		internal static List<Coroutine> incomingCoroutines = new List<Coroutine>();
		internal static ObjectPool<Coroutine> coroutinePool = ObjectPool.Create<Coroutine>();
		internal static Dictionary<Type, ObjectPool<Coroutine>> dict = new Dictionary<Type, ObjectPool<Coroutine>>();

		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			var coroutine = coroutinePool.Get();
			coroutine.Reset(routine);
			incomingCoroutines.Add(coroutine);
			return coroutine;
		}

		private static ObjectPool<Coroutine<T>> GetPool<T>()
		{
			ObjectPool<Coroutine<T>> pool;
			if (!dict.TryGetValue(typeof(T), out var poolBasic))
			{
				pool = ObjectPool.Create(new DefaultPooledObjectPolicy<Coroutine<T>>());
				dict.Add(typeof(T), pool as ObjectPool<Coroutine>);
			}
			else
			{
				pool = poolBasic as ObjectPool<Coroutine<T>>;
			}

			return pool;
		}
		
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
		public static Coroutine<T> StartCoroutine<T>(IEnumerator routine)
		{
			var coroutine = GetPool<T>().Get();
			coroutine.Reset(routine);
			incomingCoroutines.Add(coroutine);
			return coroutine;
		}

		public static bool WasRemoved(Coroutine coroutine)
			=> !activeCoroutines.Contains(coroutine) && !incomingCoroutines.Contains(coroutine);

		public static void StopCoroutine(Coroutine coroutine)
		{
			int index = activeCoroutines.FindIndex((c) => c == coroutine);

			if (index != -1)
			{
				coroutinePool.Return(activeCoroutines[index]);
				activeCoroutines.RemoveAt(index);
			}
		}
		
		public static void StopCoroutine<T>(Coroutine<T> coroutine)
		{
			int index = activeCoroutines.FindIndex((c) => c == coroutine);

			if (index != -1)
			{
				GetPool<T>().Return(activeCoroutines[index] as Coroutine<T>);
				activeCoroutines.RemoveAt(index);
			}
		}

		internal static void PreUpdateRoutine()
		{
			activeCoroutines.AddRange(incomingCoroutines);
			incomingCoroutines.Clear();
		}

		internal static void UpdateCoroutines()
		{
			for (var i = 0; i < activeCoroutines.Count; i++)
			{
				if (activeCoroutines[i].Update() == false)
				{
					coroutinePool.Return(activeCoroutines[i]);
					activeCoroutines[i] = null;
				}
			}
		}

		internal static void PostUpdateRoutine()
		{
			activeCoroutines.RemoveAll(x => x == null);
		}
	}
}
