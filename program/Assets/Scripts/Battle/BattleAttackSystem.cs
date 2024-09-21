using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	[BurstCompile]
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
			// Debug.Log("Fighting");
			var BattleConfig = SystemAPI.GetSingleton<BattleConfig>();
			currentTurnTime += Time.deltaTime;
			var turnDuration = BattleConfig.TurnDurationSeconds;
			if (currentTurnTime >= turnDuration) 
			{
				currentTurn++;
				currentTurnTime = 0.0f;
				isAttackFinished = false;
				Debug.Log($"Turn {currentTurn} complete");
			}
			
			if (isAttackFinished == false) 
			{
				Debug.Log("Attacking");
				// Attack
				isAttackFinished = true;

				PlayerCharacterDataBuffer playerCharacterDataBuffer = default;
				EnemyCharacterDataBuffer enemyCharacterDataBuffer = default;
				DynamicBuffer<PlayerCharacterDataBuffer> playerCharactersData = default;
				DynamicBuffer<EnemyCharacterDataBuffer> enemyCharactersData = default;
				foreach (var (playerBattleData, playerBattleDataEntity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess()) 
				{
					playerCharactersData = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
					
					// TODO: 이 체크 로직은 나중에 다른 곳으로 빼주자
					if (playerCharactersData.Length == 0) 
					{
						Debug.Log("Player Characters Data is empty");
						var turnPhaseComponent = SystemAPI.GetSingleton<turnPhaseComponent>();
						turnPhaseComponent.TurnPhase = TurnPhase.End;
						SystemAPI.SetSingleton(turnPhaseComponent);
						return;
					}
					playerCharacterDataBuffer = playerCharactersData[0];
				}

				foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
				{
					enemyCharactersData = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
					// TODO: 이 체크 로직은 나중에 다른 곳으로 빼주자
					if (enemyCharactersData.Length == 0) 
					{
						Debug.Log("Player Characters Data is empty");
						var turnPhaseComponent = SystemAPI.GetSingleton<turnPhaseComponent>();
						turnPhaseComponent.TurnPhase = TurnPhase.End;
						SystemAPI.SetSingleton(turnPhaseComponent);
						return;
					}
					enemyCharacterDataBuffer = enemyCharactersData[0];
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

					playerCharactersData[0] = playerCharacterDataBuffer;
					enemyCharactersData[0] = enemyCharacterDataBuffer;

					if (playerCharacterData.MaxHP <= 0) 
					{
						Debug.Log($"Player Character {playerCharacterData.Id} is dead");
						playerCharactersData.RemoveAt(0);
					}
					if (enemyCharacterData.MaxHP <= 0) 
					{
						Debug.Log($"Enemy Character {enemyCharacterData.Id} is dead");
						enemyCharactersData.RemoveAt(0);
					}
				}
			}
		}
	}
}