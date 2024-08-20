using Unity.Entities;
using UnityEngine;
namespace Battle
{
	class BattleSetupAuthoring : MonoBehaviour
	{
	
	}

	class BattleSetupBaker : Baker<BattleSetupAuthoring>
	{
		public override void Bake(BattleSetupAuthoring authoring)
		{
			Debug.Log("Baking BattleSetup");
			var entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, new BattleSetup());
		}
	}
}