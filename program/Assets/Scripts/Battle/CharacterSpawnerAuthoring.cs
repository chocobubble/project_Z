using UnityEngine;
using Unity.Entities;
using Data;

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
			
			var spawnerStatusEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(spawnerStatusEntity, new CharacterSpawnerStatus { Status = SpawnerStatus.None });

			// GO & Transform Entities
			for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			{
				var playerCharacterEntity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(playerCharacterEntity, new CharacterPositionIndex { Index = i });
			}
		}
	}

	public struct CharacterSpawnerStatus : IComponentData
	{
		public SpawnerStatus Status;
	}
}