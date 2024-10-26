using System.Collections.Generic;
using Data;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

namespace Character
{
	public class CharacterBundle
	{
		private int characterId;
		private CharacterStat characterStat;
		private string characterName;
	
		public CharacterBundle()
		{
			this.characterId = 0;
			this.characterName = "";
			this.characterStat = new CharacterStat(0, 0, 0);
		}
		public CharacterBundle(CharacterBundle other)
		{
			this.characterId = other.characterId;
			this.characterName = other.characterName;
			this.characterStat = new CharacterStat(other.characterStat);
		}
		public CharacterBundle(CharacterData characterData)
		{
			this.characterId = characterData.Id;
			this.characterName = "";
			this.characterStat = new CharacterStat(characterData.Level, characterData.HP, characterData.Attack);
		}

		public CharacterBundle(int id, string name, CharacterStat characterStat)
		{
			this.characterId = id;
			this.characterName = name;
			this.characterStat = characterStat;
		}

		public CharacterBundle(CharacterBaseData characterBaseData)
		{
			this.characterId = characterBaseData.Id;
			this.characterName = characterBaseData.Name;
			this.characterStat = new CharacterStat(1, characterBaseData.Health, characterBaseData.Attack);
		}
	
		public CharacterStat GetCharacterStat()
		{
			return characterStat;
		}

		public int GetCharacterId()
		{
			return characterId;
		}

		public string GetCharacterName()
		{
			return characterName;
		}
	}
}