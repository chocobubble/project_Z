// using PlayerInput;
// using Unity.Burst;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public partial struct DragSystem : ISystem 
// {
//     // private Camera _mainCamera;
//     private Entity _selectedEntity;
//     private bool _isDragging;
    
// 	[BurstCompile]
// 	public void OnCreate(ref SystemState state)
//     {
//         // _mainCamera = Camera.main;
//         _isDragging = false;
// 		state.RequireForUpdate<Hit>();
//     }


// 	public void OnUpdate(ref SystemState state)
//     {
// 		var hit = SystemAPI.GetSingletonRW<Hit>();
		
//         // 마우스 입력 감지
//         if (Mouse.current.leftButton.wasPressedThisFrame)
//         {
//             // 마우스 클릭 시 Raycast를 쏴서 오브젝트 선택
//             RaycastForEntity();
//         }

//         if (Mouse.current.leftButton.isPressed && hit.ValueRO.IsDragging)
//         {
//             // 마우스를 드래그 중인 경우 오브젝트 위치 업데이트
//             MoveSelectedEntity(ref state);
//         }

//         if (Mouse.current.leftButton.wasReleasedThisFrame)
//         {
//             // 마우스 버튼을 뗐을 때 드래그 종료
//             // _isDragging = false;
// 			hit.ValueRW.IsDragging = false;
//             _selectedEntity = Entity.Null;
//         }
//     }

//     private void RaycastForEntity()
//     {
//         // // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
//         // Vector2 mousePosition = Mouse.current.position.ReadValue();
//         // Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

//         // // PhysicsWorld에서 Ray를 쏴서 충돌 감지
//         // var collisionWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;
//         // var rayInput = new RaycastInput
//         // {
//         //     Start = worldPosition,
//         //     End = worldPosition + new Vector3(0, 0, 1),
//         //     Filter = CollisionFilter.Default
//         // };

//         // if (collisionWorld.CastRay(rayInput, out Unity.Physics.RaycastHit hit))
//         // {
//         //     _selectedEntity = hit.Entity;
//         //     _isDragging = true;
//         // }
//     }

//     private void MoveSelectedEntity(ref SystemState state)
//     {
//         if (_selectedEntity == Entity.Null) return;

//         // 마우스 위치를 월드 좌표로 변환하여 오브젝트 위치 이동
//         Vector2 mousePosition = Mouse.current.position.ReadValue();
//         Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

//         // 선택된 엔티티의 Translation을 업데이트하는 잡
//         var job = new MoveEntityJob
//         {
//             SelectedEntity = _selectedEntity,
//             WorldPosition = new float3(worldPosition.x, worldPosition.y, 0f)
//         };

//         job.ScheduleParallel();
//     }
// }

// [BurstCompile]
// public partial struct MoveEntityJob : IJobEntity
// {
//     public Entity SelectedEntity;
//     public float3 WorldPosition;

//     public void Execute(ref LocalTransform localTransform, in Entity entity)
//     {
//         if (entity == SelectedEntity)
//         {
//             localTransform.Position = WorldPosition;
//         }
//     }
// }