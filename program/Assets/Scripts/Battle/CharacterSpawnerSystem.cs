using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Collections;

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

			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();
			var spawnerEntity = SystemAPI.GetSingletonEntity<CharacterSpawnerComponent>();
			var spawnerDataComponent = state.EntityManager.GetComponentObject<CharacterSpawnerDataComponent>(spawnerEntity); 

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

			var turnPhaseComponent = SystemAPI.GetSingleton<TurnPhaseComponent>();
			turnPhaseComponent.TurnPhase = TurnPhase.Attack;
			SystemAPI.SetSingleton(turnPhaseComponent);



			// foreach (var (playerBattleData, playerBattleDataEntity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess()) 
			// {
			// 	var playerCharactersData = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
			// 	var playerCharacterPositions = state.EntityManager.GetBuffer<PlayerCharacterPositionBuffer>(playerBattleDataEntity);

			// 	// Spawn Characters
			// 	// For now, we will spawn the pre defined characters
			// 	var instances = new NativeArray<Entity>(BattleConstants.BATTLE_CHARACTER_COUNT, Allocator.Temp);
			// 	state.EntityManager.Instantiate(spawner.CharacterPrefab, instances);
			// 	for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			// 	{
			// 		var characterData = playerCharactersData[i];
			// 		var characterPosition = playerCharacterPositions[i];
			// 		var transform = SystemAPI.GetComponentRW<LocalTransform>(instances[i]);
			// 		transform.ValueRW.Position = characterPosition.Position;
			// 		{
			// 			// var directory = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();
			// 			// var ecb = new EntityCommandBuffer(Allocator.Temp);

			// 			// // Instantiate the associated GameObject from the prefab.
			// 			// foreach (var (goPrefab, entity) in
			// 			// 		SystemAPI.Query<RotationSpeed>()
			// 			// 			.WithNone<RotatorGO>()
			// 			// 			.WithEntityAccess())
			// 			// {
			// 			// 	var go = GameObject.Instantiate(directory.RotatorPrefab);

			// 			// 	// We can't add components to entities as we iterate over them, so we defer the change with an ECB.
			// 			// 	ecb.AddComponent(entity, new RotatorGO(go));
			// 			// }

			// 			// ecb.Playback(state.EntityManager);
			// 			ecb.AddComponent(instances[i], new CharacterPositionIndex { Index = i });
			// 		}
			// 	}
			// }

			// Debug.Log("Enemy Characters Spawn Start");
			// foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
			// {
			// 	var enemyCharactersData = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
			// 	var enemyCharacterPositions = state.EntityManager.GetBuffer<EnemyCharacterPositionBuffer>(enemyBattleDataEntity);

			// 	// Spawn Characters
			// 	// For now, we will spawn the pre defined characters
			// 	var instances = new NativeArray<Entity>(BattleConstants.BATTLE_CHARACTER_COUNT, Allocator.Temp);
			// 	state.EntityManager.Instantiate(spawner.CharacterPrefab, instances);
			// 	for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			// 	{
			// 		var characterData = enemyCharactersData[i];
			// 		var characterPosition = enemyCharacterPositions[i];
			// 		var transform = SystemAPI.GetComponentRW<LocalTransform>(instances[i]);
			// 		transform.ValueRW.Position = characterPosition.Position;
			// 		// Debug.Log("Spawned character at " + characterPosition.Position);
			// 		// Debug.Log("Character Data Id : " + characterData.Value.Id);

			// 		ecb.AddComponent(instances[i], new CharacterPositionIndex { Index = i });
			// 	}
			// }

			// ecb.Playback(state.EntityManager);

			// // Set the spawner status to None
			// spawnerStatus.Status = SpawnerStatus.Complete;
			// SystemAPI.SetSingleton(spawnerStatus);

			// var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
			// battleStateComponent.BattleState = BattleState.Fighting;

		//
			// var battleDataContainer = SystemAPI.GetSingleton<BattleDataContainer>();
			// var characterBundlesToSpawn = BattleDataContainer.Instance.CharacterBundlesToSpawn;
			// if (characterBundlesToSpawn == null || characterBundlesToSpawn.Count == 0) 
			// {
			// 	Debug.Log("CharacterBundlesToSpawn is null or empty");
			// 	spawner.Status = SpawnerStatus.None;
			// 	SystemAPI.SetSingleton(spawner);
			// 	return;
			// }

			// Debug.Log("Character Spawn Start");
			// // Spawn the first character in the list
			// var characterBundle = characterBundlesToSpawn[0];
			// var characterStat = characterBundle.GetCharacterStat();
			// var characterEntity = state.EntityManager.Instantiate(spawner.GetCharacterPrefab());
			// // state.EntityManager.AddComponent(characterEntity, 
			// // 	new HealthComponent { 
			// // 		currentHealth = characterStat.Health, 
			// // 		maxHealth = characterStat.Health });
			// state.EntityManager.AddComponent<HealthComponent>(characterEntity);
			// state.EntityManager.SetComponentData(characterEntity, 
			// 	new HealthComponent { 
			// 		currentHealth = characterStat.Health, 
			// 		maxHealth = characterStat.Health });
			// // state.EntityManager.SetComponentData(characterEntity,
			// // 	new ActGaugeComponent(characterStat.ActSpeed));
			// state.EntityManager.AddComponent<ActGaugeComponent>(characterEntity);
			// state.EntityManager.SetComponentData(characterEntity,
			// 	new ActGaugeComponent(characterStat.ActSpeed));
			// var spawnPosition = spawner.GetNextPlayerCharacterSpawnPosition();
			// var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
			// transform.ValueRW.Position = spawnPosition;
			// Debug.Log("Spawned character at " + spawnPosition);
			// characterBundlesToSpawn.RemoveAt(0);
			// SystemAPI.SetSingleton(spawner);
		}

		private void ProcessSpawner(ref SystemState state, RefRW<CharacterSpawner> spawner)
		{
			
		}
	}
}