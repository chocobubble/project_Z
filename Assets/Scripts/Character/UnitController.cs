using System;
using Battle;
using Data;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace Character 
{
	public class UnitController : MonoBehaviour
	{
		[SerializeField]
		private bool isMoving;
		[SerializeField]
		private Vector3 basePosition;
		[SerializeField]
		private Vector3 targetPosition;
		private CharactersManager allyCharactersManager;
		private CharactersManager enemyCharactersManager;
		private CharacterBundle _characterBundle;
		[SerializeField]
		private CharacterStat _characterStat;
		private CharacterActionState _characterActionState;
		public CharacterActionState CharacterActionState 
		{
			get { return _characterActionState; }
			set 
			{
				// 예외 체크 
				if (_characterActionState == CharacterActionState.Dead && value != CharacterActionState.Dead)
				{
					Debug.LogError("Character is dead. Cannot change state to " + value);
				}

				if (_characterActionState != value)
				{
					_characterActionState = value;
					OnActionStateChanged(_characterActionState);
				}
			}
		}
		public CharacterStat CharacterStat
		{
			get { return _characterStat; }
			set { _characterStat = value; }
		}

		void Start()
		{
			_characterBundle = new CharacterBundle();
			_characterActionState = CharacterActionState.Idle;
			// targetPosition = Vector3.zero;
			// isMoving = false;
		}

		void Update()
		{
			if (isMoving)
			{
				// transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * BattleConstants.MOVE_SPEED);
				transform.position = Vector3.Lerp(transform.position, targetPosition, BattleConstants.MOVE_SPEED * Time.deltaTime);
			}

			// On action state changed logic
			switch (_characterActionState)
			{
				case CharacterActionState.Idle:
					break;
				case CharacterActionState.MovingToTarget:
					isMoving = true;
					if (IsArrivedAtTargetPosition())
					{
						CharacterActionState = CharacterActionState.Attacking;
						isMoving = false;
					}
					break;
				case CharacterActionState.Attacking:
					Attack(enemyCharactersManager.GetUnitController(0));
					CharacterActionState = CharacterActionState.ReturningToPosition;
					targetPosition = basePosition;
					break;
				case CharacterActionState.ReturningToPosition:
					isMoving = true;
					if (IsArrivedAtTargetPosition())
					{
						CharacterActionState = CharacterActionState.Idle;
						isMoving = false;
					}
					break;
				case CharacterActionState.Stunned:
					break;
				case CharacterActionState.Dead:
					if (IsArrivedAtTargetPosition())
					{
						Debug.Log("Character is dead");
						Destroy(gameObject);
					}
					break;
				case CharacterActionState.Moving:
				 	if (IsArrivedAtTargetPosition())
					{
						CharacterActionState = CharacterActionState.Idle;
						isMoving = false;
					}
					break;
			}

			CheckCharacterState();
		}

		private void CheckCharacterState()
		{
			if (_characterStat.CurrentHP <= 0)
			{
				CharacterActionState = CharacterActionState.Dead;
			}
		}

		public void SetCharacterBundle(CharacterBundle characterBundle)
		{
			if (characterBundle == null)
			{
				Debug.LogError("CharacterBundle is null");
				return;
			}
			// Deep copy
			_characterBundle = new CharacterBundle(characterBundle);
			_characterStat = _characterBundle.GetCharacterStat();
			Debug.Log("CharacterStat Set to " + _characterStat);
		}

		public CharacterBundle GetCharacterBundle()
		{
			return _characterBundle;
		}

		public CharacterStat GetCharacterStat()
		{
			return _characterStat;
		}

		public void TakeDamage(int damage)
		{
			_characterStat.CurrentHP -= damage;
			if (_characterStat.CurrentHP <= 0)
			{
				// CharacterActionState = CharacterActionState.Dead;
				_characterStat.CurrentHP = 0;
			}
		}

		public int GetAttackDamage()
		{
			// Get attack damage logic
			return _characterStat.Attack;
		}

		public int GetCurrentHP()
		{
			return _characterStat.CurrentHP;
		}

		public void Attack(UnitController otherCharacter)
		{
			// Attack logic
			otherCharacter.TakeDamage(GetAttackDamage());
		}

		public void BaseAttack()
		{
			// Base attack logic
			Attack(enemyCharactersManager.GetUnitController(0));
		}

		public void OnActionStateChanged(CharacterActionState characterActionState)
		{
			// On action state changed logic
			switch (characterActionState)
			{
				case CharacterActionState.Idle:
					targetPosition = Vector3.zero;
					isMoving = false;
					break;
				case CharacterActionState.MovingToTarget:
					break;
				case CharacterActionState.Attacking:
					break;
				case CharacterActionState.ReturningToPosition:
					break;
				case CharacterActionState.Stunned:
					break;
				case CharacterActionState.Dead:
					targetPosition = BattleConstants.DEAD_POSITION;
					isMoving = true;
					break;
				case CharacterActionState.Moving:
				 	isMoving = true;
					break;
			}
		}

		public void SetCharactersManagers(CharactersManager allyCharactersManager, CharactersManager enemyCharactersManager)
		{
			this.allyCharactersManager = allyCharactersManager;
			this.enemyCharactersManager = enemyCharactersManager;
		}

		public void SetBasePosition(Vector3 basePosition)
		{
			this.basePosition = basePosition;
		}

		public void SetTargetPosition(Vector3 targetPosition)
		{
			this.targetPosition = targetPosition;
		}

		// 타겟 포지션에 도착했는 지 확인하는 함수
		public bool IsArrivedAtTargetPosition()
		{
			float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);
			if (distanceToTargetPosition < 0.5f)
			{
				Debug.Log("Arrived at target position - targetPosition: " + targetPosition + " current position: " + transform.position);
				return true;
			}
			else 
			{
				return false;
			}
		}

		public void SetCharacterActionState(CharacterActionState characterActionState)
		{
			CharacterActionState = characterActionState;
		}
	}
}