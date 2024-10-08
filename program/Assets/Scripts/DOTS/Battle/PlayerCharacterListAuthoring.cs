using Unity.Entities;
using UnityEngine;
using Data;
using Unity.VisualScripting;

namespace Battle
{
	class PlayerCharacterListAuthoring : MonoBehaviour
	{
		class Baker : Baker<PlayerCharacterListAuthoring>
		{
			public override void Bake(PlayerCharacterListAuthoring authoring)
			{
				UnityEngine.Debug.Log("Baking PlayerCharacterList");
				var entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new PlayerCharacterListComponent());
				var playerCharacterListManaged = new PlayerCharacterListManagedComponent();
				playerCharacterListManaged.PlayerCharactersData = new CharacterData[BattleConstants.BATTLE_CHARACTER_COUNT];
				AddComponentObject(entity, playerCharacterListManaged);
			}
		}
	}

	public struct PlayerCharacterListComponent : IComponentData 
	{ 

	}

	public class PlayerCharacterListManagedComponent : IComponentData 
	{
		public CharacterData[] PlayerCharactersData;
	}
}