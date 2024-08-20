using UnityEngine;
using Character;
using System.Collections.Generic;
namespace Battle
{
	// Battle Data Container Class
	// 1. Battle Data Variables
	// 2. Initialize Battle Data
	// 3. Update Battle Data
	public class BattleDataContainer
	{
		// Singleton instance
		private static BattleDataContainer instance;
		public static BattleDataContainer Instance
		{
			get
			{
				instance ??= new BattleDataContainer();
				return instance;
			}
		}

		// Add your battle data variables here
		private List<CharacterBundle> characterBundlesToSpawn = new List<CharacterBundle>();
		public List<CharacterBundle> CharacterBundlesToSpawn
		{
			get {
				return characterBundlesToSpawn;
			}
		}

		private void Start()
		{
			// Initialize your battle data here
		}

		private void Update()
		{
			// Update your battle data here
		}
	}
}