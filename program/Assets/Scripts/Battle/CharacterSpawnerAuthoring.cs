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
				new CharacterSpawner(
					GetEntity(authoring.CharacterPrefab, TransformUsageFlags.Dynamic)));
		}
	}
}