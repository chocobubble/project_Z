using Unity.Entities;
using UnityEngine;

namespace Battle
{
	public class BattleAttackAuthoring : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject CharacterPrefab;

		class Baker : Baker<BattleAttackAuthoring>
		{
			public override void Bake(BattleAttackAuthoring authoring)
			{
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new BattleAttack
				{
				});
			}
		}
	}

	public struct BattleAttack : IComponentData
	{
	}
}