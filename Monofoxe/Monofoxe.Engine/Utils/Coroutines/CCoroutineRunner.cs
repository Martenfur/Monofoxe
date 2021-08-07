using Microsoft.Extensions.ObjectPool;
using Monofoxe.Engine.EC;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils.Coroutines
{
	internal class CCoroutineRunner : Component
	{
		private List<Coroutine> _activeCoroutines = new List<Coroutine>();
		private List<Coroutine> _incomingCoroutines = new List<Coroutine>();
		
		public Coroutine StartCoroutine(IEnumerator routine)
		{
			var coroutine = new Coroutine();
			coroutine.Reset(routine);
			_incomingCoroutines.Add(coroutine);
			return coroutine;
		}


		public bool WasRemoved(Coroutine coroutine) =>
			!_activeCoroutines.Contains(coroutine) && !_incomingCoroutines.Contains(coroutine);


		public void StopCoroutine(Coroutine coroutine)
		{
			int index = _activeCoroutines.FindIndex((c) => c == coroutine);

			if (index != -1)
			{
				_activeCoroutines.RemoveAt(index);
			}
		}


		public override void Update()
		{
			_activeCoroutines.AddRange(_incomingCoroutines);
			_incomingCoroutines.Clear();

			for (var i = 0; i < _activeCoroutines.Count; i++)
			{
				if (!_activeCoroutines[i].Update())
				{
					_activeCoroutines[i] = null;
				}
			}

			_activeCoroutines.RemoveAll(x => x == null);
		}

		public override void Initialize() {}

		public override void FixedUpdate() {}

		public override void Draw() {}

		public override void Destroy() {}
	}
}
