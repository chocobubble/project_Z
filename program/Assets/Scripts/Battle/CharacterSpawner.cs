using Unity.Entities;
using Unity.Mathematics;
using Data;
using System.Collections.Generic;
using Character;
using Unity.Collections;

namespace Battle
{
	public enum SpawnerStatus
	{
		None,
		Waiting,
		Spawning
	}

	// Component to store spawner data
	public struct CharacterSpawner : IComponentData
	{
		private Entity characterPrefab;
		private SpawnerStatus status;
		public SpawnerStatus Status
		{
			get {
				return status;
			}
			set {
				status = value;
			}
		}
		private float3[] PlayerCharacterSpawnPositions
		{
			get {
				return Data.BattleConstants.playerCharacterPositions;
			}
		}
		private float3[] EnemyCharacterSpawnPositions
		{
			get {
				return Data.BattleConstants.enemyCharacterPositions;
			}
		}
		public int PlayerCharacterSpawnIndex;
		public int EnemyCharacterSpawnIndex;
		// private NativeList<CharacterBundle> characterBundlesToSpawn;
		// public NativeList<CharacterBundle> CharacterBundlesToSpawn
		// {
		// 	get {
		// 		return characterBundlesToSpawn;
		// 	}
		// }
		public CharacterSpawner(Entity characterPrefab)
		{
			this.characterPrefab = characterPrefab;
			
			status = SpawnerStatus.None;
			this.PlayerCharacterSpawnIndex = 0;
			this.EnemyCharacterSpawnIndex = 0;
		}

		public bool IsPlayerCharacterSpawnIndexValid()
		{
			return PlayerCharacterSpawnIndex >= 0 && PlayerCharacterSpawnIndex < PlayerCharacterSpawnPositions.Length;
		}
		public float3 GetNextPlayerCharacterSpawnPosition()
		{
			var position = PlayerCharacterSpawnPositions[PlayerCharacterSpawnIndex];
			PlayerCharacterSpawnIndex = (PlayerCharacterSpawnIndex + 1);
			return position;
		}
		public bool IsEnemyCharacterSpawnIndexValid()
		{
			return EnemyCharacterSpawnIndex >= 0 && EnemyCharacterSpawnIndex < EnemyCharacterSpawnPositions.Length;
		}
		public float3 GetNextEnemyCharacterSpawnPosition()
		{
			var position = EnemyCharacterSpawnPositions[EnemyCharacterSpawnIndex];
			EnemyCharacterSpawnIndex = (EnemyCharacterSpawnIndex + 1);
			return position;
		}
		public Entity GetCharacterPrefab()
		{
			return characterPrefab;
		}
	}
}