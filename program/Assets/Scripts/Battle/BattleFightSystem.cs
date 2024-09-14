using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Data;

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

				CharacterData playerCharacterData = default; 
				CharacterData enemyCharacterData = default; 
				foreach (var (playerBattleData, playerBattleDataEntity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess()) 
				{
					var playerCharactersData = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(playerBattleDataEntity);
					playerCharacterData = playerCharactersData[0].Value;
				}

				foreach (var (enemyBattleData, enemyBattleDataEntity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess()) 
				{
					var enemyCharactersData = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(enemyBattleDataEntity);
					enemyCharacterData = enemyCharactersData[0].Value;
				}

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
				}
			}


		}

	}
}