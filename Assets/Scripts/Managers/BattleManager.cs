using UnityEngine;
using Character;
using System.Collections.Generic;	
using Data;
using System;
using Unity.Entities.UniversalDelegates;
using Setup;
using BattleUI;

namespace Battle 
{
	public class BattleManager : MonoBehaviour
	{
		private List<CharacterBundle> playerCharacterBundles;
		private List<CharacterBundle> enemyCharacterBundles;
		private CharactersManager playerCharactersManager;
		private CharactersManager enemyCharactersManager;
		private CharacterSpawner characterSpawner;
		public SetupManager setupManager;
		public TurnManager turnManager;
		public GameObject characterSpawnerGameObject;
		private BattleState battleState;
		private TurnPhase turnPhase;
		public Action<BattleState> OnBattleStateChanged;
		private PlayerStats playerStats;
		private CharacterDatabase characterDatabase;	
		public BattleSetupUIController battleSetupUIController;

		public GameObject DebugUIGameObject;

		public BattleState CurrentBattleState 
		{
			get { return battleState; }
			set 
			{
				if (battleState != value)
				{
					Debug.Log("BattleState changed from " + battleState + " to " + value);
					battleState = value;
					OnBattleStateChanged?.Invoke(battleState);
				}
				else 
				{
					Debug.Log("BattleState is already " + value);
				}
			}
		}

		public void OnEnable()
		{
			playerStats = new PlayerStats(
				BattleConstants.PLAYER_STARTING_HEART,
				BattleConstants.PLAYER_STARTING_GOLD,
				BattleConstants.PLAYER_STARTING_TURN,
				BattleConstants.PLAYER_STARTING_WIN_STREAK
			);

			Debug.Log("BattleManager OnEnable");
			characterSpawner = characterSpawnerGameObject.GetComponent<CharacterSpawner>();
			playerCharacterBundles = new List<CharacterBundle>();
			enemyCharacterBundles = new List<CharacterBundle>();

			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				// playerCharacterBundles.Add(new CharacterBundle(BattleConstants.PLAYER_CHARACTERS_DATA[i]));
				playerCharacterBundles.Add(null);
				enemyCharacterBundles.Add(new CharacterBundle(BattleConstants.ENEMY_CHARACTERS_DATA[i]));
			}

			if (battleSetupUIController == null)
			{
				battleSetupUIController = GameObject.Find("BattleSetupUI").GetComponent<BattleSetupUIController>();
			}


			if (setupManager == null)
			{
				setupManager = GameObject.Find("SetupManager").GetComponent<SetupManager>();
			}
			setupManager.SetCharacterSpawner(characterSpawner);
			
			if (characterDatabase == null)
			{
				characterDatabase = new CharacterDatabase();
			}
			setupManager.SetCharacterDatabase(characterDatabase);
			setupManager.gameObject.SetActive(true);

			OnBattleStateChanged += OnBattleStateChangedHandler;
			ChangeBattleState(BattleState.Setup);
		}

		public void OnDisable()
		{
			setupManager.gameObject.SetActive(false);
		}

		public void Start()
		{
			Debug.Log("BattleManager Start");
		
			if (turnManager != null) 
			{
				turnManager.OnCharacterListChanged += (playerCharacters, enemyCharacters) => 
				{
					playerCharactersManager.UpdateCharacters(playerCharacters);
					enemyCharactersManager.UpdateCharacters(enemyCharacters);
				};
			}
		}

		public void Update()
		{
			switch (CurrentBattleState)
			{
				case BattleState.None:
					break;
				case BattleState.Setup:
				
					break;
				case BattleState.Start:
				 	if (turnManager.CurrentTurnPhase == TurnPhase.End)
					{
						RemoveCharacters();
						OnTurnEnd();
						CurrentBattleState = BattleState.Setup;
					}
					break;
				case BattleState.End:
					break;
			}
		}

		private void OnTurnEnd()
		{
			var didPlayerWin = turnManager.DidPlayerWin();
			if (didPlayerWin) OnPlayerWin();
			else OnPlayerLose();
		}

