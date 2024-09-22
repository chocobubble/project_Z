using Data;
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
			UnityEngine.Debug.Log("Baking BattleSetup");

			// BattleSetup Entity
			var battleSetupEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(battleSetupEntity, new BattleSetup());
		}
	}
	public struct BattleSetup : IComponentData { }
}