using Unity.Entities;

namespace Battle
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
		public float regenRate;
		public ActGaugeComponent(float regenRate)
		{
			this.currentValue = 0.0f;
			this.maxValue = 100.0f;
			this.regenRate = regenRate;
		}
	}
}