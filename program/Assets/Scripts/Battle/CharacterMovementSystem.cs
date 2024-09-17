using Data;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Battle 
{
	[BurstCompile]
	public partial struct CharacterMovementSystem : ISystem 
	{

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			// state.RequireForUpdate<CharacterMovement>();
			state.RequireForUpdate<CharacterPositionIndex>();
		}

		public void OnDestroy(ref SystemState state) { }
	
	 	[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (var (characterPositionIndex, localTransform, characterEntity) in SystemAPI.Query<RefRO<CharacterPositionIndex>, RefRW<LocalTransform>>().WithEntityAccess())
			{
				int characterPositionIndexValue = characterPositionIndex.ValueRO.Index;
				if (characterPositionIndexValue ==  0)
				{
					// move the entity to the position (0, 0, 0) per frame
					var currentPosition = localTransform.ValueRW.Position;
					var goalPosition = new float3(0, 0, 0);
					var moveSpeed = 0.01f;
					var newPosition = math.lerp(currentPosition, goalPosition, moveSpeed);
					localTransform.ValueRW.Position = newPosition;

				}
			}
			
		}
	}
}