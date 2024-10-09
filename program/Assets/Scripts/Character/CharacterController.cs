using Unity.VisualScripting;
using UnityEngine;

namespace Character 
{
	public class CharacterController : MonoBehaviour
	{
		private CharacterBundle _characterBundle;

		void Start()
		{
			_characterBundle = new CharacterBundle();
		}

		void Update()
		{

		}



		public void SetCharacterBundle(CharacterBundle characterBundle)
		{
			// Deep copy
			_characterBundle = new CharacterBundle(characterBundle);
		}

		public CharacterBundle GetCharacterBundle()
		{
			return _characterBundle;
		}
	}
}