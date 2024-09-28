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
			state.RequireForUpdate<CharacterMovementComponent>();
			state.RequireForUpdate<CharacterPositionIndex>();
		}

		public void OnDestroy(ref SystemState state) { }
	
	 	[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var battleConfig = SystemAPI.GetSingleton<BattleConfig>();
			var characterMovementSpeed = battleConfig.CharacterMovementSpeed;

			foreach (var (characterPositionIndex, characterMovementComponent, localTransform, characterEntity) in SystemAPI.Query<RefRO<CharacterPositionIndex>, RefRW<CharacterMovementComponent>, RefRW<LocalTransform>>().WithEntityAccess())
			{
				if (characterMovementComponent.ValueRO.IsMoving == false) continue;

				var currentPosition = localTransform.ValueRW.Position;
				var targetPosition = characterMovementComponent.ValueRO.TargetPosition;
				var newPosition = math.lerp(currentPosition, targetPosition, characterMovementSpeed);
				localTransform.ValueRW.Position = newPosition;
				if (math.distance(newPosition, targetPosition) < 0.1f)
				{
					characterMovementComponent.ValueRW.IsMoving = false;
					Debug.Log("CharacterMovementSystem: Character has arrived at the target position");
				}
			}

			// foreach (var (characterPositionIndex, localTransform, characterEntity) in SystemAPI.Query<RefRO<CharacterPositionIndex>, RefRW<LocalTransform>>().WithEntityAccess())
			// {
			// 	int characterPositionIndexValue = characterPositionIndex.ValueRO.Index;
			// 	if (characterPositionIndexValue ==  0)
			// 	{
			// 		// move the entity to the position (0, 0, 0) per frame
			// 		var currentPosition = localTransform.ValueRW.Position;
			// 		var goalPosition = new float3(0, 0, 0);
			// 		var moveSpeed = 0.01f;
			// 		var newPosition = math.lerp(currentPosition, goalPosition, moveSpeed);
			// 		localTransform.ValueRW.Position = newPosition;
			// 	}
			// }
			
			
		}
	}
}