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
			testMachine = new StateMachine<States>(States.Foxe, this);
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

		void StateFoxe(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("Foxe!");
			machine.ChangeState(States.Idle);
		}
		void StateFoxeExit(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("Foxe on exit!");
		}
		void StateFoxeEnter(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("Foxe on enter!");
		}

		void StateIdle(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("Idle!");
			machine.ChangeState(States.MyBoy);

		}
		void StateIdleExit(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("Idle on exit!");
		}


		void StateMyBoy(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("My boi!");
			machine.ChangeState(States.Foxe);
		}
		void StateMyBoyEnter(StateMachine<States> machine, Entity owner)
		{
			Console.WriteLine("My boi on enter!");
		}



	}
}
