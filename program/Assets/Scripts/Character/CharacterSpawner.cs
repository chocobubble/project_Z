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
				var characterBundle = character.GetComponent<CharacterController>().GetCharacterBundle();
				characterBundle = characterBundles[i];
			}
			
		}

		public void RemoveCharacter()
		{
		}
	}
}