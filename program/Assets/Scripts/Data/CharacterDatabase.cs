using System.Collections.Generic;


namespace Data 
{
	public struct CharacterBaseData
	{
		public int Id;
		public string Name;
		public int Health;
		public int Attack;
	}
	
	public class CharacterDatabase
	{
		private List<CharacterBaseData> characterDataList = new List<CharacterBaseData>();

		public CharacterDatabase()
		{
			characterDataList.Add(new CharacterBaseData { Id = 1, Name = "Cat", Health = 5, Attack = 2 });
			characterDataList.Add(new CharacterBaseData { Id = 2, Name = "Dog", Health = 4, Attack = 3 });
			characterDataList.Add(new CharacterBaseData { Id = 3, Name = "Rabbit", Health = 3, Attack = 4 });
			characterDataList.Add(new CharacterBaseData { Id = 4, Name = "Turtle", Health = 6, Attack = 1 });
		}

		public List<CharacterBaseData> GetCharacterDataList()
		{
			return characterDataList;
		}
	}
	
}