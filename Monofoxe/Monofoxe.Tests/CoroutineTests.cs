using Monofoxe.Engine.CoroutineSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tests
{
  [ExcludeFromCodeCoverage]
	public class CoroutineTests
	{
		[SetUp]
		public void Setup()
		{
			CoroutineMgr.ActiveCoroutines = new List<Coroutine>();
			CoroutineMgr.IncomingCoroutines = new List<Coroutine>();
		}

		private static void DoFullUpdate()
		{
			CoroutineMgr.PreUpdateRoutine();
			CoroutineMgr.UpdateCoroutines();
			CoroutineMgr.PostUpdateRoutine();
		}
		
		private static IEnumerator TestEnumerator()
		{
			var ok = false;
			
			while (!ok)
			{
				yield return null;
				ok = true;
			}
		}

		[Test]
		public void StartsUp_CanMoveNext()
		{
			var coroutine = CoroutineMgr.StartCoroutine(TestEnumerator());

			Assert.True(coroutine.RoutinesStack.Peek().MoveNext());
		}
		
		[Test]
		public void AfterFullUpdate_CannotMoreFurther()
		{
			var coroutine = CoroutineMgr.StartCoroutine(TestEnumerator());
			
			DoFullUpdate();
			
			var enumerator = (coroutine.RoutinesStack.Peek() as IEnumerator);
			Assert.NotNull(enumerator);
			Assert.False(enumerator.MoveNext());
		}

		[Test]
		public void StillInStack_IfPaused()
		{
			var coroutine = CoroutineMgr.StartCoroutine(TestEnumerator());
			coroutine.Paused = true;
			
			DoFullUpdate();
			
			Assert.True(CoroutineMgr.ActiveCoroutines.Contains(coroutine));
		}

		[Test]
		public void GetsRemovedFromStack_WhenItsOver()
		{
			var coroutine = CoroutineMgr.StartCoroutine(TestEnumerator());
			
			DoFullUpdate();
			DoFullUpdate();
			
			Assert.True(CoroutineMgr.WasRemoved(coroutine));
		}

		[Test]
		public void BreaksEarly_WhenSpecified()
		{
			var coroutine = CoroutineMgr.StartCoroutine(TestEnumeratorBreaksOnNegatives(new []{ 1, 2, 3, 4, -20, 1 }));

			DoFullUpdate();
			DoFullUpdate();
			DoFullUpdate();
			DoFullUpdate();
			
			Assert.AreEqual(1, coroutine.RoutinesStack.Count);
			Assert.True(CoroutineMgr.ActiveCoroutines.Contains(coroutine));
			
			DoFullUpdate();
			
			Assert.AreEqual(0, coroutine.RoutinesStack.Count);
			Assert.False(CoroutineMgr.ActiveCoroutines.Contains(coroutine));
		}
		
		private static IEnumerator<int> TestEnumeratorBreaksOnNegatives(int[] sequence)
		{
			foreach (var i in sequence)
			{
				if (i < 0) yield break;
				yield return i;
			}
		}
	}
}
