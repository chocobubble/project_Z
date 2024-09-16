using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Data;

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

  	public void UpdateCustomDebugger(CharacterData[] characterDataList)
	{
		debugText = "";
		foreach (var characterData in characterDataList)
		{
			debugText += $"{characterData.Id} : {characterData.MaxHP}, {characterData.Attack}\n";
		}
		UpdateText();
	}

	private void UpdateText()
	{
		debugLabel.text = debugText;
	}
}
}