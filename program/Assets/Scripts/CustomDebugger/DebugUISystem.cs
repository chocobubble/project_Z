using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace CustomDebugger
{
	public partial struct DebugUISystem : ISystem 
	{
		// private bool initialized;

		// [BurstCompile]
		// public void OnCreate(ref SystemState state)
		// {
		// 	state.RequireForUpdate<DebuggerConfig>();
		// }

		// public void OnUpdate(ref SystemState state)
		// {
		// 	var debugConfigEntity = SystemAPI.GetSingletonEntity<DebuggerConfig>();
		// 	var debugger = state.EntityManager.GetComponentObject<Debugger>(debugConfigEntity);

		// 	if (!initialized)
		// 	{
		// 		initialized = true;
		// 		debugger.DebugUIController = Object.FindFirstObjectByType<DebugUIController>();
		// 		if (debugger.DebugUIController == null)
		// 		{
		// 			UnityEngine.Debug.LogError("DebugUIController not found in scene");
		// 		}
		// 	}

		// 	foreach (var (debuggerContent, entity) in SystemAPI.Query<RefRW<DebuggerContent>>().WithEntityAccess())
		// 	{
		// 		debugger.DebugUIController.UpdateDebug(debuggerContent.ValueRO.TextKey, debuggerContent.ValueRO.TextValue);
		// 		UnityEngine.Debug.Log($"DebuggerContent: {debuggerContent.ValueRO.TextKey} - {debuggerContent.ValueRO.TextValue}");
		// 		// Remove the DebuggerContent component after updating the UI
		// 		state.EntityManager.RemoveComponent<DebuggerContent>(entity);	
		// 	}
		// }
	}
}
