using Unity.Entities;
using UnityEngine;
using Data;

namespace Battle 
{
	public class CharacterActionAuthoring : MonoBehaviour
	{
		class Baker : Baker<CharacterActionAuthoring>
		{
			public override void Bake(CharacterActionAuthoring authoring)
			{
				Debug.Log("Baking CharacterAction");
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new CharacterAction
				{
				});
			}
		}
	}

	public struct CharacterAction : IComponentData
	{
		public CharacterActionState ActionState;
	}
	

	public enum CharacterActionState { None, Idle, MovingToTarget, Attacking, ReturningToPosition, Stunned, Dead, }
}