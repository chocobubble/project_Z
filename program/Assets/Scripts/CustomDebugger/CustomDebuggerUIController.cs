using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Data;
using Battle;

namespace CustomDebugger 
{
public class CustomDebuggerUIController : MonoBehaviour
{
	private Label debugLabel;
	// private Dictionary<string, string> debugText = new Dictionary<string, string>();
	private string debugText = "Debug Text";
	private string stateText = "State Text";
	private string characterStatText = "Character Stat Text";
	private string turnTimerText = "Turn Timer Text";

	private void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;

		debugLabel = root.Q<Label>("debug-label");

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

  	public void UpdateCustomDebugger(CharacterData[] playerCharacterDataList, CharacterData[] enemyCharacterDataList)
	{
		characterStatText = "Player Characters:";
		foreach (var characterData in playerCharacterDataList)
		{
			characterStatText += $"{characterData.Id} ({characterData.MaxHP}, {characterData.Attack}) "; 
		}
		characterStatText += "\nEnemy Characters:";
		foreach (var characterData in enemyCharacterDataList)
		{
			characterStatText += $"{characterData.Id} ({characterData.MaxHP}, {characterData.Attack}) "; 
		}
		UpdateText();
	}

	private void UpdateText()
	{
		debugLabel.text = $"{debugText}\n{stateText}\n{characterStatText}\n{turnTimerText}"; 
	}
}
}