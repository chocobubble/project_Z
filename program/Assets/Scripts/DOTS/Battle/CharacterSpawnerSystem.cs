using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Collections;
using Unity.Mathematics;
using System.Threading.Tasks;
using Unity.Physics;

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
			state.RequireForUpdate<BattleSetupComponent>();
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

			// Debug.Log("CharacterSpawnerSystem OnUpdate");
			// var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
			// if (battleStateComponent.BattleState == BattleState.Setup)
			// {
				
			// 	battleStateComponent.BattleState = BattleState.Start;
			// 	SystemAPI.SetSingleton<BattleStateComponent>(battleStateComponent);
			// 	return;
			// }	

			var spawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
			if (spawnerComponent.HasToSpawn == false)	
			{
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

			// SpawnCharactersOnBattle(ref state);
			SpawnCharactersOnBattleWithBuffer(ref state);

			// TODO: 이 아래 들은 잘 모르겠음  수정하기!!

			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();
			var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity); 

			var battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();	
			if (battleConfig.ValueRO.ShouldCharactersPositionUpdate)
			{
				Debug.Log("Should Update Characters Position");
				// UpdateCharactersPosition(ref state);
				battleConfig.ValueRW.ShouldCharactersPositionUpdate = false;
				return;
			}


			// var ecb = new EntityCommandBuffer(Allocator.Temp);
			// Debug.Log("Player Characters Spawn Start");
			// for (var i = 0; i < spawnerDataComponent.CharacterDataCount; i++) 
			// {
			// 	Debug.Log("Spawning character at " + spawnerDataComponent.CharacterPositionListToSpawn[i]);
			// 	var characterData = spawnerDataComponent.CharacterDataListToSpawn[i];
			// 	var characterPosition = spawnerDataComponent.CharacterPositionListToSpawn[i];
			// 	var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
			// 	var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
			// 	transform.ValueRW.Position = characterPosition;
			// 	ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
			// 	ecb.AddComponent(characterEntity, new CharacterMovementComponent());
			// }

			// ecb.Playback(state.EntityManager);

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
		private void ProcessSpawner(ref SystemState state, RefRW<CharacterSpawner> spawner)
		{
			
		}

		private void SpawnCharactersOnBattle(ref SystemState state)
		{
			Debug.Log("Spawn Characters On Battle");
			var entity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var characterSpawnerDataInBattleComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataInBattleComponent>(entity);
			var playerCharacterDataListToSpawn = characterSpawnerDataInBattleComponent.PlayerCharacterDataListToSpawn;
			var enemyCharacterDataListToSpawn = characterSpawnerDataInBattleComponent.EnemyCharacterDataListToSpawn;
			var playerCharacterPositionListToSpawn = characterSpawnerDataInBattleComponent.PlayerCharacterPositionListToSpawn;
			var enemyCharacterPositionListToSpawn = characterSpawnerDataInBattleComponent.EnemyCharacterPositionListToSpawn;
			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();

			var ecb = new EntityCommandBuffer(Allocator.Temp);

			// Player Characters Spawn
			for (var i = 0; i < characterSpawnerDataInBattleComponent.PlayerCharacterDataCount; i++) 
			{
				var characterData = playerCharacterDataListToSpawn[i];
				var characterPosition = playerCharacterPositionListToSpawn[i];
				var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
				transform.ValueRW.Position = characterPosition;
				ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
				ecb.AddComponent(characterEntity, new CharacterMovementComponent { IsMoving = false, ReturnPosition = characterPosition });
				ecb.AddComponent(characterEntity, new CharacterOwnershipTypeComponent{ ownershipType = CharacterOwnershipType.Player });
				ecb.AddComponent(characterEntity, new CharacterAction { ActionState = CharacterActionState.None });
			}

			// Enemy Characters Spawn
			for (var i = 0; i < characterSpawnerDataInBattleComponent.EnemyCharacterDataCount; i++) 
			{
				var characterData = enemyCharacterDataListToSpawn[i];
				var characterPosition = enemyCharacterPositionListToSpawn[i];
				var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
				transform.ValueRW.Position = characterPosition;
				ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
				ecb.AddComponent(characterEntity, new CharacterMovementComponent { IsMoving = false, ReturnPosition = characterPosition });
				ecb.AddComponent(characterEntity, new CharacterOwnershipTypeComponent{ ownershipType = CharacterOwnershipType.Enemy });
				ecb.AddComponent(characterEntity, new CharacterAction { ActionState = CharacterActionState.None });
			}
			// for (var i = 0; i < spawnerDataComponent.CharacterDataCount; i++) 
			// {
			// 	Debug.Log("Spawning character at " + spawnerDataComponent.CharacterPositionListToSpawn[i]);
			// 	var characterData = spawnerDataComponent.CharacterDataListToSpawn[i];
			// 	var characterPosition = spawnerDataComponent.CharacterPositionListToSpawn[i];
			// 	var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
			// 	var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
			// 	transform.ValueRW.Position = characterPosition;
			// 	ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
			// 	ecb.AddComponent(characterEntity, new CharacterMovementComponent());
			// }

			ecb.Playback(state.EntityManager);
			
			// Set the spawner status to None
			var spawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
			spawnerComponent.SpawnerState = SpawnerState.Complete;
			spawnerComponent.HasToSpawn = false;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(spawnerComponent);

			// why!?

			var	battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();
			battleConfig.ValueRW.IsSpawnFinished = true;
		}

		private void SpawnCharactersOnBattleWithBuffer(ref SystemState state)
		{
			Debug.Log("Spawn Characters On Battle");
			var entity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var characterSpawnerDataInBattleComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataInBattleComponent>(entity);
			var playerCharacterDataListToSpawn = characterSpawnerDataInBattleComponent.PlayerCharacterDataListToSpawn;
			var enemyCharacterDataListToSpawn = characterSpawnerDataInBattleComponent.EnemyCharacterDataListToSpawn;
			var playerCharacterPositionListToSpawn = characterSpawnerDataInBattleComponent.PlayerCharacterPositionListToSpawn;
			var enemyCharacterPositionListToSpawn = characterSpawnerDataInBattleComponent.EnemyCharacterPositionListToSpawn;
			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();
			
			var playerBattleDataEntity = SystemAPI.GetSingletonEntity<PlayerBattleData>();
			var playerCharacterDataBuffer = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
			var enemyBattleDataEntity = SystemAPI.GetSingletonEntity<EnemyBattleData>();
			var enemyCharacterDataBuffer = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);

			var ecb = new EntityCommandBuffer(Allocator.Temp);

			// Player Characters Spawn
			for (var i = 0; i < playerCharacterDataBuffer.Length; i++) 
			{
				var characterData = playerCharacterDataBuffer[i].Value;
				var characterPosition = playerCharacterPositionListToSpawn[i];
				var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
				transform.ValueRW.Position = characterPosition;
				ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
				ecb.AddComponent(characterEntity, new CharacterMovementComponent { IsMoving = false, ReturnPosition = characterPosition });
				ecb.AddComponent(characterEntity, new CharacterOwnershipTypeComponent{ ownershipType = CharacterOwnershipType.Player });
				ecb.AddComponent(characterEntity, new CharacterAction { ActionState = CharacterActionState.None });
				// ecb.AddComponent(characterEntity, new PhysicsCollider {
				// 	Value = Unity.Physics.BoxCollider.Create(new BoxGeometry {
				// 		Center = new float3(0, 0, 0),
				// 		Size = new float3(1, 1, 1),
				// 		Orientation = quaternion.identity,
				// 		BevelRadius = 0.0f
				// 	})
				// });
			}

			// Enemy Characters Spawn
			for (var i = 0; i < enemyCharacterDataBuffer.Length; i++) 
			{
				var characterData = enemyCharacterDataBuffer[i];
				var characterPosition = enemyCharacterPositionListToSpawn[i];
				var characterEntity = state.EntityManager.Instantiate(spawner.CharacterPrefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
				transform.ValueRW.Position = characterPosition;
				ecb.AddComponent(characterEntity, new CharacterPositionIndex { Index = i });
				ecb.AddComponent(characterEntity, new CharacterMovementComponent { IsMoving = false, ReturnPosition = characterPosition });
				ecb.AddComponent(characterEntity, new CharacterOwnershipTypeComponent{ ownershipType = CharacterOwnershipType.Enemy });
				ecb.AddComponent(characterEntity, new CharacterAction { ActionState = CharacterActionState.None });
			}

			ecb.Playback(state.EntityManager);
			
			// Set the spawner status to None
			var spawnerComponent = SystemAPI.GetSingleton<CharacterSpawnerComponent>();
			spawnerComponent.SpawnerState = SpawnerState.Complete;
			spawnerComponent.HasToSpawn = false;
			SystemAPI.SetSingleton<CharacterSpawnerComponent>(spawnerComponent);

			// why!?

			var	battleConfig = SystemAPI.GetSingletonRW<BattleConfig>();
			battleConfig.ValueRW.IsSpawnFinished = true;
		}
	}
}