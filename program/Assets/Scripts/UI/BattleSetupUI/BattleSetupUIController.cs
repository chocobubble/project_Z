using System;
using Battle;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleUI
{
	public class BattleSetupUIController : MonoBehaviour
	{
		public bool isSetupEnd;
		private Label coinLabel;
		private Label winStreakLabel;
		private Label turnLabel;
		private Label heartLabel;
		private Button rerollButton;
		private Button setupEndButton;
		private PlayerStats playerStats;
		public BattleManager battleManager;

		private void OnEnable()
		{
			var root = GetComponent<UIDocument>().rootVisualElement;

			coinLabel = root.Q<Label>("coin-label");
			winStreakLabel = root.Q<Label>("winstreak-label");
			turnLabel = root.Q<Label>("turn-label");
			heartLabel = root.Q<Label>("heart-label");

			rerollButton = root.Q<Button>("reroll-button");
			setupEndButton = root.Q<Button>("setup-end-button");

			rerollButton.clicked += Reroll;
			setupEndButton.clicked += SetupEnd;

			isSetupEnd = false;

			if (battleManager != null)
			{
				playerStats = battleManager.GetPlayerStats();
				playerStats.OnGoldChanged += UpdateCoin;
				playerStats.OnHeartChanged += UpdateHeart;
				playerStats.OnTurnChanged += UpdateTurn;
				playerStats.OnWinStreakChanged += UpdateWinStreak;
				UpdatePlayerStats();
			}
			else 
			{
				Debug.LogError("BattleManager is null");
			}
		}

		private void UpdatePlayerStats()
		{
			UpdateCoin(playerStats.Gold);
			UpdateHeart(playerStats.Heart);
			UpdateTurn(playerStats.Turn);
			UpdateWinStreak(playerStats.WinStreak);
		}

		private void UpdateWinStreak(int obj)
		{
			winStreakLabel.text = $"Win Streak\n{obj}";
		}

		private void UpdateTurn(int obj)
		{
			turnLabel.text = $"Turn\n{obj}";
		}

		private void UpdateHeart(int obj)
		{
			heartLabel.text = $"Heart\n{obj}";
		}

		private void Reroll()
		{
			Debug.Log("Rerolling Button Clicked");
		}

		private void SetupEnd()
		{
			Debug.Log("Setup End Button Clicked");
			isSetupEnd = true;

			// TODO : battle scene 켜지게
			// if (battleManager != null)
			// {
			// 	battleManager.SetActive(true);
			// }
			if (battleManager != null)
			{
				battleManager.ChangeBattleState(BattleState.Start);
			}
			else
			{
				Debug.LogError("BattleManager is null");
			}
			DeActivateUI();
		}

		public void UpdateCoin(int coin)
		{
			coinLabel.text = $"Coin\n{coin}";
		}

		public void DeActivateUI()
		{
			Debug.Log("Deactivating BattleSetupUI");
			gameObject.SetActive(false);
		}
	}
}