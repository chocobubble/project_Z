using Unity.Entities;
using UnityEngine;
using PlayerData;
using Unity.Burst;

namespace Battle 
{
	public enum BattleSetupStatus
	{
		None,
		Initializing,
		Complete,
	}

	public enum BattleState 
	{
		None,
		Setup,
		Spawning,
		Fighting,
		Complete,
	}

	// Component to store battle setup data
	public struct BattleSetup : IComponentData
	{
		private BattleSetupStatus battleSetupStatus;
		public BattleSetupStatus BattleSetupStatus
		{
			get {
				// Debug.Log("Getting BattleSetupStatus as " + battleSetupStatus);
				return battleSetupStatus;
			}
			set {
				Debug.Log("Setting BattleSetupStatus to " + value);
				battleSetupStatus = value;
			}
		}
	}

	public struct BattleStateComponent : IComponentData
	{
		private BattleState battleState;
		public BattleState BattleState
		{
			get {
				// Debug.Log("Getting BattleSetupStatus as " + battleSetupStatus);
				return battleState;
			}
			set {
				Debug.Log("Setting BattleState to " + value);
				battleState = value;
			}
		}
	}

	[BurstCompile]
	public partial struct BattleSetupSystem : ISystem
	{
		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleSetup>();
		}
		public void OnDestroy(ref SystemState state) {}

		// [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.HasSingleton<BattleSetup>() == false) 
			{
				Debug.LogError("BattleSetup component is null");
				return;
			}
			var battleSetup = SystemAPI.GetSingleton<BattleSetup>();
			if (battleSetup.BattleSetupStatus == BattleSetupStatus.None) 
			{
				if (SystemAPI.HasSingleton<CharacterSpawner>() == false)
				{
					Debug.LogError("CharacterSpawner component is null");
					return;
				}
				Debug.Log("Battle Setup Start");
				var characterSpawnerStatus = SystemAPI.GetSingleton<CharacterSpawnerStatus>();
				characterSpawnerStatus.Status = SpawnerStatus.Spawning;
				SystemAPI.SetSingleton<CharacterSpawnerStatus>(characterSpawnerStatus);
				// foreach (var characterBundle in PlayerDataContainer.Instance.GetCharacterBundles())
				// {
				// 	BattleDataContainer.Instance.CharacterBundlesToSpawn.Add(characterBundle);
				// }
				
				battleSetup.BattleSetupStatus = BattleSetupStatus.Initializing;
				SystemAPI.SetSingleton<BattleSetup>(battleSetup);
				Debug.Log("BattleSetupSystem OnUpdate");
			}
			else if (battleSetup.BattleSetupStatus == BattleSetupStatus.Initializing)
			{
				Debug.Log("Battle Setup Initializing");
				// Initialize the battle setup
				// For now, we will just set the battle state to setup
				var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
				battleStateComponent.BattleState = BattleState.Fighting;
				SystemAPI.SetSingleton<BattleStateComponent>(battleStateComponent);

				battleSetup.BattleSetupStatus = BattleSetupStatus.Complete;
				SystemAPI.SetSingleton<BattleSetup>(battleSetup);
			}
		}
	}
}