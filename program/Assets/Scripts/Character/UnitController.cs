using System.Collections.Generic;
using Battle;
using Data;
using Unity.VisualScripting;
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
			_characterActionState = CharacterActionState.None;
			targetPosition = Vector3.zero;
			isMoving = false;
		}

		void Update()
		{
			if (isMoving)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * BattleConstants.MOVE_SPEED);
				if (transform.position == targetPosition)
				{
					isMoving = false;
					if (_characterActionState == CharacterActionState.MovingToTarget)
					{
						_characterActionState = CharacterActionState.Attacking;
					}
					else if (_characterActionState == CharacterActionState.ReturningToPosition)
					{
						_characterActionState = CharacterActionState.Idle;
					}
				}
			}
		}



		public void SetCharacterBundle(CharacterBundle characterBundle)
		{
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
				_characterActionState = CharacterActionState.Dead;
				_characterStat.CurrentHP = 0;
			}
		}

		public int GetAttackDamage()
		{
			// Get attack damage logic
			return _characterStat.Attack;
		}

		public void Attack(UnitController otherCharacter)
		{
			// Attack logic
			otherCharacter.TakeDamage(GetAttackDamage());
		}

		public void OnActionStateChanged(CharacterActionState characterActionState)
		{
			// On action state changed logic
			switch (characterActionState)
			{
				case CharacterActionState.Idle:
					break;
				case CharacterActionState.MovingToTarget:
					isMoving = true;
					break;
				case CharacterActionState.Attacking:
					Attack(enemyCharactersManager.GetUnitController(0));
					CharacterActionState = CharacterActionState.ReturningToPosition;
					targetPosition = basePosition;
					break;
				case CharacterActionState.ReturningToPosition:
					break;
				case CharacterActionState.Stunned:
					break;
				case CharacterActionState.Dead:
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
	}
}