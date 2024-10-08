using Unity.Entities;
using UnityEngine;

namespace Battle
{
	public class TurnManagerAuthoring : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject CharacterPrefab;

		class Baker : Baker<TurnManagerAuthoring>
		{
			public override void Bake(TurnManagerAuthoring authoring)
			{
				Debug.Log("Baking TurnManager");
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new TurnManager
				{
					TurnDurationTime = 0.0f,
					LastTurnPhaseTime = 0.0f
				});
			}
		}
	}

	public struct TurnManager : IComponentData
	{
		public float TurnDurationTime;
		public float LastTurnPhaseTime;
	}
}