using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Battle
{
	public class CharactersManager 
	{
		private List<UnitController> characters;
		
		public CharactersManager(List<UnitController> characters)
		{
			this.characters = characters;
		}

		public UnitController GetUnitController(int index)
		{
			if (index < 0 || index >= characters.Count)
			{
				Debug.LogError("Invalid index: " + index);
				return null;
			}
			return characters[index];
		}
	}
}