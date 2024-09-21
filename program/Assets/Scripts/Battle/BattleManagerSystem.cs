using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;

namespace Battle 
{
	[BurstCompile]
	public partial struct BattleManagerSystem : ISystem 
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleConfig>();
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
			var battleConfig = SystemAPI.GetSingleton<BattleConfig>();
			var battleState = SystemAPI.GetSingleton<BattleStateComponent>().BattleState;
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;

			if (battleConfig.ShouldCharactersPositionUpdate)
			{
				turnPhase = TurnPhase.Spawning;
				SystemAPI.SetSingleton(new TurnPhaseComponent { TurnPhase = TurnPhase.Spawning });
				UpdateCharactersPosition(ref state);
				battleConfig.ShouldCharactersPositionUpdate = false;
				SystemAPI.SetSingleton<BattleConfig>(battleConfig);
				return;
			}
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

		private void UpdateCharactersPosition(ref SystemState state)
		{
			var characterSpawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
			var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity);
			
			spawnerDataComponent.CharacterDataCount = 0;
			spawnerDataComponent.CharacterDataListToSpawn = new CharacterData[SpawnerConstants.MAX_SPAWN_DATA_COUNT];
			spawnerDataComponent.CharacterPositionListToSpawn = new float3[SpawnerConstants.MAX_SPAWN_DATA_COUNT];
			
			foreach (var (playerBattleData, playerBattleDataEntity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess()) 
			{
				var playerCharactersData = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
				for (int i = 0; i < playerCharactersData.Length; i++)
				{
					spawnerDataComponent.CharacterDataCount++;
					spawnerDataComponent.CharacterDataListToSpawn[i] = playerCharactersData[i].Value;
					spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.playerCharacterPositions[i];
				}
			}

			foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
			{
				var enemyCharactersData = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
				int j = spawnerDataComponent.CharacterDataCount;
				for (int i = j; i < enemyCharactersData.Length + j; i++)
				{
					int k = i - j;
					spawnerDataComponent.CharacterDataCount++;
					spawnerDataComponent.CharacterDataListToSpawn[i] = enemyCharactersData[k].Value;
					spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.enemyCharacterPositions[k];
				}
			}
			

			characterSpawnerComponent.HasToSpawn = true;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(characterSpawnerComponent);	
		}
	}
}