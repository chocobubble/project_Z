using UnityEngine;
using UnityEngine.UIElements;

public class CharacterUIController : MonoBehaviour
{
	private ProgressBar healthBar;
	private ProgressBar actGaugeBar;

	private void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;
		healthBar = root.Q<ProgressBar>("health-bar");
		actGaugeBar = root.Q<ProgressBar>("act-gauge-bar");

	}
	public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = (currentHealth / maxHealth) * 100f;
        }
    }

    public void UpdateActGaugeBar(float currentGauge, float maxGauge)
    {
        if (actGaugeBar != null)
        {
            actGaugeBar.value = (currentGauge / maxGauge) * 100f;
        }
    }
    // private Label dousedLabel;
    // private Button repositionButton;

    // private bool reposition = false; 
    
    // private void OnEnable()
    // {
    //     var root = GetComponent<UIDocument>().rootVisualElement;
    //     dousedLabel = root.Q<Label>();
    //     repositionButton = root.Q<Button>();

    //     repositionButton.clicked += OnRepositionButton;
    // }

    // private void OnRepositionButton()
    // {
    //     reposition = true;
    // }

    // public bool ShouldReposition()
    // {
    //     var temp = reposition;
    //     reposition = false;
    //     return temp;
    // }

    // public void SetNumFiresDoused(int numFiresDoused)
    // {
    //     dousedLabel.text = $"Number of fires doused: {numFiresDoused}";
    // }
}