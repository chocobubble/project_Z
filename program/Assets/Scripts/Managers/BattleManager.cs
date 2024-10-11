using UnityEngine;
using Character;
using System.Collections.Generic;	
using Data;
using System;
using Unity.Entities.UniversalDelegates;

namespace Battle 
{
	public class BattleManager : MonoBehaviour
	{
		private List<CharacterBundle> playerCharacterBundles;
		private List<CharacterBundle> enemyCharacterBundles;
		private CharactersManager playerCharactersManager;
		private CharactersManager enemyCharactersManager;
		private CharacterSpawner characterSpawner;
		public TurnManager turnManager;
		public GameObject characterSpawnerGameObject;
		private BattleState battleState;
		private TurnPhase turnPhase;
		public Action<BattleState> OnBattleStateChanged;

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
			Debug.Log("BattleManager OnEnable");
			characterSpawner = characterSpawnerGameObject.GetComponent<CharacterSpawner>();
			playerCharacterBundles = new List<CharacterBundle>();
			enemyCharacterBundles = new List<CharacterBundle>();

			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				playerCharacterBundles.Add(new CharacterBundle(BattleConstants.PLAYER_CHARACTERS_DATA[i]));
				enemyCharacterBundles.Add(new CharacterBundle(BattleConstants.ENEMY_CHARACTERS_DATA[i]));
			}

			OnBattleStateChanged += OnBattleStateChangedHandler;
			ChangeBattleState(BattleState.Setup);
		}

		public void Start()
		{
			Debug.Log("BattleManager Start");
		}

		public void Update()
		{

		}

		private void SpawnCharacters()
		{
			var playerCharacters = new List<UnitController>();
			var enemyCharacters = new List<UnitController>();
			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				var playerCharacter = characterSpawner.SpawnCharacter(playerCharacterBundles[i], BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
				var enemyCharacter = characterSpawner.SpawnCharacter(enemyCharacterBundles[i], BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
				turnManager.playerCharacters[i] = playerCharacter;
				turnManager.enemyCharacters[i] = enemyCharacter;
				playerCharacters.Add(playerCharacter.GetComponent<UnitController>());
				enemyCharacters.Add(enemyCharacter.GetComponent<UnitController>());
				playerCharacter.GetComponent<UnitController>().SetBasePosition(BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
				enemyCharacter.GetComponent<UnitController>().SetBasePosition(BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
			}

			playerCharactersManager = new CharactersManager(playerCharacters);
			enemyCharactersManager = new CharactersManager(enemyCharacters);

			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				playerCharacters[i].SetCharactersManagers(playerCharactersManager, enemyCharactersManager);
				enemyCharacters[i].SetCharactersManagers(enemyCharactersManager, playerCharactersManager);
			}
		}

		private void RemoveCharacters()
		{
			characterSpawner.RemoveCharacter();
		}

		private void ChangeBattleState(BattleState battleState)
		{
			CurrentBattleState = battleState;	
		}

		private void OnBattleStateChangedHandler(BattleState battleState)
		{
			// TODO : 스택 무한히 쌓일 가능성..
			switch (battleState)
			{
				case BattleState.None:
					break;
				case BattleState.Setup:
					SpawnCharacters();
					ChangeBattleState(BattleState.Start);
					break;
				case BattleState.Start:
					turnManager.CurrentTurnPhase = TurnPhase.PreAttack;
					// ChangeBattleState(BattleState.End);
					break;
				case BattleState.End:
					RemoveCharacters();
					break;
			}
		}
	}
}	