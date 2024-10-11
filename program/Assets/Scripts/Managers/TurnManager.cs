using UnityEngine;
using Data;
using System;
using System.Collections.Generic;
using Character;

namespace Battle 
{
	public class TurnManager : MonoBehaviour
	{
		public List<GameObject> playerCharacters;
		public List<GameObject> enemyCharacters;
		private TurnPhase turnPhase;
		public bool ShouldCharacterSpawn { get; set; }

		public TurnPhase CurrentTurnPhase 
		{
			get { return turnPhase; }
			set 
			{
				if (turnPhase != value)
				{
					Debug.Log("TurnPhase changed from " + turnPhase + " to " + value);
					turnPhase = value;
				}
				else 
				{
					Debug.Log("TurnPhase is already " + value);
				}
			}
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
					break;
			}	
		}

		private void OnAttackPhase()
		{
			Debug.Log("OnAttackPhase");
			playerCharacters[0].GetComponent<UnitController>().CharacterActionState = CharacterActionState.MovingToTarget;
			enemyCharacters[0].GetComponent<UnitController>().CharacterActionState = CharacterActionState.MovingToTarget;
		}

		void OnDestroy()
		{
			Debug.Log("TurnManager OnDestroy");
		}
	}
}