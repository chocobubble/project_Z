using Unity.Entities;
using UnityEngine;
using Data;
using BattleUI;
using Unity.Burst;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	/// <summary>
	/// Battle State 가 Setup 상태일 때, 작동하는 시스템
	/// </summary>
	[UpdateAfter(typeof(BattleManagerSystem))]
	public partial struct BattleSetupSystem : ISystem
	{
		int coin;
		private bool initialized;
		private bool isCoinUpdateRequired;

		public void OnCreate(ref SystemState state) 
		{
			state.RequireForUpdate<BattleSetupComponent>();
			state.RequireForUpdate<BattleStateComponent>();
			state.RequireForUpdate<BattleSetupManagedComponent>();

			coin = BattleConstants.BATTLE_DEFAULT_COIN;
			initialized = false;
			isCoinUpdateRequired = true;
		}
		public void OnDestroy(ref SystemState state) {}

		// [BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (SystemAPI.HasSingleton<BattleStateComponent>() == false) 
			{
				Debug.LogError("BattleSetup component is null");
				return;
			}
			var battleStateComponent = SystemAPI.GetSingleton<BattleStateComponent>();
			if (battleStateComponent.BattleState != BattleState.Setup) 
			{
				return;
			} 

			var battleSetupEntity = SystemAPI.GetSingletonEntity<BattleSetupComponent>();
			var battleSetupManagedComponent = state.EntityManager.GetComponentObject<BattleSetupManagedComponent>(battleSetupEntity);

			if (!initialized) 
			{
				battleSetupManagedComponent.BattleSetupUIController = Object.FindFirstObjectByType<BattleSetupUIController>();
				if (battleSetupManagedComponent.BattleSetupUIController == null) 
				{
					Debug.LogError("BattleSetupUIController not found in scene");
					return;
				}
				initialized = true;
				Debug.Log("BattleSetupSystem Initialized");
			}

			var battleSetupUIController = battleSetupManagedComponent.BattleSetupUIController;
			if (battleSetupUIController != null && isCoinUpdateRequired) 
			{
				battleSetupUIController.UpdateCoin(coin);
				isCoinUpdateRequired = false;
			}
		}

		public void Reroll()
		{
			if (coin <= 0) 
			{
				Debug.Log("Not enough coin to reroll");
				return;
			}

			SetCoin(-1);
			Debug.Log("Rerolling");			
		}

		public void SetCoin(int value)
		{
			this.coin = coin + value;
			Debug.Log("Setting Coin to " + this.coin);
			isCoinUpdateRequired = true;
		}


	}
}