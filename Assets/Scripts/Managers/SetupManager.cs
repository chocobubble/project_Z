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
		private List<CharacterBundle> purchaseableCharacters;

		void Start()
		{

		}

		void OnEnable()
		{
			purchaseableCharacters = new List<CharacterBundle>();
			// SpawnPurchaseableCharacters();
		}

		void Update()
		{
		}

		private void SpawnPurchaseableCharacters()
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
		public void SetPurchaseableCharacters(List<CharacterBundle> purchaseableCharacters)
		{
			this.purchaseableCharacters = purchaseableCharacters;
		}
		public List<CharacterBundle> GetPurchaseableCharacters()
		{
			return purchaseableCharacters;
		}
		public CharacterDatabase GetCharacterDatabase()
		{
			return characterDatabase;
		}
#endregion
	}
}