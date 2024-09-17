using Unity.Entities;
using UnityEngine;

namespace Battle 
{
	public class CharacterMovementAuthoring : MonoBehaviour
	{
		class Baker : Baker<CharacterMovementAuthoring>
		{
			public override void Bake(CharacterMovementAuthoring authoring)
			{
				
			}
		}
	}

	public struct CharacterMovement : IComponentData 
	{
		
	}
}