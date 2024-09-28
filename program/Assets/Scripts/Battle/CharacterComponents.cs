using Unity.Entities;

namespace Battle
{
	public enum CharacterOwnershipType
	{
		Player,  // 플레이어 소유 캐릭터
		Enemy,   // 적 캐릭터
		Shop     // 상점에서 구입 가능한 캐릭터
	}
	
	public struct CharacterOwnershipTypeComponent : IComponentData
	{
		public CharacterOwnershipType ownershipType;
	}

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