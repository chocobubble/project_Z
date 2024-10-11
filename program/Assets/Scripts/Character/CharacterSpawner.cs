using UnityEngine;

namespace Character 
{
	public class CharacterSpawner : MonoBehaviour
	{
		public GameObject characterPrefab;

		public CharacterSpawner(GameObject gameObject = null)
		{
			characterPrefab = gameObject;			
		}

		public GameObject SpawnCharacter(CharacterBundle characterBundle, Vector3 position)
		{
			var character = Instantiate(characterPrefab, position, Quaternion.identity);
			var UnitController = character.GetComponent<UnitController>();
			UnitController.SetCharacterBundle(characterBundle);
			return character;
		}

		public void SpawnCharacter(CharacterBundle[] characterBundles, Vector3[] positions)
		{
			if (characterBundles.Length != positions.Length)
			{
				Debug.LogError("CharacterBundle and positions length must be the same");
				return;
			}

			for (int i = 0; i < characterBundles.Length; i++) 
			{
				var character = Instantiate(characterPrefab, positions[i], Quaternion.identity);
				var characterBundle = character.GetComponent<UnitController>().GetCharacterBundle();
				characterBundle = characterBundles[i];
			}
			
		}

		public void RemoveCharacter()
		{
		}
	}
}