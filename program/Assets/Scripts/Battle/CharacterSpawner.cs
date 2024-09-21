using Unity.Entities;
using Unity.Mathematics;
using Data;
using System.Collections.Generic;
using Character;
using Unity.Collections;

namespace Battle
{


	// Component to store spawner data
	public struct CharacterSpawner : IComponentData
	{
		// private Entity characterPrefab;
		// private SpawnerStatus status;
		// public SpawnerStatus Status
		// {
		// 	get {
		// 		return status;
		// 	}
		// 	set {
		// 		status = value;
		// 	}
		// }
		// private float3[] PlayerCharacterSpawnPositions
		// {
		// 	get {
		// 		return Data.BattleConstants.playerCharacterPositions;
		// 	}
		// }
		// private float3[] EnemyCharacterSpawnPositions
		// {
		// 	get {
		// 		return Data.BattleConstants.enemyCharacterPositions;
		// 	}
		// }
		// public int PlayerCharacterSpawnIndex;
		// public int EnemyCharacterSpawnIndex;

		// public CharacterData[] playerCharacters
		// {
		// 	get {
		// 		return Data.BattleConstants.playerCharacters;
		// 	}
		// }

		// public CharacterData[] enemyCharacters
		// {
		// 	get {
		// 		return Data.BattleConstants.enemyCharacters;
		// 	}
		// }
		
		// Remove the parameterless constructor

		// public bool IsPlayerCharacterSpawnIndexValid()
		// {
		// 	return PlayerCharacterSpawnIndex >= 0 && PlayerCharacterSpawnIndex < PlayerCharacterSpawnPositions.Length;
		// }
		// public float3 GetNextPlayerCharacterSpawnPosition()
		// {
		// 	var position = PlayerCharacterSpawnPositions[PlayerCharacterSpawnIndex];
		// 	PlayerCharacterSpawnIndex = (PlayerCharacterSpawnIndex + 1);
		// 	return position;
		// }
		// public bool IsEnemyCharacterSpawnIndexValid()
		// {
		// 	return EnemyCharacterSpawnIndex >= 0 && EnemyCharacterSpawnIndex < EnemyCharacterSpawnPositions.Length;
		// }
		// public float3 GetNextEnemyCharacterSpawnPosition()
		// {
		// 	var position = EnemyCharacterSpawnPositions[EnemyCharacterSpawnIndex];
		// 	EnemyCharacterSpawnIndex = (EnemyCharacterSpawnIndex + 1);
		// 	return position;
		// }
		
		public Entity CharacterPrefab;

		// public Entity GetCharacterPrefab()
		// {
		// 	return characterPrefab;
		// }
	}
}