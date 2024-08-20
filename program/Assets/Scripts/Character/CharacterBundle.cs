using System.Collections.Generic;

namespace Character
{
	public struct CharacterBundle 
	{
		private CharacterStat characterStat;
		private string characterName;
	
		public CharacterBundle(string name, CharacterStat characterStat)
		{
			this.characterName = name;
			this.characterStat = characterStat;
		}
	
		public CharacterStat GetCharacterStat()
		{
			return characterStat;
		}
	}
}