using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	[BurstCompile]
	public partial struct BattleManagerSystem : ISystem 
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleManager>();
			state.RequireForUpdate<BattleStateComponent>();
			state.RequireForUpdate<TurnPhaseComponent>();	
		}

		public void OnDestroy(ref SystemState state) 
		{
		}

		// [BurstCompile]
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

					// put character data to spawn
					{
						var characterSpawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
						var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
						var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity);
						for (int i = 0; i < BattleConstants.playerCharactersData.Length; i++)
						{
							spawnerDataComponent.CharacterDataCount++;
							spawnerDataComponent.CharacterDataListToSpawn[i] = BattleConstants.playerCharactersData[i];
							spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.playerCharacterPositions[i]; 
						}

						for (int i = BattleConstants.playerCharactersData.Length; i < BattleConstants.enemyCharactersData.Length + BattleConstants.playerCharactersData.Length; i++)
						{
							int j = i - BattleConstants.playerCharactersData.Length;
							spawnerDataComponent.CharacterDataCount++;
							spawnerDataComponent.CharacterDataListToSpawn[i] = BattleConstants.enemyCharactersData[j];
							spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.enemyCharacterPositions[j]; 
						}

						characterSpawnerComponent.HasToSpawn = true;
						SystemAPI.SetSingleton<CharacterSpawnerComponent>(characterSpawnerComponent);
					}
				}
			}

		}
	}
}