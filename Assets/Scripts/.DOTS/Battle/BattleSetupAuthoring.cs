using System.Security.Cryptography;
using Data;
using Unity.Entities;
using UnityEngine;
using BattleUI; 

namespace Battle
{
	class BattleSetupAuthoring : MonoBehaviour
	{
		[Header("Battle Setup UI Prefab")]
		public GameObject BattleSetupUIPrefab;

		class Baker : Baker<BattleSetupAuthoring>
		{
			public override void Bake(BattleSetupAuthoring authoring)
			{
				UnityEngine.Debug.Log("Baking BattleSetup");

				// BattleSetup Entity
				var battleSetupEntity = GetEntity(TransformUsageFlags.None);
				AddComponent(battleSetupEntity, new BattleSetupComponent());
				var battleSetupManaged = new BattleSetupManagedComponent();
				AddComponentObject(battleSetupEntity, battleSetupManaged);
			}
		}
	}
	public struct BattleSetupComponent : IComponentData { }
	public class BattleSetupManagedComponent : IComponentData 
	{
		public BattleSetupUIController BattleSetupUIController;
	}
}