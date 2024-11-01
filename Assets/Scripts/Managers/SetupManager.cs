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
		private List<GameObject> purchaseableCharacters;

		void Start()
		{

		}

		void OnEnable()
		{
			purchaseableCharacters = new List<GameObject>();
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

			// TODO: 임시
			var characterDataList = characterDatabase.GetCharacterDataList();
			for (int i = 0; i < 4 ; i++)
			{
				var characterBundle = new CharacterBundle(characterDataList[i]);
				var characterGameObject = characterSpawner.SpawnCharacter(characterBundle, BattleConstants.PURCHASAEALBE_CHARACTER_POSITIONS[i]);
				purchaseableCharacters.Add(characterGameObject);
			}
		}


		public void SetCharacterDatabase(CharacterDatabase characterDatabase)
		{
			this.characterDatabase = characterDatabase;
		}
		public void SetCharacterSpawner(CharacterSpawner characterSpawner)
		{
			this.characterSpawner = characterSpawner;
		}
		public List<GameObject> GetPurchaseableCharacters()
		{
			return purchaseableCharacters;
		}

	}
}