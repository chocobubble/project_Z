using System.Collections.Generic;
using Character;
using Unity.Collections;
using Unity.Entities;

namespace PlayerData 
{
	public class PlayerDataContainer 
	{
		// Singleton instance
		private static PlayerDataContainer instance;
		public static PlayerDataContainer Instance
		{
			get
			{
				instance ??= new PlayerDataContainer();
				return instance;
			}
		}
		private List<CharacterBundle> characterBundles;

		private PlayerDataContainer()
		{
			characterBundles = new List<CharacterBundle>();

			// For prototype purposes, add some default character bundles
			characterBundles.Add(new CharacterBundle("Player1", new CharacterStat()));
			characterBundles.Add(new CharacterBundle("Player2", new CharacterStat()));
			characterBundles.Add(new CharacterBundle("Player3", new CharacterStat()));
		}

		public void AddCharacterBundle(CharacterBundle characterBundle)
		{
			characterBundles.Add(characterBundle);
		}

		public void RemoveCharacterBundle(CharacterBundle characterBundle)
		{
			characterBundles.Remove(characterBundle);
		}

		public List<CharacterBundle> GetCharacterBundles()
		{
			return characterBundles;
		}
	}
}
