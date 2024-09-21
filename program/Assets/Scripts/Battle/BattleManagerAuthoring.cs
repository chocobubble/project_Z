using Data;
using Unity.Entities;
using UnityEngine;

namespace Battle
{
	class BattleManagerBaker : Baker<BattleManagerAuthoring>
	{
		public override void Bake(BattleManagerAuthoring authoring)
		{
			Debug.Log("Baking BattleManager");

			// BattleManager Entity
			var battleManagerEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(battleManagerEntity, new BattleManager());
			
			// Player Entity
			var playerEntity = GetEntity(TransformUsageFlags.None);
			AddComponent(playerEntity, new PlayerBattleData());
		
			// TODO: System 로직으로 옮겨야 함
			{
				// Set up the buffer for player and enemy characters
				var playerCharacters = AddBuffer<PlayerCharacterDataBuffer>(playerEntity);

				for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
				{
					playerCharacters.Add(new PlayerCharacterDataBuffer
					{
						Value = BattleConstants.playerCharactersData[i]
					});
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
					enemyCharacters.Add(new EnemyCharacterDataBuffer
					{
						Value = BattleConstants.enemyCharactersData[i]
					});
				}
			}
		}
	}

	class BattleManagerAuthoring : MonoBehaviour
	{
	}

	public struct BattleManager : IComponentData
	{
	}

	public struct PlayerBattleData : IComponentData
	{

	}

	public struct EnemyBattleData : IComponentData
	{

	}

	public struct CharacterPositionIndex : IComponentData
	{
		public int Index;
	}

}