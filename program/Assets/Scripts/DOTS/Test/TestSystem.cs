using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Project.Test
{
	[BurstCompile]
	public partial struct TestSystem : ISystem 
	{
		bool isInitialized;

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<TestPrefabComponent>();

			isInitialized = false;
		}

		public void OnUpdate(ref SystemState state)
		{
			if (!isInitialized)
			{
				isInitialized = true;
				
				var TestPrefabComponent = SystemAPI.GetSingleton<TestPrefabComponent>();
				var testEntity = state.EntityManager.Instantiate(TestPrefabComponent.Prefab);
				var transform = SystemAPI.GetComponentRW<LocalTransform>(testEntity);
				transform.ValueRW.Position = new float3(0, 5, 0);



			}
		}
	}
}