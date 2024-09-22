using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Battle 
{
	public class CharacterMovementAuthoring : MonoBehaviour
	{
		class Baker : Baker<CharacterMovementAuthoring>
		{
			public override void Bake(CharacterMovementAuthoring authoring)
			{
				// BattleConfig 에 넣자.
			}
		}
	}

	public struct CharacterMovementComponent : IComponentData 
	{
		// public float MoveSpeed;		
		public float3 TargetPosition;
		public bool IsMoving;
	}
}