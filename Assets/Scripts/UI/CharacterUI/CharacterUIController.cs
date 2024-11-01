using UnityEngine;
using UnityEngine.UIElements;

public class CharacterUIController : MonoBehaviour
{
	// // private ProgressBar healthBar;
	// // private ProgressBar actGaugeBar;
	// private Label HealthLabel;
	// private Label AttackLabel;
	
	// // basic ui element 
	// private VisualElement HealthBox;
	// private VisualElement AttackBox;

	// private void OnEnable()
	// {
	// 	var root = GetComponent<UIDocument>().rootVisualElement;
	// 	HealthBox = root.Q<VisualElement>("health-box");
	// 	AttackBox = root.Q<VisualElement>("attack-box");
	// 	HealthLabel = root.Q<Label>("health-label");
	// 	AttackLabel = root.Q<Label>("attack-label");
	// }

	// private void UpdateHealth(int health)
	// {
	// 	HealthLabel.text = health.ToString();
 	// }

	// private void UpdateAttack(int attack)
	// {
	// 	AttackLabel.text = attack.ToString();
	// }

	// public void	UpdateUI(int health, int attack)
	// {
	// 	UpdateHealth(health);
	// 	UpdateAttack(attack);
	// }

	// // public void UpdateHealthBar(float currentHealth, float maxHealth)
    // // {
    // //     if (healthBar != null)
    // //     {
    // //         healthBar.value = (currentHealth / maxHealth) * 100f;
    // //     }
    // // }

    // // public void UpdateActGaugeBar(float currentGauge, float maxGauge)
    // // {
    // //     if (actGaugeBar != null)
    // //     {
    // //         actGaugeBar.value = (currentGauge / maxGauge) * 100f;
    // //     }
    // // }
    // // private Label dousedLabel;
    // // private Button repositionButton;

    // // private bool reposition = false; 
    
    // // private void OnEnable()
    // // {
    // //     var root = GetComponent<UIDocument>().rootVisualElement;
    // //     dousedLabel = root.Q<Label>();
    // //     repositionButton = root.Q<Button>();

    // //     repositionButton.clicked += OnRepositionButton;
    // // }

    // // private void OnRepositionButton()
    // // {
    // //     reposition = true;
    // // }

    // // public bool ShouldReposition()
    // // {
    // //     var temp = reposition;
    // //     reposition = false;
    // //     return temp;
    // // }

    // // public void SetNumFiresDoused(int numFiresDoused)
    // // {
    // //     dousedLabel.text = $"Number of fires doused: {numFiresDoused}";
    // // }
}