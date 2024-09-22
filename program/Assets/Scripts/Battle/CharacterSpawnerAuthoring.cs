using UnityEngine;
using Unity.Entities;
using Data;
using Unity.Mathematics;
using System;
using Unity.Collections;

namespace Battle
{
	class CharacterSpawnerAuthoring : MonoBehaviour
	{
		public GameObject CharacterPrefab;
	}

	class CharacterSpawnerBaker : Baker<CharacterSpawnerAuthoring>
	{
		public override void Bake(CharacterSpawnerAuthoring authoring)
		{
			Debug.Log("Baking CharacterSpawner");
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, 
				new CharacterSpawner 
				{
					CharacterPrefab = GetEntity(authoring.CharacterPrefab, TransformUsageFlags.Dynamic)
				});
			
			var spawnerEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(spawnerEntity, 
				new CharacterSpawnerComponent 
				{ 
					SpawnerState = SpawnerState.None,
					HasToSpawn = false
				});
			// var spawnerDataEntity = GetEntity(TransformUsageFlags.None);
			AddComponentObject(spawnerEntity,
				new CharacterSpawnerDataComponent()
				{
					CharacterDataCount = 0,
					CharacterDataListToSpawn = new CharacterData[SpawnerConstants.MAX_SPAWN_DATA_COUNT],
					CharacterPositionListToSpawn = new float3[SpawnerConstants.MAX_SPAWN_DATA_COUNT]
				}
				
				);
			AddComponentObject(spawnerEntity,
				new CharacterSpawnerDataInBattleComponent()
				{
					PlayerCharacterDataCount = 0,
					EnemyCharacterDataCount = 0,
					PlayerCharacterDataListToSpawn = new CharacterData[SpawnerConstants.MAX_SPAWN_DATA_COUNT],
					EnemyCharacterDataListToSpawn = new CharacterData[SpawnerConstants.MAX_SPAWN_DATA_COUNT],
					PlayerCharacterPositionListToSpawn = new float3[SpawnerConstants.MAX_SPAWN_DATA_COUNT],
					EnemyCharacterPositionListToSpawn = new float3[SpawnerConstants.MAX_SPAWN_DATA_COUNT]
				}
			);
		}
	}

	public enum SpawnerState
	{
		None,
		Waiting,
		Spawning,
		Complete,
	}

	public struct CharacterSpawnerComponent : IComponentData
	{
		public SpawnerState SpawnerState;
		public bool HasToSpawn;
		// public DynamicBuffer<CharacterData> characterDataListToSpawn;
		// public DynamicBuffer<float3> characterPositionListToSpawn;
	}

	public class CharacterSpawnerDataComponent : IComponentData
	{
		public int CharacterDataCount;
		public CharacterData[] CharacterDataListToSpawn;
		public float3[] CharacterPositionListToSpawn;
	}

	public class CharacterSpawnerDataInBattleComponent : IComponentData
	{
		public int PlayerCharacterDataCount;
		public int EnemyCharacterDataCount;
		public CharacterData[] PlayerCharacterDataListToSpawn;
		public CharacterData[] EnemyCharacterDataListToSpawn;
		public float3[] PlayerCharacterPositionListToSpawn;
		public float3[] EnemyCharacterPositionListToSpawn;

	}
}