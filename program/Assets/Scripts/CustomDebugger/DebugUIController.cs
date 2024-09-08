using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace CustomDebugger 
{
public class DebugUIController : MonoBehaviour
{
	// private Label debugLabel;
	// private Dictionary<string, string> debugText = new Dictionary<string, string>();

	// private void OnEnable()
	// {
	// 	var root = GetComponent<UIDocument>().rootVisualElement;

	// 	debugLabel = root.Q<Label>("debug-label");

	// }

  	// public void UpdateDebug(string textKey, string textValue)
	// {
	// 	if (debugText.ContainsKey(textKey))
	// 	{
	// 		debugText[textKey] = textValue;
	// 	}
	// 	else
	// 	{
	// 		debugText.Add(textKey, textValue);
	// 	}
	// 	UpdateText();
	// }

	// private void UpdateText()
	// {
	// 	debugLabel.text = "";
	// 	foreach (var item in debugText)
	// 	{
	// 		debugLabel.text += $"{item.Key}: {item.Value}\n";
	// 	}
	// }
}
}