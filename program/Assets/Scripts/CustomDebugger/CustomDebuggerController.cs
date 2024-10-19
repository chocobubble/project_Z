using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Data;
using Battle;
using Character;

namespace CustomDebugger 
{
public class CustomDebuggerUIController : MonoBehaviour
{
	private Label debugLabel;
	private string debugText = "Debug Text";
	private string stateText = "State Text";
	private string characterStatText = "Character Stat Text";
	private string turnTimerText = "Turn Timer Text";

	private TurnManager turnManager;
	private BattleManager battleManager;
	public TurnManager TurnManager 
	{
		get 
		{ 
			if (turnManager == null)
			{
				var turnManagerGameObject = GameObject.Find("TurnManager");
				if (turnManagerGameObject == null) return null;
				turnManager = turnManagerGameObject.GetComponent<TurnManager>();

			}
			return turnManager; 
		}
		set { turnManager = value; }
	}
	public BattleManager BattleManager 
	{
		get 
		{ 
			if (battleManager == null)
			{
				var battleManagerGameObject = GameObject.Find("BattleManager");
				if (battleManagerGameObject == null) return null;
				battleManager = battleManagerGameObject.GetComponent<BattleManager>();
			}
			return battleManager; 
		}
		set { battleManager = value; }
	}

	private void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;

		debugLabel = root.Q<Label>("custom-debugger-label");

	}

	public void Start() 
	{
		if (turnManager == null)
			turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
	}

	public void Update()
	{
		UpdateStates();
		UpdateCustomDebugger();
		UpdateText();
	}

	public void UpdateCustomDebugger() 
	{
		if (TurnManager == null)
			return;

		characterStatText = "Player Characters:";
		for (int i = 0; i < TurnManager.PlayerCharacters.Count; i++)
		{
			if (TurnManager.PlayerCharacters[i] == null)
				continue;
			var characterStat = TurnManager.PlayerCharacters[i].GetComponent<UnitController>().CharacterStat;
			characterStatText += $"idx : {i} | stat : ({characterStat.HP}, {characterStat.Attack}) "; 
		}
		characterStatText += "\nEnemy Characters:";
		for (int i = 0; i < TurnManager.EnemyCharacters.Count; i++)
		{
			if (TurnManager.EnemyCharacters[i] == null)
				continue;
			var characterData = TurnManager.EnemyCharacters[i].GetComponent<UnitController>().CharacterStat;
			characterStatText += $"idx : {i} | stat : ({characterData.HP}, {characterData.Attack}) "; 
		}
	}

	public void UpdateStates() 
	{
		if (BattleManager == null || TurnManager == null)
			return;
		stateText = $"Battle State: {BattleManager.CurrentBattleState}, Turn Phase: {TurnManager.CurrentTurnPhase}";
	}

  	public void UpdateCustomDebugger(CharacterData[] playerCharacterDataList, CharacterData[] enemyCharacterDataList)
	{
		characterStatText = "Player Characters:";
		foreach (var characterData in playerCharacterDataList)
		{
			characterStatText += $"{characterData.Id} ({characterData.HP}, {characterData.Attack}) "; 
		}
		characterStatText += "\nEnemy Characters:";
		foreach (var characterData in enemyCharacterDataList)
		{
			characterStatText += $"{characterData.Id} ({characterData.HP}, {characterData.Attack}) "; 
		}
		UpdateText();
	}

	private void UpdateText()
	{
		debugLabel.text = $"{debugText}\n{stateText}\n{characterStatText}\n{turnTimerText}"; 
	}


	public void UpdateTurnTimerDebugger(float turnDurationTime, float lastTurnPhaseTime)
	{
		turnTimerText = $"Turn Duration Time: {turnDurationTime}\n Last Turn Phase Time: {lastTurnPhaseTime}";
		UpdateText();
	}
	public void UpdateCustomDebugger(BattleState battleState, TurnPhase turnPhase)	
	{
		stateText = $"Battle State: {battleState}, Turn Phase: {turnPhase}";
		UpdateText();
	}
}
}