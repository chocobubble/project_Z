using Unity.Mathematics;

namespace Data 
{
	public class BattleConstants
	{
		// positions of the player characters
		public static float3[] playerCharacterPositions = new float3[]
		{
			new float3(-1, 0, 0),
			new float3(-2, 0, 0),
			new float3(-3, 0, 0),
			new float3(-4, 0, 0),
		};

		// positions of the enemy characters
		public static float3[] enemyCharacterPositions = new float3[]
		{
			new float3(2, 0, 0),
			new float3(3, 0, 0),
			new float3(4, 0, 0),
			new float3(5, 0, 0),
		};
	}
}
