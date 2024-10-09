using UnityEngine;
using Character;
using System.Collections.Generic;	
using Data;
using System;

namespace Battle 
{
	public class BattleManager : MonoBehaviour
	{
		private List<CharacterBundle> playerCharacterBundles;
		private List<CharacterBundle> enemyCharacterBundles;
		private CharacterSpawner characterSpawner;
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
			characterSpawner.SpawnCharacter(playerCharacterBundles.ToArray(), BattleConstants.PLAYER_CHARACTER_POSITIONS);
			characterSpawner.SpawnCharacter(enemyCharacterBundles.ToArray(), BattleConstants.ENEMY_CHARACTER_POSITIONS);
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
			switch (battleState)
			{
				case BattleState.None:
					break;
				case BattleState.Setup:
					SpawnCharacters();
					ChangeBattleState(BattleState.Start);
					break;
				case BattleState.Start:
					ChangeBattleState(BattleState.End);
					break;
				case BattleState.End:
					RemoveCharacters();
					break;
			}
		}
	}
}	