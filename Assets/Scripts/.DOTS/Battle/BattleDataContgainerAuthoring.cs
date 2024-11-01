using UnityEngine;
using Character;
using System.Collections.Generic;
using Unity.Entities;
using Data;

namespace Battle
{
	public class BattleDataContainerAuthoring : MonoBehaviour
	{
		class Baker : Baker<BattleDataContainerAuthoring>
		{
			public override void Bake(BattleDataContainerAuthoring authoring)
			{
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				// var battleDataContainer = new BattleDataContainer();
				// AddComponentObject(entity, new BattleDataContainer
				// {
				// 	PlayerCharacters = BattleConstants.playerCharacters,
				// 	EnemyCharacters = BattleConstants.enemyCharacters
				// });

				// var playerCharacters = AddBuffer<CharacterDataBuffer>(entity);
				// var enemyCharacters = AddBuffer<CharacterDataBuffer>(entity);

				// for (int i = 0; i < BattleConstants.BATTLE_CHARACTER_COUNT; i++)
				// {
				// 	playerCharacters.Add(new CharacterDataBuffer());
				// 	enemyCharacters.Add(new CharacterDataBuffer());
				// }
			}
		}
	}
	
	// public class BattleDataContainer : IBufferElementData
	// {
	// 	public CharacterData[] PlayerCharacters;
	// 	public CharacterData[] EnemyCharacters;

	// 	// 우선 이렇게 하고 나중에 수정할 것
	// 	public int[] PlayerCharacterSpawnIndices;
	// 	public int[] EnemyCharacterSpawnIndices;

	// }
}