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

			CharacterData[] playerCharacterDataList;
			foreach (var (characterData, entity) in SystemAPI.Query<PlayerBattleData>().WithEntityAccess())
			{
				var playerCharacterDataBuffer = state.EntityManager.GetBuffer<PlayerCharacterDataBuffer>(entity); 
				playerCharacterDataList = new CharacterData[playerCharacterDataBuffer.Length];
				for (int i = 0; i < playerCharacterDataBuffer.Length; i++)
				{
					playerCharacterDataList[i] = playerCharacterDataBuffer[i].Value;
				}
				debuggerConfigManaged.CustomDebuggerUIController.UpdateCustomDebugger(playerCharacterDataList);
			}

			// foreach (var (debuggerContent, entity) in SystemAPI.Query<RefRW<DebuggerContent>>().WithEntityAccess())
			// {
			// 	debuggerConfigManaged.CustomDebuggerUIController.UpdateCustomDebugger(debuggerContent.ValueRO.TextKey, debuggerContent.ValueRO.TextValue);
			// 	UnityEngine.Debug.Log($"DebuggerContent: {debuggerContent.ValueRO.TextKey} - {debuggerContent.ValueRO.TextValue}");
			// 	// Remove the DebuggerContent component after updating the UI
			// 	state.EntityManager.RemoveComponent<DebuggerContent>(entity);	
			// }
		}
	}
}
