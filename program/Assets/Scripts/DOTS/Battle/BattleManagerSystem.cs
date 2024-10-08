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

			// TODO : 임시로 작업한 사항들이므로 나중에 수정해야함
			{
				if (battleState == BattleState.None)
				{
					Debug.Log("Set BattleState to Setup from None");
					SystemAPI.SetSingleton(new BattleStateComponent { BattleState = BattleState.Setup });

					// put characters data on battle
					{
						var entity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
						var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataInBattleComponent>(entity);
						for (int i = 0; i < spawnerDataComponent.PlayerCharacterDataCount || i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
						{
							spawnerDataComponent.PlayerCharacterDataCount++;
							spawnerDataComponent.PlayerCharacterDataListToSpawn[i] = BattleConstants.PLAYER_CHARACTERS_DATA[i];
							spawnerDataComponent.PlayerCharacterPositionListToSpawn[i] = BattleConstants.PLAYER_CHARACTER_POSITIONS[i];
						}
						for (int i = 0; i < spawnerDataComponent.EnemyCharacterDataCount || i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
						{
							spawnerDataComponent.EnemyCharacterDataCount++;
							spawnerDataComponent.EnemyCharacterDataListToSpawn[i] = BattleConstants.ENEMY_CHARACTERS_DATA[i];
							spawnerDataComponent.EnemyCharacterPositionListToSpawn[i] = BattleConstants.ENEMY_CHARACTER_POSITIONS[i];
						}
					}
					// // put character data to spawn
					// {
					// 	var characterSpawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
					// 	var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
					// 	var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity);
					// 	for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
					// 	{
					// 		spawnerDataComponent.CharacterDataCount++;
					// 		spawnerDataComponent.CharacterDataListToSpawn[i] = BattleConstants.PLAYER_CHARACTERS_DATA[i];
					// 		spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.PLAYER_CHARACTER_POSITIONS[i]; 
					// 	}

					// 	for (int i = BattleConstants.PLAYER_CHARACTERS_DATA.Length; i < BattleConstants.ENEMY_CHARACTERS_DATA.Length + BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
					// 	{
					// 		int j = i - BattleConstants.PLAYER_CHARACTERS_DATA.Length;
					// 		spawnerDataComponent.CharacterDataCount++;
					// 		spawnerDataComponent.CharacterDataListToSpawn[i] = BattleConstants.ENEMY_CHARACTERS_DATA[j];
					// 		spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.ENEMY_CHARACTER_POSITIONS[j]; 
					// 	}
					// }
				}
				else if (battleState == BattleState.Setup)
				{
					if (battleConfig.IsBattleSetupFinished == false) return;
					var characterSpawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
					characterSpawnerComponent.HasToSpawn = true;
					SystemAPI.SetSingleton<CharacterSpawnerComponent>(characterSpawnerComponent);
					SystemAPI.SetSingleton(new BattleStateComponent { BattleState = BattleState.Start });
					Debug.Log("Set BattleState to Start from Setup");
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
				var PLAYER_CHARACTERS_DATA = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
				for (int i = 0; i < PLAYER_CHARACTERS_DATA.Length; i++)
				{
					Debug.Log("Character Data Count : " + spawnerDataComponent.CharacterDataCount);
					spawnerDataComponent.CharacterDataCount++;
					spawnerDataComponent.CharacterDataListToSpawn[i] = PLAYER_CHARACTERS_DATA[i].Value;
					spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.PLAYER_CHARACTER_POSITIONS[i];
				}
			}

			foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
			{
				var ENEMY_CHARACTERS_DATA = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
				int j = spawnerDataComponent.CharacterDataCount;
				for (int i = j; i < ENEMY_CHARACTERS_DATA.Length + j; i++)
				{
					Debug.Log("Character Data Count : " + spawnerDataComponent.CharacterDataCount);
					int k = i - j;
					spawnerDataComponent.CharacterDataCount++;
					spawnerDataComponent.CharacterDataListToSpawn[i] = ENEMY_CHARACTERS_DATA[k].Value;
					spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.ENEMY_CHARACTER_POSITIONS[k];
				}
			}
			

			characterSpawnerComponent.HasToSpawn = true;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(characterSpawnerComponent);	
		}
	}
}