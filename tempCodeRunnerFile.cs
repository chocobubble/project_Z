using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using NSubstitute;
using System.Collections.Generic;
using Battle;
using Character;
using Setup;

namespace BattleUITests
{
	public class BattleSetupUIControllerTest
	{
		private BattleSetupUIController controller;
		private BattleManager battleManager;
		private SetupManager setupManager;
		private UIDocument uiDocument;
		private VisualElement root;

		[SetUp]
		public void SetUp()
		{
			// Create a GameObject and add the BattleSetupUIController component
			var gameObject = new GameObject();
			controller = gameObject.AddComponent<BattleSetupUIController>();

			// Mock dependencies
			battleManager = Substitute.For<BattleManager>();
			setupManager = Substitute.For<SetupManager>();
			controller.battleManager = battleManager;
			controller.setupManager = setupManager;

			// Mock UI Document and root element
			uiDocument = gameObject.AddComponent<UIDocument>();
			root = new VisualElement();
			uiDocument.rootVisualElement = root;

			// Mock buttons
			var button1 = new Button();
			var button2 = new Button();
			var button3 = new Button();
			var button4 = new Button();
			root.Add(button1);
			root.Add(button2);
			root.Add(button3);
			root.Add(button4);

			controller.playerCharacterButton1 = button1;
			controller.playerCharacterButton2 = button2;
			controller.playerCharacterButton3 = button3;
			controller.playerCharacterButton4 = button4;

			// Mock CharacterBundles
			var characterBundles = new List<CharacterBundle>
			{
				new CharacterBundle(new CharacterData { Id = "1" }),
				new CharacterBundle(new CharacterData { Id = "2" }),
				new CharacterBundle(new CharacterData { Id = "3" }),
				new CharacterBundle(new CharacterData { Id = "4" })
			};
			battleManager.GetPlayerCharacterBundles().Returns(characterBundles);
		}

		[Test]
		public void InitializePlayerCharacterButtons_ShouldInitializeButtonsCorrectly()
		{
			// Act
			controller.InitializePlayerCharacterButtons();

			// Assert
			Assert.NotNull(controller.playerCharacterButtons);
			Assert.AreEqual(4, controller.playerCharacterButtons.Count);
			for (int i = 0; i < 4; i++)
			{
				Assert.NotNull(controller.playerCharacterButtons[i]);
				Assert.AreEqual(controller.playerCharacterButtons[i].GetCharacterBundle().GetCharacterId(), (i + 1).ToString());
			}
		}

		[Test]
		public void InitializePlayerCharacterButtons_ShouldSetSelectedCharacterBundleOnClick()
		{
			// Arrange
			controller.InitializePlayerCharacterButtons();
			var button = controller.playerCharacterButtons[0].GetButtonUI();
			var characterBundle = controller.playerCharacterButtons[0].GetCharacterBundle();

			// Act
			button.clicked.Invoke();

			// Assert
			Assert.AreEqual(controller.selectedCharacterBundle, characterBundle);
		}
	}
}