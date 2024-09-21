using Unity.Entities;
using Unity.Burst;
using UnityEngine;

namespace Battle 
{
	[BurstCompile]
	public partial struct BattleManagerSystem : ISystem 
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleManager>();
		}

		public void OnDestroy(ref SystemState state) 
		{
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) 
		{
			var battleState = SystemAPI.GetSingleton<BattleStateComponent>().BattleState;
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;

			// TODO : 임시로 작업한 사항들이므로 나중에 수정해야함
			{
				if (battleState == BattleState.None)
				{
					Debug.Log("Set BattleState to Setup from None");
					SystemAPI.SetSingleton(new BattleStateComponent { BattleState = BattleState.Start });
				}
				if (turnPhase == TurnPhase.None)
				{
					Debug.Log("Set TurnPhase to Spawning from None");
					SystemAPI.SetSingleton(new TurnPhaseComponent { TurnPhase = TurnPhase.Spawning });
				}
			}

		}
	}
}