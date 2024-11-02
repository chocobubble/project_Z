using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Battle;
using Character;
using Data;
using Setup;
using Unity.Entities.UniversalDelegates;
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
		private Button playerCharacterButton0;
		private Button playerCharacterButton1;
		private Button playerCharacterButton2;
		private Button playerCharacterButton3;
		private Button purchasableCharacterButton0;
		private Button purchasableCharacterButton1;
		private Button purchasableCharacterButton2;
		private Button purchasableCharacterButton3;
		private Button purchasableCharacterButton4;
		[SerializeField]
		private List<CharacterButton> characterButtons;
		[SerializeField]
		private List<CharacterButton> purchasableCharacterButtons;
		// private List<Button> purchasableCharacterButtons;
		[SerializeField]
		private List<CharacterButton> playerCharacterButtons;
		// private List<Button> playerCharacterButtons;
		[SerializeField]
		private CharacterBundle selectedCharacterBundle;
		[SerializeField]
		private CharacterButton selectedCharacterButton;

		public CharacterButton SelectedCharacterButton
		{
			get { return selectedCharacterButton; }
			set {
				selectedCharacterButton = value;
				selectedCharacterBundle = value?.GetCharacterBundle();
			}
		}

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
			} else Debug.LogError("BattleManager is null");

			playerCharacterButton0 = root.Q<Button>("player-character-button0");
			playerCharacterButton1 = root.Q<Button>("player-character-button1");
			playerCharacterButton2 = root.Q<Button>("player-character-button2");
			playerCharacterButton3 = root.Q<Button>("player-character-button3");
			purchasableCharacterButton0 = root.Q<Button>("purchasable-character-button0");
			purchasableCharacterButton1 = root.Q<Button>("purchasable-character-button1");
			purchasableCharacterButton2 = root.Q<Button>("purchasable-character-button2");
			purchasableCharacterButton3 = root.Q<Button>("purchasable-character-button3");
			purchasableCharacterButton4 = root.Q<Button>("purchasable-character-button4");
			
			characterButtons = new List<CharacterButton>();
			
			CreatepurchasableCharacters();
			InitializePlayerCharacterButtons();
			InitializepurchasableCharacterButtons();
			InitializeCharacterButtons();
			UpdateCharacterButtonsText();
		}

		private void CreatepurchasableCharacters()
		{
			var characterDatabase = setupManager.GetCharacterDatabase();
			// select 5 random characters in the database
			var characterDataList = characterDatabase.GetCharacterDataList();
			var purchasableCharacters = new List<CharacterBundle>();
			var random = new System.Random();
			for (int i = 0; i < 5; i++)
			{
				var randomIndex = random.Next(0, characterDataList.Count);
				var characterBundle = new CharacterBundle(characterDataList[randomIndex]);
				purchasableCharacters.Add(characterBundle);
			}
			setupManager.SetpurchasableCharacters(purchasableCharacters);
		}

		private void InitializePlayerCharacterButtons()
		{
			var playerCharacterBundles = battleManager.GetPlayerCharacterBundles();
			playerCharacterButtons = new List<CharacterButton> {
				new CharacterButton(0, playerCharacterButton0, playerCharacterBundles[0], CharacterButtonType.PLAYER),
				new CharacterButton(1, playerCharacterButton1, playerCharacterBundles[1], CharacterButtonType.PLAYER),
				new CharacterButton(2, playerCharacterButton2, playerCharacterBundles[2], CharacterButtonType.PLAYER),
				new CharacterButton(3, playerCharacterButton3, playerCharacterBundles[3], CharacterButtonType.PLAYER)
			};	

			foreach (var playerButton in playerCharacterButtons)
			{
				if (playerButton == null || playerButton.GetButtonUI() == null)
				{
					Debug.Log("Player Button is null");
					return;
				}
				characterButtons.Add(playerButton);
			}
		}
		private void InitializeCharacterButtons()
		{
			foreach (var characterButton in characterButtons)
			{
				var button = characterButton.GetButtonUI();
				var characterBundle = characterButton.GetCharacterBundle();
				button.clicked += () =>
				{
					// if (button.focusController.focusedElement == button)
					// {
					// 	Debug.Log("Button is already focused");
					// 	button.Blur();
					// }
					OnCharacterButtonClicked(characterButton);
				};
				button.AddToClassList("selected");
			}
		}

		private void OnCharacterButtonClicked(CharacterButton characterButton)
		{
			Debug.Log("Character Button Clicked");
			if (characterButton == null) 
			{
				Debug.LogError("Character Button is null");
				return;
			}
			if (SelectedCharacterButton == null)
			{
				SelectedCharacterButton = characterButton;
				return;
			}
			if (SelectedCharacterButton == characterButton)
			{
				characterButton.GetButtonUI().Blur();
				SelectedCharacterButton = null;
				return;
			}
			if (SelectedCharacterButton.GetCharacterButtonType() == CharacterButtonType.PURCHASABLE
				&& characterButton.GetCharacterButtonType() == CharacterButtonType.PLAYER)
			{
				// Check index of player character buttons and empty slot
				var characterButtonIndex = characterButton.GetButtonIndex();
				var playerCharacterBundles = battleManager.GetPlayerCharacterBundles(); 

				// TODO: 같은 캐릭터면 합체될 수 있게 변경하기
				if (playerCharacterBundles[characterButtonIndex] != null)
				{
					Debug.Log("Player character already exists in the slot");
					return;
				}
				
				// check if the player has enough gold
				var playerStats = battleManager.GetPlayerStats();
				var characterCost = BattleConstants.PURCHASE_CHARACTER_PRICE; 
				if (playerStats.Gold < characterCost)
				{
					Debug.Log("Not enough gold to purchase character");
					return;
				}
				// Purchase Character
				var purchasableCharacterBundle = SelectedCharacterButton.GetCharacterBundle();
				playerCharacterBundles[characterButtonIndex] = new CharacterBundle(purchasableCharacterBundle);
				playerStats.Gold -= characterCost;	

				// Update player character button
				characterButton.SetCharacterBundle(playerCharacterBundles[characterButtonIndex]);
				// Make selected character button to null
				SelectedCharacterButton.Reset();
				SelectedCharacterButton.GetButtonUI().Blur();
				SelectedCharacterButton = null;
				UpdateCharacterButtonsText();

				return;
			}
		}

		private void InitializepurchasableCharacterButtons()
		{
			var purchasableCharacters = setupManager.GetpurchasableCharacters();
			purchasableCharacterButtons = new List<CharacterButton> {
				new CharacterButton(0, purchasableCharacterButton0, purchasableCharacters[0], CharacterButtonType.PURCHASABLE),
				new CharacterButton(1, purchasableCharacterButton1, purchasableCharacters[1], CharacterButtonType.PURCHASABLE),
				new CharacterButton(2, purchasableCharacterButton2, purchasableCharacters[2], CharacterButtonType.PURCHASABLE),
				new CharacterButton(3, purchasableCharacterButton3, purchasableCharacters[3], CharacterButtonType.PURCHASABLE),
				new CharacterButton(4, purchasableCharacterButton4, purchasableCharacters[4], CharacterButtonType.PURCHASABLE)
			};	

			foreach (var purchasableButton in purchasableCharacterButtons)
			{
				if (purchasableButton == null || purchasableButton.GetButtonUI() == null)
				{
					Debug.Log("purchasable Button is null");
					return;
				}
				characterButtons.Add(purchasableButton);
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

			foreach (var purchasableButton in purchasableCharacterButtons)
			{
				purchasableButton.UpdateCharacterButtonsText();
			}
		}
	}
}