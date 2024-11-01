using Battle;
using Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CustomDebugger
{
	public partial struct CustomDebuggerUISystem : ISystem 
	{
		private bool initialized;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<CustomDebuggerConfigManaged>();
			state.RequireForUpdate<BattleConfig>();
			state.RequireForUpdate<BattleStateComponent>();
			state.RequireForUpdate<TurnPhaseComponent>();
			state.RequireForUpdate<TurnManager>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var debugConfigEntity = SystemAPI.GetSingletonEntity<CustomDebuggerConfig>();
			var debuggerConfigManaged = state.EntityManager.GetComponentObject<CustomDebuggerConfigManaged>(debugConfigEntity);

			if (!initialized)
			{
				initialized = true;
				debuggerConfigManaged.CustomDebuggerUIController = Object.FindFirstObjectByType<CustomDebuggerUIController>();
				if (debuggerConfigManaged.CustomDebuggerUIController == null)
				{
					UnityEngine.Debug.LogError("DebugUIController not found in scene");
				}
				Debug.Log("CustomDebuggerUISystem Initialized");
			}

			CharacterData[] playerCharacterDataList = null;
			CharacterData[] enemyCharacterDataList = null;
			foreach (var (characterData, entity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess())
			{
				var playerCharacterDataBuffer = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(entity); 
				playerCharacterDataList = new CharacterData[playerCharacterDataBuffer.Length];
				for (int i = 0; i < playerCharacterDataBuffer.Length; i++)
				{
					playerCharacterDataList[i] = playerCharacterDataBuffer[i].Value;
				}
			}
			foreach (var (characterData, entity) in SystemAPI.Query<EnemyBattleData>().WithEntityAccess())
			{
				var enemyCharacterDataBuffer = state.EntityManager.GetBuffer<EnemyCharacterDataBuffer>(entity);
				enemyCharacterDataList = new CharacterData[enemyCharacterDataBuffer.Length];
				for (int i = 0; i < enemyCharacterDataBuffer.Length; i++)
				{
					enemyCharacterDataList[i] = enemyCharacterDataBuffer[i].Value;
				}
			}

			if (playerCharacterDataList == null || enemyCharacterDataList == null)
			{
				// Debug.LogError("PlayerCharacterDataList or EnemyCharacterDataList is null");
				// return;
			}
			else 
			{
				debuggerConfigManaged.CustomDebuggerUIController.UpdateCustomDebugger(playerCharacterDataList, enemyCharacterDataList);
			}
		
			var battleState = SystemAPI.GetSingleton<BattleStateComponent>().BattleState;
			var turnPhase = SystemAPI.GetSingleton<TurnPhaseComponent>().TurnPhase;	
			debuggerConfigManaged.CustomDebuggerUIController.UpdateCustomDebugger(battleState, turnPhase);

			var turnManager = SystemAPI.GetSingleton<TurnManager>();
			debuggerConfigManaged.CustomDebuggerUIController.UpdateTurnTimerDebugger(turnManager.TurnDurationTime, turnManager.LastTurnPhaseTime);
		}
	}
}