		private void OnPlayerLose()
		{
			if (playerStats == null) 
			{
				CustomLogger.LogError("PlayerStats is null");
				return;
			}
			playerStats.Gold = BattleConstants.BATTLE_DEFAULT_COIN;
			playerStats.Turn += 1;
			playerStats.Heart -= 1;

			// TODO: playerStats.Heart <= 0 일 때 종료
		}

		private void OnPlayerWin()
		{
			if (playerStats == null) 
			{
				CustomLogger.LogError("PlayerStats is null");
				return;
			}
			playerStats.Gold = BattleConstants.BATTLE_DEFAULT_COIN;
			playerStats.Turn += 1;
			playerStats.WinStreak += 1;

			// TODO: player winstreak > 10 일 때 종료
		}

		private void SpawnCharacters()
		{
			var playerCharacters = new List<UnitController>();
			var enemyCharacters = new List<UnitController>();
			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				if (playerCharacterBundles[i] == null) continue;
				var playerCharacter = characterSpawner.SpawnCharacter(playerCharacterBundles[i], BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
				turnManager.PlayerCharacters[i] = playerCharacter;
				playerCharacters.Add(playerCharacter.GetComponent<UnitController>());
				playerCharacter.GetComponent<UnitController>().SetBasePosition(BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
			}

			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				if (enemyCharacterBundles[i] == null) continue;
				var enemyCharacter = characterSpawner.SpawnCharacter(enemyCharacterBundles[i], BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
				turnManager.EnemyCharacters[i] = enemyCharacter;
				enemyCharacters.Add(enemyCharacter.GetComponent<UnitController>());
				enemyCharacter.GetComponent<UnitController>().SetBasePosition(BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
			}

			playerCharactersManager = new CharactersManager(playerCharacters);
			enemyCharactersManager = new CharactersManager(enemyCharacters);

			foreach (var playerCharacter in playerCharacters)
			{
				playerCharacter.SetCharactersManagers(playerCharactersManager, enemyCharactersManager);
			}

			foreach (var enemyCharacter in enemyCharacters)
			{
				enemyCharacter.SetCharactersManagers(enemyCharactersManager, playerCharactersManager);
			}
		}

		private void RemoveCharacters()
		{
			characterSpawner.RemoveCharacter();
		}

		public void ChangeBattleState(BattleState battleState)
		{
			CurrentBattleState = battleState;	
		}

		private void OnBattleStateChangedHandler(BattleState battleState)
		{
			Debug.Log("Battle State Changed to  " + battleState);
			// TODO : 스택 무한히 쌓일 가능성..
			switch (battleState)
			{
				case BattleState.None:
					break;
				case BattleState.Setup:
					Camera.main.transform.position = BattleConstants.BATTLE_SETUP_CAMERA_POSITION;
					turnManager.gameObject.SetActive(false);
					setupManager.gameObject.SetActive(true);
					battleSetupUIController.gameObject.SetActive(true);
					DebugUIGameObject?.SetActive(false);
					break;
				case BattleState.Start:
				 	Camera.main.transform.position = BattleConstants.BATTLE_START_CAMERA_POSITION;
					turnManager.gameObject.SetActive(true);
					SpawnCharacters();
					// turnManager.CurrentTurnPhase = TurnPhase.PreAttack;
					turnManager.CurrentTurnPhase = TurnPhase.Spawning;
					// ChangeBattleState(BattleState.End);
					DebugUIGameObject?.SetActive(true);
					break;
				case BattleState.End:
					break;
			}
		}

		private void SpawnCharactersForSetup()
		{

		}

		public void ExcludeCharacterFromPlayer(int index)
		{
			playerCharacterBundles[index] = null;
		}

		public void GainCoin(int amount)
		{
			playerStats.Gold += amount;
		}

#region Get Set
		public PlayerStats GetPlayerStats()
		{
			return playerStats;
		}

		public SetupManager GetSetupManager()
		{
			return setupManager;
		}
	
		public List<CharacterBundle> GetPlayerCharacterBundles()
		{
			return playerCharacterBundles;
		}	

		public int GetCoin()
		{
			if (playerStats == null) 
			{
				CustomLogger.LogError("PlayerStats is null");
				return 0;
			}
			return playerStats.Gold;
		}
#endregion
	}
}	