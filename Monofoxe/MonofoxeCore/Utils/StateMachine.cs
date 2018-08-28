using System.Collections.Generic;

namespace Monofoxe.Utils
{
	public delegate void StateMachineDelegate<T>(StateMachine<T> caller);

	/// <summary>
	/// Stack-based state machine.
	/// </summary>
	public class StateMachine<T>
	{
		/// <summary>
		/// All the available states.
		/// </summary>
		protected Dictionary<T, StateMachineDelegate<T>> _states;	

		/// <summary>
		/// Stack of active states.
		/// </summary>
		protected Stack<T> _stateStack;

		public T CurrentState => _stateStack.Peek();

		public readonly T DefaultState;


		public StateMachine(T _defaultState) 
		{
			_states = new Dictionary<T, StateMachineDelegate<T>>();
			_stateStack = new Stack<T>();
			DefaultState = _defaultState;
			_stateStack.Push(_defaultState);
		}
		

		/// <summary>
		/// Updates state machine and executes current state method.
		/// </summary>
		public virtual void Update() =>
			_states[CurrentState](this);
		

		
		/// <summary>
		/// Empties state stack, but keeps current state.
		/// </summary>
		public virtual void ClearStack()
		{
			var currentState = CurrentState;
			_stateStack.Clear();
			_stateStack.Push(currentState);
		}


		/// <summary>
		/// Empties state stack and resets state to default value.
		/// </summary>
		public virtual void Reset()
		{
			_stateStack.Clear();
			_stateStack.Push(DefaultState);
		}


		
		/// <summary>
		/// Adds new state to a state machine.
		/// </summary>
		public virtual void AddState(T stateName, StateMachineDelegate<T> stateMethod) =>
			_states.Add(stateName, stateMethod);
		

		/// <summary>
		/// Removes existing state from a state machine.
		/// </summary>
		public virtual void RemoveState(T state) =>
			_states.Remove(state);


		/// <summary>
		/// Pushes new state to a state machine.
		/// 
		/// NOTE: State should already exist in the machine.
		/// </summary>
		public virtual void PushState(T state) =>
			_stateStack.Push(state);

		
		/// <summary>
		/// Pops current active state from a machine.
		/// </summary>
		public virtual T PopState() =>
			_stateStack.Pop();
		

		/// <summary>
		/// Replaces current state with a new state. Basically, pop and push together.
		/// </summary>
		public virtual T ReplaceState(T state)
		{
			var oldState = _stateStack.Pop();
			_stateStack.Push(state);
			return oldState;
		}
	}
}
