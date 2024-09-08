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
		}
	}

	public struct CharacterSpawnerStatus : IComponentData
	{
		public SpawnerStatus Status;
	}
}