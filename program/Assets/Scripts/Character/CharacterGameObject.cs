using UnityEngine;

namespace Character 
{
	public class CharacterGameObject : MonoBehaviour
	{
		private CharacterBundle characterBundle;
		private Sprite characterSprite;

		public CharacterGameObject(CharacterBundle characterBundle, Sprite characterSprite)
		{
			this.characterBundle = characterBundle;
			this.characterSprite = characterSprite;
		}		
	}
}