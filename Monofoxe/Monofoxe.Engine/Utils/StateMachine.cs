using System.Collections.Generic;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.Utils
{
	public delegate void StateMachineDelegate<T>(StateMachine<T> caller, Entity owner);

	/// <summary>
	/// Stack-based state machine.
	/// </summary>
	public class StateMachine<T>
	{
		/// <summary>
		/// All the available states. Excuted on each update.
		/// </summary>
		protected Dictionary<T, StateMachineDelegate<T>> _states;	
		
		/// <summary>
		/// State enter events. Executed when machine enters a certain event.
		/// </summary>
		protected Dictionary<T, StateMachineDelegate<T>> _enterStateEvents;

		/// <summary>
		/// State exit events. Executed when machine exits a certain event.
		/// </summary>
		protected Dictionary<T, StateMachineDelegate<T>> _exitStateEvents;
		

		/// <summary>
		/// Stack of active states.
		/// </summary>
		protected Stack<T> _stateStack;


		/// <summary>
		/// Current state machine state.
		/// </summary>
		public T CurrentState => _stateStack.Peek();

		/// <summary>
		/// Previous state machine state.
		/// </summary>
		public T PreviousState {get; protected set;}


		/// <summary>
		/// State machine owner.
		/// </summary>
		public Entity Owner;



		public StateMachine(T startingState, Entity owner) 
		{
			Owner = owner;

			_states = new Dictionary<T, StateMachineDelegate<T>>();
			_enterStateEvents = new Dictionary<T, StateMachineDelegate<T>>();
			_exitStateEvents = new Dictionary<T, StateMachineDelegate<T>>();

			_stateStack = new Stack<T>();
			_stateStack.Push(startingState);
			PreviousState = startingState;
		}
		

		/// <summary>
		/// Updates state machine and executes current state method.
		/// </summary>
		public virtual void Update() =>
			_states[CurrentState](this, Owner);
		

		
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
		/// Empties state stack and sets state to a new one without calling exit or enter events.
		/// </summary>
		public virtual void Reset(T state)
		{
			PreviousState = CurrentState;
			_stateStack.Clear();
			_stateStack.Push(state);
		}


		
		/// <summary>
		/// Adds new state to a state machine.
		/// </summary>
		public virtual void AddState(
			T stateKey, 
			StateMachineDelegate<T> stateMethod, 
			StateMachineDelegate<T> enterStateEvent = null, 
			StateMachineDelegate<T> exitStateEvent = null
		)
		{
			_states.Add(stateKey, stateMethod);
			if (enterStateEvent != null)
			{
				_enterStateEvents.Add(stateKey, enterStateEvent);
			}
			if (exitStateEvent != null)
			{
				_exitStateEvents.Add(stateKey, exitStateEvent);
			}
		}
		

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
		public virtual void PushState(T state)
		{
			PreviousState = CurrentState;
			_stateStack.Push(state);
			CallExitEvent(PreviousState);
			CallEnterEvent(CurrentState);
		}

		
		/// <summary>
		/// Pops current active state from a machine.
		/// </summary>
		public virtual T PopState()
		{
			PreviousState = _stateStack.Pop();
			CallExitEvent(PreviousState);
			CallEnterEvent(CurrentState);
			return PreviousState;
		}
		

		/// <summary>
		/// Replaces current state with a new state. Basically, pop and push together.
		/// </summary>
		public virtual T ChangeState(T state)
		{
			PreviousState = _stateStack.Pop();
			_stateStack.Push(state);
			CallExitEvent(PreviousState);
			CallEnterEvent(CurrentState);

			return PreviousState;
		}



		/// <summary>
		/// Calls exit event for a state.
		/// </summary>
		protected void CallExitEvent(T state)
		{
			if (_exitStateEvents.TryGetValue(state, out StateMachineDelegate<T> exitEvent))
			{
				exitEvent(this, Owner);
			}
		}

		/// <summary>
		/// Calls enter event for a state.
		/// </summary>
		protected void CallEnterEvent(T state)
		{
			if (_enterStateEvents.TryGetValue(state, out StateMachineDelegate<T> enterEvent))
			{
				enterEvent(this, Owner);
			}
		}

	}
}
