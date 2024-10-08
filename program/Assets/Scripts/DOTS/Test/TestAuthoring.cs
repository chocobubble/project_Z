using Unity.Entities;
using UnityEngine;


namespace Project.Test
{
	public class TestAuthoring : MonoBehaviour
	{
		public GameObject Prefab;
		public uint Size;
		public float Radius;
	}
	class TestBaker : Baker<TestAuthoring>
	{
		public override void Bake(TestAuthoring authoring)
		{
			var entity = GetEntity(TransformUsageFlags.None);

			AddComponent(entity, new TestPrefabComponent {
				Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
				Size = authoring.Size,
				Radius = authoring.Radius
			});
		}
	}	

	public struct TestPrefabComponent : IComponentData
	{
		public Entity Prefab;
		public uint Size;
		public float Radius;
	}
}