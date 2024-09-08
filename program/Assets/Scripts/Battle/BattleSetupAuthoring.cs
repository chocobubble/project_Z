using Data;
using Unity.Entities;
using UnityEngine;
namespace Battle
{
	class BattleSetupAuthoring : MonoBehaviour
	{
	
	}

	class BattleSetupBaker : Baker<BattleSetupAuthoring>
	{
		public override void Bake(BattleSetupAuthoring authoring)
		{
			UnityEngine.Debug.Log("Baking BattleSetup");

			// BattleSetup Entity
			var battleSetupEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(battleSetupEntity, new BattleSetup());
			
			// Player Entity
			var playerEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(playerEntity, new PlayerBattleData());
		
			// Set up the buffer for player and enemy characters
			var playerCharacters = AddBuffer<PlayerCharacterDataBuffer>(playerEntity);

			for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			{
				playerCharacters.Add(new PlayerCharacterDataBuffer());
			}

			var playerCharacterPositions = AddBuffer<PlayerCharacterPositionBuffer>(playerEntity);

			for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			{
				playerCharacterPositions.Add(new PlayerCharacterPositionBuffer
				{
					Position = BattleConstants.playerCharacterPositions[i]
				});
			}

			var enemyEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(enemyEntity, new EnemyBattleData());
			var enemyCharacters = AddBuffer<EnemyCharacterDataBuffer>(enemyEntity);
			for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			{
				enemyCharacters.Add(new EnemyCharacterDataBuffer());
			}

			var enemyCharacterPositions = AddBuffer<EnemyCharacterPositionBuffer>(enemyEntity);
			for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
			{
				enemyCharacterPositions.Add(new EnemyCharacterPositionBuffer
				{
					Position = BattleConstants.enemyCharacterPositions[i]
				});
			}
		}
	}

	public struct PlayerBattleData : IComponentData
	{

	}

	public struct EnemyBattleData : IComponentData
	{

	}
}