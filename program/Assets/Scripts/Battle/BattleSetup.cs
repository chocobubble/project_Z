using Unity.Entities;
using UnityEngine;
using PlayerData;
using Unity.Burst;

namespace Battle 
{
	/// <summary>
	/// Battle State 가 Setup 상태일 때, 작동하는 시스템
	/// </summary>
	[BurstCompile]
	public partial struct BattleSetupSystem : ISystem
	{
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleSetup>();
			state.RequireForUpdate<BattleState>();
		}
		public void OnDestroy(ref SystemState state) {}

		// [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.HasSingleton<BattleState>() == false) 
			{
				Debug.LogError("BattleSetup component is null");
				return;
			}
			var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
			if (battleStateComponent.BattleState != BattleState.Setup) 
			{
				return;
			} 

			// TODO : 아래 작업 마무리 못했음 
			Debug.Log("Battle Setup Start");
			if (SystemAPI.HasSingleton<CharacterSpawner>() == false)
			{
				Debug.LogError("CharacterSpawner component is null");
				return;
			}
			var characterSpawnerStatus = SystemAPI.GetSingleton<CharacterSpawnerStatus>();
			characterSpawnerStatus.Status = SpawnerStatus.Spawning;
			SystemAPI.SetSingleton<CharacterSpawnerStatus>(characterSpawnerStatus);
			
			// battleSetup.BattleSetupStatus = BattleSetupStatus.Initializing;
			// SystemAPI.SetSingleton<BattleSetup>(battleSetup);
			Debug.Log("BattleSetupSystem OnUpdate");
		}
	}
}