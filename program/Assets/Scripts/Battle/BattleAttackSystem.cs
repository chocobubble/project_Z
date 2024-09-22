using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	[UpdateAfter(typeof(TurnManagerSystem))]
	public partial struct BattleAttackSystem : ISystem 
	{
		float currentTurnTime;
		float currentTurn;
		bool isAttackFinished;
		
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleAttack>();

			// 
			{
				currentTurnTime = 0.0f;
				currentTurn = 0;
				isAttackFinished = false;
			}

			Debug.Log("BattleAttackSystem Created");
		}

		public void OnDestroy(ref SystemState state) 
		{
			Debug.Log("BattleAttackSystem Destroyed");
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) 
		{
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;
			if (turnPhase != TurnPhase.Attack) 
			{
				return;
			}
			Attack(ref state);

		}

		[BurstCompile]
		private void Attack(ref SystemState state)
		{
			var battleConfig = SystemAPI.GetSingleton<BattleConfig>();
			isAttackFinished = battleConfig.IsAttackFinished;

			if (isAttackFinished == false) 
			{
				Debug.Log("Attacking");
				PlayerCharacterDataBuffer playerCharacterDataBuffer = default;
				EnemyCharacterDataBuffer enemyCharacterDataBuffer = default;
				DynamicBuffer<PlayerCharacterDataBuffer> PLAYER_CHARACTERS_DATA = default;
				DynamicBuffer<EnemyCharacterDataBuffer> ENEMY_CHARACTERS_DATA = default;
				foreach (var (playerBattleData, playerBattleDataEntity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess()) 
				{
					PLAYER_CHARACTERS_DATA = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
					
					// TODO: 이 체크 로직은 나중에 다른 곳으로 빼주자
					if (PLAYER_CHARACTERS_DATA.Length == 0) 
					{
						Debug.Log("Player Characters Data is empty. Battle is over");
						var battleStateComponent = SystemAPI.GetSingletonRW<BattleStateComponent>();
						battleStateComponent.ValueRW.BattleState = BattleState.End;
						SystemAPI.SetSingleton<TurnPhaseComponent>(new TurnPhaseComponent { TurnPhase = TurnPhase.None });
						return;
					}
					playerCharacterDataBuffer = PLAYER_CHARACTERS_DATA[0];
				}

				foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
				{
					ENEMY_CHARACTERS_DATA = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
					// TODO: 이 체크 로직은 나중에 다른 곳으로 빼주자
					if (ENEMY_CHARACTERS_DATA.Length == 0) 
					{
						Debug.Log("Player Characters Data is empty. Battle is over");
						var battleStateComponent = SystemAPI.GetSingletonRW<BattleStateComponent>();
						battleStateComponent.ValueRW.BattleState = BattleState.End;
						SystemAPI.SetSingleton<TurnPhaseComponent>(new TurnPhaseComponent { TurnPhase = TurnPhase.None });
						return;
					}
					enemyCharacterDataBuffer = ENEMY_CHARACTERS_DATA[0];
				}
				CharacterData playerCharacterData = playerCharacterDataBuffer.Value; 
				CharacterData enemyCharacterData = enemyCharacterDataBuffer.Value; 

				if (playerCharacterData.Id == 0 || enemyCharacterData.Id == 0) 
				{
					Debug.LogError("Player or Enemy Character Id is 0");
					return;
				}
				else 
				{
					Debug.Log($"Player Character {playerCharacterData.Id} attacked Enemy Character {enemyCharacterData.Id}");
					enemyCharacterData.MaxHP -= playerCharacterData.Attack;
					Debug.Log($"Enemy Character {enemyCharacterData.Id} HP : {enemyCharacterData.MaxHP}");
					Debug.Log($"Enemy Character {enemyCharacterData.Id} attacked Player Character {playerCharacterData.Id}");
					playerCharacterData.MaxHP -= enemyCharacterData.Attack;
					Debug.Log($"Player Character {playerCharacterData.Id} HP : {playerCharacterData.MaxHP}");

					playerCharacterDataBuffer.Value = playerCharacterData;	
					enemyCharacterDataBuffer.Value = enemyCharacterData;	

					PLAYER_CHARACTERS_DATA[0] = playerCharacterDataBuffer;
					ENEMY_CHARACTERS_DATA[0] = enemyCharacterDataBuffer;

					bool shouldCharactersPositionUpdate = false;
					if (playerCharacterData.MaxHP <= 0) 
					{
						Debug.Log($"Player Character {playerCharacterData.Id} is dead");
						PLAYER_CHARACTERS_DATA.RemoveAt(0);
						shouldCharactersPositionUpdate = true;
					}
					if (enemyCharacterData.MaxHP <= 0) 
					{
						Debug.Log($"Enemy Character {enemyCharacterData.Id} is dead");
						ENEMY_CHARACTERS_DATA.RemoveAt(0);
						shouldCharactersPositionUpdate = true;
					}
					if (shouldCharactersPositionUpdate) 
					{
						battleConfig.ShouldCharactersPositionUpdate = true;
					}
				}
				battleConfig.IsAttackFinished = true;
				SystemAPI.SetSingleton<BattleConfig>(battleConfig);

			}
		}
	}
}