using System;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Test
{
	public class StateMachineTester : Entity
	{
		public enum States
		{
			Foxe,
			Idle,
			MyBoy,
		}

		StateMachine<States> testMachine;

		public StateMachineTester(Layer layer) : base(layer)
		{
			testMachine = new StateMachine<States>(States.Foxe);
			testMachine.AddState(States.Foxe, StateFoxe, StateFoxeEnter, StateFoxeExit);
			
			testMachine.AddState(States.Idle, StateIdle, null, StateIdleExit);
			testMachine.AddState(States.MyBoy, StateMyBoy, StateMyBoyEnter);


			testMachine.Update();
			testMachine.Update();
			testMachine.Update();
			testMachine.Update();
			testMachine.Update();
			testMachine.Update();
			testMachine.Update();
			testMachine.Update();

		}

		void StateFoxe(StateMachine<States> owner)
		{
			Console.WriteLine("Foxe!");
			owner.ChangeState(States.Idle);
		}
		void StateFoxeExit(StateMachine<States> owner)
		{
			Console.WriteLine("Foxe on exit!");
		}
		void StateFoxeEnter(StateMachine<States> owner)
		{
			Console.WriteLine("Foxe on enter!");
		}

		void StateIdle(StateMachine<States> owner)
		{
			Console.WriteLine("Idle!");
			owner.ChangeState(States.MyBoy);

		}
		void StateIdleExit(StateMachine<States> owner)
		{
			Console.WriteLine("Idle on exit!");
		}


		void StateMyBoy(StateMachine<States> owner)
		{
			Console.WriteLine("My boi!");
			owner.ChangeState(States.Foxe);
		}
		void StateMyBoyEnter(StateMachine<States> owner)
		{
			Console.WriteLine("My boi on enter!");
		}



	}
}
