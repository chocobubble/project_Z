using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using System.Net.Http.Headers;
using Unity.Mathematics;
using Data;

namespace Battle 
{
	[UpdateAfter(typeof(TurnManagerSystem))]
	public partial struct CharacterActionSystem : ISystem 
	{
		// TODO: Temporal code
		public float attackingDuration; 
		public float elapsedAttackingTime;

		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<CharacterAction>();
			attackingDuration = 1.0f;	
			elapsedAttackingTime = 0.0f;	
			Debug.Log("CharacterActionSystem Created");
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) 
		{
			Debug.Log("CharacterActionSystem Destroyed");
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) 
		{
			foreach (var (characterAction, characterEntity) in SystemAPI.Query<CharacterAction>().WithEntityAccess()) 
			{
				switch (characterAction.ActionState) 
				{
					case CharacterActionState.None:
						None(ref state, characterEntity);
						break;
					case CharacterActionState.Idle:
						Idle(ref state, characterEntity);
						break;
					case CharacterActionState.MovingToTarget:
						MovingToTarget(ref state, characterEntity);
						break;
					case CharacterActionState.Attacking:
						Attacking(ref state, characterEntity);
						break;
					case CharacterActionState.ReturningToPosition:
						ReturningToPosition(ref state, characterEntity);
						break;
					case CharacterActionState.Stunned:
						Stunned(ref state, characterEntity);
						break;
					case CharacterActionState.Dead:
						Dead(ref state, characterEntity);
						break;
				}
			}
		}

		[BurstCompile]
		private void None(ref SystemState state, Entity characterEntity) 
		{
			// TODO : Delete below code
			var characterAction = state.EntityManager.GetComponentData<CharacterAction>(characterEntity);
			characterAction.ActionState = CharacterActionState.Idle;
			state.EntityManager.SetComponentData(characterEntity, characterAction);
			Debug.Log("Character Action State is set to Idle"); 
		}

		[BurstCompile]
		private void Idle(ref SystemState state, Entity characterEntity) 
		{
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;
			if (turnPhase != TurnPhase.Attack) return;
			
			var characterPostionIndex = state.EntityManager.GetComponentData<CharacterPositionIndex>(characterEntity);
			if (characterPostionIndex.Index != 0) return;

			var characterAction = state.EntityManager.GetComponentData<CharacterAction>(characterEntity);
			characterAction.ActionState = CharacterActionState.MovingToTarget;
			state.EntityManager.SetComponentData(characterEntity, characterAction);

			var characterMovementComponent = state.EntityManager.GetComponentData<CharacterMovementComponent>(characterEntity);
			characterMovementComponent.IsMoving = true;
			characterMovementComponent.TargetPosition = new float3(0, 0, 0);
			state.EntityManager.SetComponentData(characterEntity, characterMovementComponent);
			Debug.Log("Character Action State is set to MovingToTarget");
		}

		[BurstCompile]
		private void MovingToTarget(ref SystemState state, Entity characterEntity) 
		{
			// if entity's charactermovementcomponent is not moving, set characteraction.actionstate to idle
			var characterMovementComponent = state.EntityManager.GetComponentData<CharacterMovementComponent>(characterEntity);
			if (characterMovementComponent.IsMoving) return;

			var characterAction = state.EntityManager.GetComponentData<CharacterAction>(characterEntity);
			characterAction.ActionState = CharacterActionState.Attacking;
			state.EntityManager.SetComponentData(characterEntity, characterAction);
			Debug.Log("Character Action State is set to Attacking");
		}

		[BurstCompile]
		private void Attacking(ref SystemState state, Entity characterEntity) 
		{
			// elapsedAttackingTime += SystemAPI.Time.DeltaTime;
			// if (elapsedAttackingTime < attackingDuration) return;

			// elapsedAttackingTime = 0.0f;
			var characterMovementComponent = state.EntityManager.GetComponentData<CharacterMovementComponent>(characterEntity);
			var characterOwnershipType = state.EntityManager.GetComponentData<CharacterOwnershipTypeComponent>(characterEntity);
			characterMovementComponent.TargetPosition = characterMovementComponent.ReturnPosition;
			characterMovementComponent.IsMoving = true;
			state.EntityManager.SetComponentData(characterEntity, characterMovementComponent);	

			var characterAction = state.EntityManager.GetComponentData<CharacterAction>(characterEntity);
			characterAction.ActionState = CharacterActionState.ReturningToPosition;
			state.EntityManager.SetComponentData(characterEntity, characterAction);
			Debug.Log("Character Action State is set to ReturningToPosition");
		}

		[BurstCompile]
		private void ReturningToPosition(ref SystemState state, Entity characterEntity) 
		{
			var characterMovementComponent = state.EntityManager.GetComponentData<CharacterMovementComponent>(characterEntity);
			if (characterMovementComponent.IsMoving) return;

			// TODO: Temporal code
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;
			if (turnPhase == TurnPhase.Attack) return;

			var characterAction = state.EntityManager.GetComponentData<CharacterAction>(characterEntity);
			characterAction.ActionState = CharacterActionState.Idle;
			state.EntityManager.SetComponentData(characterEntity, characterAction);
			Debug.Log("Character Action State is set to Idle");	
		}

		[BurstCompile]
		private void Stunned(ref SystemState state, Entity characterEntity) 
		{
		}

		[BurstCompile]
		private void Dead(ref SystemState state, Entity characterEntity) 
		{
		}

	}
}