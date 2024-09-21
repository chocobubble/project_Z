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

	private void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;

		debugLabel = root.Q<Label>("debug-label");

	}

	public void UpdateCustomDebugger(BattleState battleState, TurnPhase turnPhase)	
	{
		debugText = $"Battle State: {battleState}, Turn Phase: {turnPhase}";
		UpdateText();
	}

  	public void UpdateCustomDebugger(CharacterData[] playerCharacterDataList, CharacterData[] enemyCharacterDataList)
	{
		debugText = "Player Characters:";
		foreach (var characterData in playerCharacterDataList)
		{
			debugText += $"{characterData.Id} ({characterData.MaxHP}, {characterData.Attack}) "; 
		}
		debugText += "\nEnemy Characters:";
		foreach (var characterData in enemyCharacterDataList)
		{
			debugText += $"{characterData.Id} ({characterData.MaxHP}, {characterData.Attack}) "; 
		}
		UpdateText();
	}

	private void UpdateText()
	{
		debugLabel.text = debugText;
	}
}
}