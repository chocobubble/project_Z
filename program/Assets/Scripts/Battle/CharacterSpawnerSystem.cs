using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;

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
		}
		public void OnDestroy(ref SystemState state) {}

		// [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.HasSingleton<CharacterSpawner>())
			{
				Debug.Log("No CharacterSpawner component found");
				return;
			}
			var spawner = SystemAPI.GetSingleton<CharacterSpawner>();
			
			if (spawner.Status != SpawnerStatus.Spawning) 
			{
				// Debug.Log("Spawner status is not Spawning");
				return;
			}
			
			var characterBundlesToSpawn = BattleDataContainer.Instance.CharacterBundlesToSpawn;
			if (characterBundlesToSpawn == null || characterBundlesToSpawn.Count == 0) 
			{
				Debug.Log("CharacterBundlesToSpawn is null or empty");
				spawner.Status = SpawnerStatus.None;
				SystemAPI.SetSingleton(spawner);
				return;
			}

			Debug.Log("Character Spawn Start");
			// Spawn the first character in the list
			var characterBundle = characterBundlesToSpawn[0];
			var characterStat = characterBundle.GetCharacterStat();
			var characterEntity = state.EntityManager.Instantiate(spawner.GetCharacterPrefab());
			// state.EntityManager.AddComponent(characterEntity, 
			// 	new HealthComponent { 
			// 		currentHealth = characterStat.Health, 
			// 		maxHealth = characterStat.Health });
			state.EntityManager.AddComponent<HealthComponent>(characterEntity);
			state.EntityManager.SetComponentData(characterEntity, 
				new HealthComponent { 
					currentHealth = characterStat.Health, 
					maxHealth = characterStat.Health });
			// state.EntityManager.SetComponentData(characterEntity,
			// 	new ActGaugeComponent(characterStat.ActSpeed));
			state.EntityManager.AddComponent<ActGaugeComponent>(characterEntity);
			state.EntityManager.SetComponentData(characterEntity,
				new ActGaugeComponent(characterStat.ActSpeed));
			var spawnPosition = spawner.GetNextPlayerCharacterSpawnPosition();
			var transform = SystemAPI.GetComponentRW<LocalTransform>(characterEntity);
			transform.ValueRW.Position = spawnPosition;
			Debug.Log("Spawned character at " + spawnPosition);
			characterBundlesToSpawn.RemoveAt(0);
			SystemAPI.SetSingleton(spawner);
		}

		private void ProcessSpawner(ref SystemState state, RefRW<CharacterSpawner> spawner)
		{
			
		}
	}
}