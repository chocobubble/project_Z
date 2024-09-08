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
		}
	}
}