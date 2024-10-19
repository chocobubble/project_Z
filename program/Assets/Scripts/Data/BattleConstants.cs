using UnityEngine;

namespace Data 
{
	public struct BattleConstants
	{
		public static int BATTLE_DEFAULT_COIN = 10;
		public static int BATTLE_CHARACTER_COUNT = 4;
		public static float TURN_DURATION = 2.0f;
		public static float MOVE_SPEED = 0.1f;
		public static Vector3 ENEMY_ATTACK_POSITION = new Vector3(0.5f, 0, 0);
		public static Vector3 PLAYER_ATTACK_POSITION = new Vector3(-0.5f, 0, 0);

		// positions of the player characters
		public static Vector3[] PLAYER_CHARACTER_POSITIONS = new Vector3[]
		{
			new Vector3(-2, 0, 0),
			new Vector3(-4, 0, 0),
			new Vector3(-6, 0, 0),
			new Vector3(-8, 0, 0),
		};

		// positions of the enemy characters
		public static Vector3[] ENEMY_CHARACTER_POSITIONS = new Vector3[]
		{
			new Vector3(2, 0, 0),
			new Vector3(4, 0, 0),
			new Vector3(6, 0, 0),
			new Vector3(8, 0, 0),
		};
	
		// player's character list for the test
		public static CharacterData[] PLAYER_CHARACTERS_DATA = new CharacterData[]
		{
			new CharacterData
			{
				Id = 1,
				Level = 1,
				HP = 4,
				Attack = 2 
			},
			new CharacterData
			{
				Id = 2,
				Level = 1,
				HP = 3,
				Attack = 1
			},
			new CharacterData
			{
				Id = 3,
				Level = 1,
				HP = 1,
				Attack = 4
			},
			new CharacterData
			{
				Id = 4,
				Level = 1,
				HP = 2,
				Attack = 3
			},
		};

		// enemy's character list for the test
		public static CharacterData[] ENEMY_CHARACTERS_DATA = new CharacterData[]
		{
			new CharacterData
			{
				Id = 5,
				Level = 1,
				HP = 4,
				Attack = 2
			},
			new CharacterData
			{
				Id = 6,
				Level = 1,
				HP = 3,
				Attack = 1
			},
			new CharacterData
			{
				Id  = 7,
				Level = 1,
				HP = 1,
				Attack = 4
			},
			new CharacterData
			{
				Id = 8,
				Level = 1,
				HP = 2,
				Attack = 3
			},
		};
	}
}
