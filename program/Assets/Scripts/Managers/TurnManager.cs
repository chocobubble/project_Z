using UnityEngine;
using Data;
using System;
using System.Collections.Generic;
using Character;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Battle 
{
	public class TurnManager : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> playerCharacters;
		[SerializeField]
		private List<GameObject> enemyCharacters;
		private TurnPhase turnPhase;
		public bool ShouldCharacterSpawn { get; set; }
		private bool shouldCharactersReposition = false;

		public Action<List<GameObject>, List<GameObject>> OnCharacterListChanged;

		public TurnPhase CurrentTurnPhase 
		{
			get { return turnPhase; }
			set 
			{
				if (turnPhase != value)
				{
					Debug.Log("TurnPhase changed from " + turnPhase + " to " + value);
					turnPhase = value;
					OnPhaseChanged(turnPhase);
				}
				else 
				{
					Debug.Log("TurnPhase is already " + value);
				}
			}
		}

		private void OnPhaseChanged(TurnPhase turnPhase)
		{
			switch (turnPhase)
			{
				case TurnPhase.None:
					break;
				case TurnPhase.Spawning:
					break;
				case TurnPhase.PreAttack:
					break;
				case TurnPhase.Attack:
				 	// OnAttackPhase();
					break;
				case TurnPhase.PostAttack:
					break;
				case TurnPhase.Positioning:
					break;
			}
		}

		public List<GameObject> PlayerCharacters 
		{
			get { return playerCharacters; }
			set { playerCharacters = value; }
		}

		public List<GameObject> EnemyCharacters 
		{
			get { return enemyCharacters; }
			set { enemyCharacters = value; }
		}

		void OnEnable()
		{
			Debug.Log("TurnManager OnEnable");

			turnPhase = TurnPhase.None;

		}

		void Start()
		{
			Debug.Log("TurnManager Start");
			// 단순히 괄호 안에 숫자만 넣으면 용량만 정하는 거라 작동을 안한다.
			// 그래서 null로 초기화를 해줘야 한다.
			playerCharacters = new List<GameObject>();
			enemyCharacters = new List<GameObject>();
			for (int i = 0; i < BattleConstants.PLAYER_CHARACTERS_DATA.Length; i++)
			{
				playerCharacters.Add(null);
				enemyCharacters.Add(null);
			}
		}

		void Update()
		{
			switch (turnPhase)
			{
				case TurnPhase.None:
					break;
				case TurnPhase.Spawning:
					break;
				case TurnPhase.PreAttack:
					CurrentTurnPhase = TurnPhase.Attack;
					break;
				case TurnPhase.Attack:
				 	OnAttackPhase();
					CurrentTurnPhase = TurnPhase.PostAttack;
					break;
				case TurnPhase.PostAttack:
					if (shouldCharactersReposition)
						RepositionCharacters();
					CheckCharactersState();
					// CurrentTurnPhase = TurnPhase.PreAttack;
					// wait for a while

					break;
				case TurnPhase.Positioning:
					// if (IsAllCharactersIdle())
					// {
					// 	CurrentTurnPhase = TurnPhase.PreAttack;
					// }
					break;
			}
		}

		private bool IsMovingToTargetEnd()
		{
			return playerCharacters[0].GetComponent<UnitController>().CharacterActionState == CharacterActionState.Attacking
				&& enemyCharacters[0].GetComponent<UnitController>().CharacterActionState == CharacterActionState.Attacking;
		}

		private void ActionBaseAttack()
		{
			playerCharacters[0].GetComponent<UnitController>().BaseAttack();
			enemyCharacters[0].GetComponent<UnitController>().BaseAttack();
		}

		private void RepositionCharacters()
		{
			
		}

		private void CheckCharactersState()
		{
			// Debug.Log("CheckCharactersState");
			for (int i = 0; i < playerCharacters.Count; i++)
			{
				if (playerCharacters[i] == null)
				{
					for (int j = i + 1; j < playerCharacters.Count; j++)
					{
						if (playerCharacters[j] != null)
						{
							playerCharacters[i] = playerCharacters[j];
							playerCharacters[j] = null;

							// Set the base position of the character
							playerCharacters[i].GetComponent<UnitController>().SetBasePosition(BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
							playerCharacters[i].GetComponent<UnitController>().SetTargetPosition(BattleConstants.PLAYER_CHARACTER_POSITIONS[i]);
							playerCharacters[i].GetComponent<UnitController>().SetCharacterActionState(CharacterActionState.Moving);
							break;
						}
					}
					continue;
				}

				// 뭐지? 이게 왜 필요한거지?
				var characterActionState = playerCharacters[i].GetComponent<UnitController>().CharacterActionState;
				if (characterActionState != CharacterActionState.Idle)
				{
					return;
				}
			}

			for (int i = 0; i < enemyCharacters.Count; i++)
			{
				if (enemyCharacters[i] == null)
				{
					for (int j = i + 1; j < enemyCharacters.Count; j++)
					{
						if (enemyCharacters[j] != null)
						{
							enemyCharacters[i] = enemyCharacters[j];
							enemyCharacters[j] = null;

							// Set the base position of the character
							enemyCharacters[i].GetComponent<UnitController>().SetBasePosition(BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
							enemyCharacters[i].GetComponent<UnitController>().SetTargetPosition(BattleConstants.ENEMY_CHARACTER_POSITIONS[i]);
							enemyCharacters[i].GetComponent<UnitController>().SetCharacterActionState(CharacterActionState.Moving);
							break;
						}
					}
					continue;
				}
				var characterActionState = enemyCharacters[i].GetComponent<UnitController>().CharacterActionState;
				if (characterActionState != CharacterActionState.Idle)
				{
					return;
				}
			}

			OnCharacterListChanged?.Invoke(playerCharacters, enemyCharacters);
			CurrentTurnPhase = TurnPhase.PreAttack;
		}

		private void OnAttackPhase()
		{
			Debug.Log("OnAttackPhase");
			playerCharacters[0].GetComponent<UnitController>().CharacterActionState = CharacterActionState.MovingToTarget;
			playerCharacters[0].GetComponent<UnitController>().SetTargetPosition(BattleConstants.PLAYER_ATTACK_POSITION);
			enemyCharacters[0].GetComponent<UnitController>().CharacterActionState = CharacterActionState.MovingToTarget;
			enemyCharacters[0].GetComponent<UnitController>().SetTargetPosition(BattleConstants.ENEMY_ATTACK_POSITION);
		}

		void OnDestroy()
		{
			Debug.Log("TurnManager OnDestroy");
		}
	}
}