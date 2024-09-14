using Unity.Entities;
using UnityEngine;

namespace Battle
{
	public class BattleFightAuthoring : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject CharacterPrefab;

		class Baker : Baker<BattleFightAuthoring>
		{
			public override void Bake(BattleFightAuthoring authoring)
			{
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new BattleFight
				{
				});
			}
		}
	}

	public struct BattleFight : IComponentData
	{
	}
}