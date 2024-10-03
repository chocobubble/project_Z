using UnityEngine;
using UnityEngine.UIElements;

namespace BattleUI
{
	public class BattleSetupUIController : MonoBehaviour
	{
		private Label coinLabel;
		private Button rerollButton;
		private Button setupEndButton;

		private void OnEnable()
		{
			var root = GetComponent<UIDocument>().rootVisualElement;

			coinLabel = root.Q<Label>("coin-label");
			rerollButton = root.Q<Button>("reroll-button");
			setupEndButton = root.Q<Button>("setup-end-button");

			rerollButton.clicked += Reroll;
			setupEndButton.clicked += SetupEnd;
		}

		private void Reroll()
		{
			Debug.Log("Rerolling Button Clicked");
		}

		private void SetupEnd()
		{
			Debug.Log("Setup End Button Clicked");
		}

		public void UpdateCoin(int coin)
		{
			// Debug.Log("Update Coin Label Text");
			coinLabel.text = $"Coin: {coin}";
		}
	}
}