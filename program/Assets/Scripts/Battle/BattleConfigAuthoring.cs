using Unity.Entities;
using UnityEngine;

namespace Battle
{
	public class BattleConfigAuthoring : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject CharacterPrefab;

		class Baker : Baker<BattleConfigAuthoring>
		{
			public override void Bake(BattleConfigAuthoring authoring)
			{
				Debug.Log("Baking BattleConfig");
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new BattleConfig
				{
					CharacterPrefab = GetEntity(authoring.CharacterPrefab, TransformUsageFlags.None),
					TurnCount = 0,
					TurnDurationSeconds = 3.0f
				});
			}
		}
	}

	public struct BattleConfig : IComponentData
	{
		public Entity CharacterPrefab;
		public int TurnCount;
		public float TurnDurationSeconds;
	}
}