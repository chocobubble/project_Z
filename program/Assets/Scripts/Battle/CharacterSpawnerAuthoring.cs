using UnityEngine;
using Unity.Entities;
using Data;
using Unity.Mathematics;
using System;

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

			// TODO : 임시이므로 나중에 다른 곳으로 옮겨야 함
			{
				// GO & Transform Entities
				// for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
				// {
				// 	var playerCharacterEntity = GetEntity(TransformUsageFlags.Dynamic);
				// 	AddComponent(playerCharacterEntity, new CharacterPositionIndex { Index = i });
				// }
			}
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
}