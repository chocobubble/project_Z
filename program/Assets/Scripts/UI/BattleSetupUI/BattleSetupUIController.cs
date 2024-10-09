using UnityEngine;
using UnityEngine.UIElements;

namespace BattleUI
{
	public class BattleSetupUIController : MonoBehaviour
	{
		public bool isSetupEnd;
		private Label coinLabel;
		private Button rerollButton;
		private Button setupEndButton;

		public GameObject battleManager;

		private void OnEnable()
		{
			var root = GetComponent<UIDocument>().rootVisualElement;

			coinLabel = root.Q<Label>("coin-label");
			rerollButton = root.Q<Button>("reroll-button");
			setupEndButton = root.Q<Button>("setup-end-button");

			rerollButton.clicked += Reroll;
			setupEndButton.clicked += SetupEnd;

			isSetupEnd = false;
		}

		private void Reroll()
		{
			Debug.Log("Rerolling Button Clicked");
		}

		private void SetupEnd()
		{
			Debug.Log("Setup End Button Clicked");
			isSetupEnd = true;

			// TODO : battle scene 켜지게
			if (battleManager != null)
			{
				battleManager.SetActive(true);
			}
			DeActivateUI();
		}

		public void UpdateCoin(int coin)
		{
			// Debug.Log("Update Coin Label Text");
			coinLabel.text = $"Coin: {coin}";
		}

		public void DeActivateUI()
		{
			Debug.Log("Deactivating BattleSetupUI");
			gameObject.SetActive(false);
		}
	}
}