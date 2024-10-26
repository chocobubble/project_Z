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

		public void UpdateCharacters(List<GameObject> characterGameObjects)
		{
			characters.Clear();
			foreach (var characterGameObject in characterGameObjects)
			{
				if (characterGameObject == null)
				{
					Debug.LogError("Character GameObject is null");
					continue;
				}
				
				var unitController = characterGameObject.GetComponent<UnitController>();
				if (unitController != null)
				{
					characters.Add(unitController);
				}
			}
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