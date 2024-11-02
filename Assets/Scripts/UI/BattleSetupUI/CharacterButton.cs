using UnityEngine;
using Character;
using UnityEngine.UIElements;

namespace BattleUI
{
	public enum CharacterButtonType
	{
		PLAYER,
		PURCHASABLE
	}

	public class CharacterButton
	{
		private int buttonIndex;
		private CharacterButtonType characterButtonType;
		private Button button;
		private CharacterBundle characterBundle;

		public CharacterButton(int idx, Button button, CharacterBundle characterBundle, CharacterButtonType characterButtonType)
		{
			this.buttonIndex = idx;
			this.button = button;
			this.characterBundle = characterBundle;
			this.characterButtonType = characterButtonType;
		}

		public void Initialize() 
		{
		}

		public void UpdateCharacterButtonsText()
		{
			if (button == null || characterBundle == null)
			{
				Debug.Log("Button or characterBundle is null");
				button.text = "";
				return;
			}

			button.text = $"{characterBundle.GetCharacterId()}\n{characterBundle.GetCharacterStat().Attack} | {characterBundle.GetCharacterStat().HP}";			
		}

		public void Reset() 
		{
			button.text = "";
			characterBundle = null;
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
		public void SetCharacterBundle(CharacterBundle characterBundle)
		{
			this.characterBundle = characterBundle;
		}
		public CharacterButtonType GetCharacterButtonType()
		{
			return characterButtonType;
		}
		public int GetButtonIndex()
		{
			return buttonIndex;
		}
#endregion
	}
}