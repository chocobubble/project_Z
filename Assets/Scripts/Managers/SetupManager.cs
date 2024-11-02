using System.Collections.Generic;
using System.Security.Cryptography;
using Character;
using Data;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace Setup
{
	public class SetupManager : MonoBehaviour
	{
		[SerializeField]
		private CharacterDatabase characterDatabase;
		[SerializeField]
		private CharacterSpawner characterSpawner;
		[SerializeField]
		private List<CharacterBundle> purchasableCharacters;

		void Start()
		{

		}

		void OnEnable()
		{
			purchasableCharacters = new List<CharacterBundle>();
			// SpawnpurchasableCharacters();
		}

		void Update()
		{
		}

		private void SpawnpurchasableCharacters()
		{
			if (characterDatabase == null)
			{
				Debug.LogError("CharacterDatabase is null");
				return;
			}
			if (characterSpawner == null)
			{
				Debug.LogError("characterSpawner is null");
				return;
			}
		}


#region Get Set
		public void SetCharacterDatabase(CharacterDatabase characterDatabase)
		{
			this.characterDatabase = characterDatabase;
		}
		public void SetCharacterSpawner(CharacterSpawner characterSpawner)
		{
			this.characterSpawner = characterSpawner;
		}
		public void SetpurchasableCharacters(List<CharacterBundle> purchasableCharacters)
		{
			this.purchasableCharacters = purchasableCharacters;
		}
		public List<CharacterBundle> GetpurchasableCharacters()
		{
			return purchasableCharacters;
		}
		public CharacterDatabase GetCharacterDatabase()
		{
			return characterDatabase;
		}
#endregion
	}
}