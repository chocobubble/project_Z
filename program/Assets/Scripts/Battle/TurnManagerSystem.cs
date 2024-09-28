using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using TMPro;
using Unity.Collections;

namespace Battle
{
	[UpdateAfter(typeof(BattleManagerSystem))]
	public partial struct TurnManagerSystem : ISystem
	{
		private float turnDurationTime;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<TurnManager>();
			state.RequireForUpdate<TurnPhaseComponent>();
			state.RequireForUpdate<BattleConfig>();
			turnDurationTime = 2.0f;
		}

		public void OnDestroy(ref SystemState state)
		{
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!CheckUpdateConditions()) return;

			var battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();
			var turnPhaseComponent = SystemAPI.GetSingletonRW<TurnPhaseComponent>();
			var turnPhase = turnPhaseComponent.ValueRO.TurnPhase;	 
			var turnManager = SystemAPI.GetSingletonRW<TurnManager>();
			turnManager.ValueRW.TurnDurationTime += Time.deltaTime;
			float phaseElapsedTime = turnManager.ValueRW.TurnDurationTime - turnManager.ValueRW.LastTurnPhaseTime;
			
			switch (turnPhase)
			{
				case TurnPhase.None:
					{
						if (phaseElapsedTime >= turnDurationTime) 
						{
							Debug.Log("Set TurnPhase to Spawning from None");
							turnPhaseComponent.ValueRW.TurnPhase = TurnPhase.Spawning;
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;

							// TODO : 임시 코드
							var spawnerComponent = SystemAPI.GetSingletonRW<CharacterSpawnerComponent>();
							spawnerComponent.ValueRW.HasToSpawn = true;

						}
						break;
					}
				case TurnPhase.Spawning:
					{
						if (battleConfig.ValueRO.IsSpawnFinished)
						{
							Debug.Log("Set TurnPhase to PreAttack from Spawning");
							turnPhaseComponent.ValueRW.TurnPhase = TurnPhase.PreAttack;
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;
						}
						break;
					}
				case TurnPhase.PreAttack:
					{
						if (phaseElapsedTime >= turnDurationTime)
						{
							Debug.Log("Set TurnPhase to Attack from PreAttack");
							turnPhaseComponent.ValueRW.TurnPhase = TurnPhase.Attack;
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;
							battleConfig.ValueRW.IsAttackFinished = false;
						}
						break;
					}
				case TurnPhase.Attack:
					{
						if (battleConfig.ValueRO.IsAttackFinished && phaseElapsedTime >= turnDurationTime)
						{
							Debug.Log("Set TurnPhase to PostAttack from Attack");
							turnPhaseComponent.ValueRW.TurnPhase = TurnPhase.PostAttack;
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;
						}
						break;
					}
				case TurnPhase.PostAttack:
					{
						if (battleConfig.ValueRO.ShouldCharactersPositionUpdate)
						{
							Debug.Log("Set TurnPhase to Spawning from PostAttack");
							SetTurnPhase(ref state, TurnPhase.Spawning);
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;
						}
						else 
						{
							Debug.Log("Set TurnPhase to PreAttack from PostAttack");
							turnPhaseComponent.ValueRW.TurnPhase = TurnPhase.PreAttack;
							turnManager.ValueRW.LastTurnPhaseTime = turnManager.ValueRW.TurnDurationTime;
						}
						break;
					}
				default:
					{
						break;
					}
			}
		}

		private void SetTurnPhase(ref SystemState state, TurnPhase turnPhaseToSet)
		{
			var turnPhaseComponent = SystemAPI.GetSingletonRW<TurnPhaseComponent>();
			var currentTurnPhase = turnPhaseComponent.ValueRO.TurnPhase;
			turnPhaseComponent.ValueRW.TurnPhase = turnPhaseToSet;
			switch (turnPhaseToSet)
			{
				case TurnPhase.Spawning:
					{
						var spawnerComponent = SystemAPI.GetSingletonRW<CharacterSpawnerComponent>();
						spawnerComponent.ValueRW.HasToSpawn = true;
						var battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();
						battleConfig.ValueRW.IsSpawnFinished = false;
						break;
					}
				case TurnPhase.PreAttack:
					{
						break;
					}
				case TurnPhase.Attack:
					{
						break;
					}
				case TurnPhase.PostAttack:
					{
						break;
					}
				default:
					{
						break;
					}
			}
			Debug.Log($"Set TurnPhase to {turnPhaseToSet} from {currentTurnPhase}");
		}

		private bool CheckUpdateConditions() 
		{
			var battleState = SystemAPI.GetSingleton<BattleStateComponent>().BattleState;
			if (battleState != BattleState.Start) return false;
			return true;
		}
	}
}