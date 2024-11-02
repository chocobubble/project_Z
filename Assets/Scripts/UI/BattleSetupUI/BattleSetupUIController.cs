using System;
using System.Collections.Generic;
using Battle;
using Character;
using Data;
using Setup;
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
		public SetupManager	setupManager;
		private Button playerCharacterButton1;
		private Button playerCharacterButton2;
		private Button playerCharacterButton3;
		private Button playerCharacterButton4;
		private Button purchaseableCharacterButton1;
		private Button purchaseableCharacterButton2;
		private Button purchaseableCharacterButton3;
		private Button purchaseableCharacterButton4;
		private Button purchaseableCharacterButton5;
		[SerializeField]
		private List<CharacterButton> purchaseableCharacterButtons;
		// private List<Button> purchaseableCharacterButtons;
		[SerializeField]
		private List<CharacterButton> playerCharacterButtons;
		// private List<Button> playerCharacterButtons;
		[SerializeField]
		private CharacterBundle selectedCharacterBundle;

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

				setupManager = battleManager.GetSetupManager();
			}
			else 
			{
				Debug.LogError("BattleManager is null");
			}
			playerCharacterButton1 = root.Q<Button>("player-character-button1");
			playerCharacterButton2 = root.Q<Button>("player-character-button2");
			playerCharacterButton3 = root.Q<Button>("player-character-button3");
			playerCharacterButton4 = root.Q<Button>("player-character-button4");


			// playerCharacterButtons = new List<Button>
			// {
			// 	playerCharacterButton1,
			// 	playerCharacterButton2,
			// 	playerCharacterButton3,
			// 	playerCharacterButton4
			// };
			purchaseableCharacterButton1 = root.Q<Button>("purchaseable-character-button1");
			purchaseableCharacterButton2 = root.Q<Button>("purchaseable-character-button2");
			purchaseableCharacterButton3 = root.Q<Button>("purchaseable-character-button3");
			purchaseableCharacterButton4 = root.Q<Button>("purchaseable-character-button4");
			purchaseableCharacterButton5 = root.Q<Button>("purchaseable-character-button5");


			// purchaseableCharacterButtons = new List<Button>
			// {
			// 	purchaseableCharacterButton1,
			// 	purchaseableCharacterButton2,
			// 	purchaseableCharacterButton3,
			// 	purchaseableCharacterButton4,
			// 	purchaseableCharacterButton5
			// };
			CreatePurchaseableCharacters();
			InitializePlayerCharacterButtons();
			InitializePurchaseableCharacterButtons();
			UpdateCharacterButtonsText();
		}

		private void CreatePurchaseableCharacters()
		{
			var characterDatabase = setupManager.GetCharacterDatabase();
			// select 5 random characters in the database
			var characterDataList = characterDatabase.GetCharacterDataList();
			var purchaseableCharacters = new List<CharacterBundle>();
			var random = new System.Random();
			for (int i = 0; i < 5; i++)
			{
				var randomIndex = random.Next(0, characterDataList.Count);
				var characterBundle = new CharacterBundle(characterDataList[randomIndex]);
				purchaseableCharacters.Add(characterBundle);
			}
			setupManager.SetPurchaseableCharacters(purchaseableCharacters);
		}

		private void InitializePlayerCharacterButtons()
		{
			var playerCharacterBundles = battleManager.GetPlayerCharacterBundles();
			playerCharacterButtons = new List<CharacterButton> {
				new CharacterButton(playerCharacterButton1, playerCharacterBundles[0]),
				new CharacterButton(playerCharacterButton2, playerCharacterBundles[1]),
				new CharacterButton(playerCharacterButton3, playerCharacterBundles[2]),
				new CharacterButton(playerCharacterButton4, playerCharacterBundles[3])
			};	

			foreach (var playerButton in playerCharacterButtons)
			{
				if (playerButton == null || playerButton.GetButtonUI() == null)
				{
					Debug.Log("Player Button is null");
					return;
				}
				var button = playerButton.GetButtonUI();	
				var characterBundle = playerButton.GetCharacterBundle();
				button.clicked += () =>
				{
					selectedCharacterBundle = characterBundle; 
					if (selectedCharacterBundle != null)
						Debug.Log($"Selected Character : {selectedCharacterBundle.GetCharacterId()}");
				};
			}
		}

		private void OnPlayerCharacterButtonClicked()
		{

		}

		private void InitializePurchaseableCharacterButtons()
		{
			var purchaseableCharacters = setupManager.GetPurchaseableCharacters();
			purchaseableCharacterButtons = new List<CharacterButton> {
				new CharacterButton(purchaseableCharacterButton1, purchaseableCharacters[0]),
				new CharacterButton(purchaseableCharacterButton2, purchaseableCharacters[1]),
				new CharacterButton(purchaseableCharacterButton3, purchaseableCharacters[2]),
				new CharacterButton(purchaseableCharacterButton4, purchaseableCharacters[3]),
				new CharacterButton(purchaseableCharacterButton5, purchaseableCharacters[4])
			};	

			foreach (var purchaseableButton in purchaseableCharacterButtons)
			{
				if (purchaseableButton == null || purchaseableButton.GetButtonUI() == null)
				{
					Debug.Log("Purchaseable Button is null");
					return;
				}
				var button = purchaseableButton.GetButtonUI();	
				var characterBundle = purchaseableButton.GetCharacterBundle();
				button.clicked += () =>
				{
					selectedCharacterBundle = characterBundle; 
					if (selectedCharacterBundle != null)
						Debug.Log($"Selected Character : {selectedCharacterBundle.GetCharacterId()}");
				};
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

		public void UpdateCharacterButtonsText()
		{
			if (battleManager == null)
			{
				Debug.LogError("BattleManager is null");
				return;
			}

			foreach (var playerButton in playerCharacterButtons)
			{
				playerButton.UpdateCharacterButtonsText();
			}

			foreach (var purchaseableButton in purchaseableCharacterButtons)
			{
				purchaseableButton.UpdateCharacterButtonsText();
			}

			// var playerCharacterBundles = battleManager.GetPlayerCharacterBundles();
			// var purchaseableCharacters = setupManager.GetPurchaseableCharacters();

			// for (int i = 0; i < playerCharacterBundles.Count; i++)
			// {
			// 	UpdateCharacterButtonText(playerCharacterButtons[i], playerCharacterBundles[i]);
			// }

			// for (int i = 0; i < purchaseableCharacters.Count; i++)
			// {
			// 	UpdateCharacterButtonText(purchaseableCharacterButtons[i], purchaseableCharacters[i].GetComponent<UnitController>().GetCharacterBundle());
			// }
		}

		// private void UpdateCharacterButtonText(Button button, CharacterBundle characterBundle)
		// {
		// 	if (button == null)
		// 	{
		// 		Debug.LogError("Button is null");
		// 		return;
		// 	}

		// 	if (characterBundle == null)
		// 	{
		// 		Debug.LogError("CharacterBundle is null");
		// 		return;
		// 	}
		// 	button.text = $"{characterBundle.GetCharacterId()}\n{characterBundle.GetCharacterStat().Attack} | {characterBundle.GetCharacterStat().HP}";
		// }
	}
}