using UnityEngine;
using Character;
using UnityEngine.UIElements;

namespace BattleUI
{
	public class CharacterButton
	{
		private Button button;
		private CharacterBundle characterBundle;

		public CharacterButton(Button button, CharacterBundle characterBundle)
		{
			this.button = button;
			this.characterBundle = characterBundle;
		}	

		public void Initialize() 
		{
		}

		public void UpdateCharacterButtonsText()
		{
			if (button == null || characterBundle == null)
			{
				Debug.Log("Button or characterBundle is null");
				return;
			}

			button.text = $"{characterBundle.GetCharacterId()}\n{characterBundle.GetCharacterStat().Attack} | {characterBundle.GetCharacterStat().HP}";			
		}

#region Get Set
		public Button GetButtonUI()
		{
			return button;
		}
		public CharacterBundle GetCharacterBundle()
		{
			return characterBundle;
		}
#endregion
	}
}