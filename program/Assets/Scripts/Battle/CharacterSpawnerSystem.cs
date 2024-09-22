using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Collections;
using Unity.Mathematics;

namespace Battle
{
	[BurstCompile]
	public partial struct CharacterSpawnerSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			// This call makes the system not update unless at least one entity in the world exists that has the Spawner component.
			state.RequireForUpdate<CharacterSpawner>();
			state.RequireForUpdate<CharacterSpawnerComponent>();
			state.RequireForUpdate<BattleConfig>();
			state.RequireForUpdate<BattleSetup>();
		}
		public void OnDestroy(ref SystemState state) {}

		// [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.HasSingleton<CharacterSpawnerComponent>())
			{
				Debug.Log("No CharacterSpawner component found");
				return;
			}
			var spawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
			if (spawnerComponent.HasToSpawn == false)	
			{
				return;
			}

			Debug.Log("CharacterSpawnerSystem OnUpdate");
			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();
			var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity); 

			var battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();	
			if (battleConfig.ValueRO.ShouldCharactersPositionUpdate)
			{
				Debug.Log("Should Update Characters Position");
				UpdateCharactersPosition(ref state);
				battleConfig.ValueRW.ShouldCharactersPositionUpdate = false;
				return;
			}

			// delete existing characters
			{
				var ecb2 = new EntityCommandBuffer(Allocator.Temp);
				var entities = state.EntityManager.GetAllEntities();
				foreach (var entity in entities)
				{
					if (state.EntityManager.HasComponent<CharacterPositionIndex>(entity))
					{
						ecb2.DestroyEntity(entity);
					}
				}
				ecb2.Playback(state.EntityManager);
			}

			var ecb = new EntityCommandBuffer(Allocator.Temp);
			Debug.Log("Player Characters Spawn Start");
			for (var i = 0; i < spawnerDataComponent.CharacterDataCount; i++) 
			{
				Debug.Log("Spawning character at " + spawnerDataComponent.CharacterPositionListToSpawn[i]);
				var characterData = spawnerDataComponent.CharacterDataListToSpawn[i];
				var characterPosition = spawnerDataComponent.CharacterPositionListToSpawn[i];
				var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
				transform.ValueRW.Position = characterPosition;
				ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
			}

			ecb.Playback(state.EntityManager);

			// Set the spawner status to None
			spawnerComponent.SpawnerState = SpawnerState.Complete;
			spawnerComponent.HasToSpawn = false;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(spawnerComponent);

			// why!?
			battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();
			battleConfig.ValueRW.IsSpawnFinished = true;
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
					Debug.Log("Character Data Count : " + spawnerDataComponent.CharacterDataCount);
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
					Debug.Log("Character Data Count : " + spawnerDataComponent.CharacterDataCount);
					int k = i - j;
					spawnerDataComponent.CharacterDataCount++;
					spawnerDataComponent.CharacterDataListToSpawn[i] = enemyCharactersData[k].Value;
					spawnerDataComponent.CharacterPositionListToSpawn[i] = BattleConstants.enemyCharacterPositions[k];
				}
			}
			

			characterSpawnerComponent.HasToSpawn = true;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(characterSpawnerComponent);	
		}
		private void ProcessSpawner(ref SystemState state, RefRW<CharacterSpawner> spawner)
		{
			
		}
	}
}