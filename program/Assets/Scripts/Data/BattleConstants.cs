using System.Data.Common;
using Unity.Mathematics;

namespace Data 
{
	public struct BattleConstants
	{
		public static int BATTLE_CHARACTER_COUNT = 4;

		// positions of the player characters
		public static float3[] playerCharacterPositions = new float3[]
		{
			new float3(-1, 0, 0),
			new float3(-3, 0, 0),
			new float3(-5, 0, 0),
			new float3(-7, 0, 0),
		};

		// positions of the enemy characters
		public static float3[] enemyCharacterPositions = new float3[]
		{
			new float3(2, 0, 0),
			new float3(4, 0, 0),
			new float3(6, 0, 0),
			new float3(8, 0, 0),
		};
	
		// player's character list for the test
		public static CharacterData[] playerCharactersData = new CharacterData[]
		{
			new CharacterData
			{
				Id = 1,
				Level = 1,
				MaxHP = 4,
				Attack = 2 
			},
			new CharacterData
			{
				Id = 2,
				Level = 1,
				MaxHP = 3,
				Attack = 1
			},
			new CharacterData
			{
				Id = 3,
				Level = 1,
				MaxHP = 1,
				Attack = 4
			},
			new CharacterData
			{
				Id = 4,
				Level = 1,
				MaxHP = 2,
				Attack = 3
			},
		};

		// enemy's character list for the test
		public static CharacterData[] enemyCharactersData = new CharacterData[]
		{
			new CharacterData
			{
				Id = 5,
				Level = 1,
				MaxHP = 4,
				Attack = 2
			},
			new CharacterData
			{
				Id = 6,
				Level = 1,
				MaxHP = 3,
				Attack = 1
			},
			new CharacterData
			{
				Id  = 7,
				Level = 1,
				MaxHP = 1,
				Attack = 4
			},
			new CharacterData
			{
				Id = 8,
				Level = 1,
				MaxHP = 2,
				Attack = 3
			},
		};
	}
}
