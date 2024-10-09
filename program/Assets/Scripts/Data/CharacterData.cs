using Unity.Entities;
using Unity.Mathematics;

namespace Data
{
	public struct CharacterData
	{
		// public string Name;
		public int Id;
		public int Level;
		public int HP;
		public int Attack;

	}

	public struct PlayerCharacterDataBuffer : IBufferElementData
	{
		public CharacterData Value;
	}

	public struct EnemyCharacterDataBuffer : IBufferElementData
	{
		public CharacterData Value;
	}

	public struct PlayerCharacterPositionBuffer : IBufferElementData
	{
		public float3 Position;
	}

	public struct EnemyCharacterPositionBuffer : IBufferElementData
	{
		public float3 Position;
	}
}