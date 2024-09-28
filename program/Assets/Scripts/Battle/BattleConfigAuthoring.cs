using Unity.Entities;
using UnityEngine;
using Data;
using Unity.Mathematics;

namespace Battle
{
	public class BattleConfigAuthoring : MonoBehaviour
	{
		[Header("Character Prefabs")]
		public GameObject CharacterPrefab;

		class Baker : Baker<BattleConfigAuthoring>
		{
			public override void Bake(BattleConfigAuthoring authoring)
			{
				Debug.Log("Baking BattleConfig");
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new BattleConfig
				{
					CharacterPrefab = GetEntity(authoring.CharacterPrefab, TransformUsageFlags.None),
					TurnCount = 0,
					TurnDurationSeconds = 3.0f,
					ShouldCharactersPositionUpdate = false,
					CharacterMovementSpeed = BattleConstants.MOVE_SPEED,
					IsSpawnFinished = false,
					IsPreAttackFinished = false,
					IsAttackFinished = false,
					IsPostAttackFinished = false
					
				});
				AddComponent(entity, new BattleStateComponent
				{
					BattleState = BattleState.None
				});
				AddComponent(entity, new TurnPhaseComponent
				{
					TurnPhase = TurnPhase.None
				});
				var battleConfigManaged = new BattleConfigManaged();
				battleConfigManaged.PlayerCharacterPositions = BattleConstants.PLAYER_CHARACTER_POSITIONS;
				battleConfigManaged.EnemyCharacterPositions = BattleConstants.ENEMY_CHARACTER_POSITIONS;
				AddComponentObject(entity, battleConfigManaged); 
				
			}
		}
	}

	public struct BattleConfig : IComponentData
	{
		public Entity CharacterPrefab;
		public int TurnCount;
		public float TurnDurationSeconds;
		public bool ShouldCharactersPositionUpdate;
		public float CharacterMovementSpeed;

		// Turn
		// TODO : count 하는 방식으로 바꿀 수 있지 않을까?
		public bool IsSpawnFinished;
		public bool IsPreAttackFinished;
		public bool IsAttackFinished;
		public bool IsPostAttackFinished;
	}

	public class BattleConfigManaged : IComponentData
	{
		public float3[] PlayerCharacterPositions = new float3[4];
		public float3[] EnemyCharacterPositions = new float3[4];
		public BattleConfigManaged()
		{

		}
	}
		
	public enum BattleState  { None, Setup, Start, End, }

	public enum TurnPhase { None, Spawning, PreAttack, Attack, PostAttack, }

		// Component to store battle setup data
	public struct BattleStateComponent : IComponentData
	{
		private BattleState battleState;
		public BattleState BattleState
		{
			get {
				// Debug.Log("Getting BattleSetupStatus as " + battleSetupStatus);
				return battleState;
			}
			set {
				// Debug.Log("Setting BattleState to " + value);
				battleState = value;
			}
		}
	}

	public struct TurnPhaseComponent : IComponentData
	{
		private TurnPhase turnPhase;
		public TurnPhase TurnPhase
		{
			get {
				// Debug.Log("Getting BattleSetupStatus as " + battleSetupStatus);
				return turnPhase;
			}
			set {
				// Debug.Log("Setting BattleState to " + value);
				turnPhase = value;
			}
		}
	}
}