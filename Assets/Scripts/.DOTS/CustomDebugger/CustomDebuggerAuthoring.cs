using Unity.Entities;
using UnityEngine;

namespace CustomDebugger
{
	public class CustomDebuggerAuthoring : MonoBehaviour
	{
		[Header("Debugger Prefab")]
		public GameObject DebuggerPrefab;

		class Baker : Baker<CustomDebuggerAuthoring>
		{
			public override void Bake(CustomDebuggerAuthoring authoring)
			{
				var entity = GetEntity(authoring, TransformUsageFlags.None);
				AddComponent(entity, new CustomDebuggerConfig());
				var configManaged = new CustomDebuggerConfigManaged();
				AddComponentObject(entity, configManaged);
			}
		}
	}

	public class Debugger : IComponentData
	{
		// public DebugUIController DebugUIController;
	}

	public struct CustomDebuggerConfig : IComponentData
	{
	}

	public struct DebuggerContent : IComponentData
	{
		// public string TextKey;
		// public string TextValue;
	}

	public class CustomDebuggerConfigManaged : IComponentData
	{
		public CustomDebuggerUIController CustomDebuggerUIController;
	}
}