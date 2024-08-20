using Unity.Entities;
using UnityEngine;

namespace CharacterUI 
{
	// Component to store health data for a character
	public struct HealthComponent : IComponentData
	{
		public int currentHealth;
		public int maxHealth;
	}

	// Component to store gauge data for a character
	public struct ActGaugeComponent : IComponentData
	{
		public float currentValue;
		public float maxValue;
	}

	// // MonoBehaviour to handle updating the UI based on the character's health and gauge
	// public class CharacterUIComponents : MonoBehaviour
	// {
	// 	// Reference to the character entity
	// 	public Entity characterEntity;

	// 	// Reference to the health component
	// 	public HealthComponent healthComponent;

	// 	// Reference to the gauge component
	// 	public GaugeComponent gaugeComponent;

	// 	// Update the UI based on the character's health and gauge
	// 	private void UpdateUI()
	// 	{
	// 		// TODO: Implement UI update logic here
	// 	}

	// 	// Update is called once per frame
	// 	private void Update()
	// 	{
	// 		// Check if the character entity is still valid
	// 		if (!characterEntity.Equals(Entity.Null))
	// 		{
	// 			// Get the latest health and gauge components from the entity
	// 			healthComponent = EntityManager.GetComponentData<HealthComponent>(characterEntity);
	// 			gaugeComponent = EntityManager.GetComponentData<GaugeComponent>(characterEntity);

	// 			// Update the UI
	// 			UpdateUI();
	// 		}
	// 	}
	// }
}