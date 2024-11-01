namespace Data
{
	public enum BattleState { None, Setup, Start, End, }

	public enum TurnPhase { None, Spawning, PreAttack, Attack, PostAttack, Positioning, End,}

	public enum SpawnerState
	{
		None,
		Waiting,
		Spawning,
		Complete,
	}

	public enum CharacterActionState { None, Idle, MovingToTarget, Attacking, ReturningToPosition, Stunned, Dead, Moving }
}