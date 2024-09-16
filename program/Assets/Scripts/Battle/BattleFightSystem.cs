using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	[BurstCompile]
	public partial struct BattleFightSystem : ISystem 
	{
		float currentTurnTime;
		float currentTurn;
		bool isAttackFinished;
		
		[BurstCompile]
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleFight>();

			// 
			{
				currentTurnTime = 0.0f;
				currentTurn = 0;
				isAttackFinished = false;
			}

			Debug.Log("BattleFightSystem Created");
		}

		public void OnDestroy(ref SystemState state) 
		{
			Debug.Log("BattleFightSystem Destroyed");
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) 
		{
			// Debug.Log("Battle Fight Start");

			var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
			var battleState = battleStateComponent.BattleState;
			switch (battleState)
			{
				case BattleState.None:
					// Debug.Log("Battle Fight None");
					break;
				case BattleState.Setup:
					// Debug.Log("Battle Fight Setup");
					break;
				case BattleState.Spawning:
					// Debug.Log("Battle Fight Spawning");
					break;
				case BattleState.Fighting:
					// Debug.Log("Battle Fight Fighting");
					Fight(ref state);
					break;
				case BattleState.Complete:
					// Debug.Log("Battle Fight Complete");
					break;
				default:
					// Debug.Log("Battle Fight Default");
					break;
			}			
		}

		[BurstCompile]
		private void Fight(ref SystemState state)
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
						var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
						battleStateComponent.BattleState = BattleState.Complete;
						SystemAPI.SetSingleton(battleStateComponent);
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
						var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
						battleStateComponent.BattleState = BattleState.Complete;
						SystemAPI.SetSingleton(battleStateComponent);
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