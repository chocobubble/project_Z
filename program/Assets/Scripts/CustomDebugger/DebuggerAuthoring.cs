using Unity.Entities;
using UnityEngine;

namespace CustomDebugger
{
	public class DebuggerAuthoring : MonoBehaviour
	{
		[Header("Debugger Prefab")]
		public GameObject DebuggerPrefab;

		class Baker : Baker<DebuggerAuthoring>
		{
			public override void Bake(DebuggerAuthoring authoring)
			{
				// var entity = GetEntity(authoring, TransformUsageFlags.None);
				// AddComponent(entity, new DebuggerConfig());
				// var debugger = new Debugger();
				// AddComponentObject(entity, debugger);
				// AddComponent(entity, new Debugger
				// {
				// 	DebuggerPrefab = GetEntity(authoring.DebuggerPrefab, TransformUsageFlags.None)
				// });

			}
		}
	}

	public class Debugger : IComponentData
	{
		public DebugUIController DebugUIController;
	}

	public struct DebuggerConfig : IComponentData
	{
	}

	public struct DebuggerContent : IComponentData
	{
		// public string TextKey;
		// public string TextValue;
	}
}