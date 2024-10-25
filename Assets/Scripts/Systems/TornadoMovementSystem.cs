using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct TornadoMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        // create a Command buffer for Destroy the entity 
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        //adding with WithEntityAccess to get the entity from the query 
        foreach (var (transform, movement, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<TornadoMovementData>>().WithEntityAccess())
        {
            movement.ValueRW.angle  += movement.ValueRO.speed * deltaTime;
            movement.ValueRW.height += movement.ValueRO.heightSpeed * deltaTime;
            
            var x = math.cos(movement.ValueRO.angle) * (movement.ValueRO.radius );
            var z = math.sin(movement.ValueRO.angle) * (movement.ValueRO.radius );
            var y = movement.ValueRO.height;
            
            float3 newPosition = new float3(x, y, z);
            
            transform.ValueRW.Position = newPosition;

            if (transform.ValueRW.Position.y > 50)
            {
                ecb.DestroyEntity(entity);
            }
        }
        
        ecb.Playback(state.EntityManager);
        // You are responsible for disposing of any ECB you create.
        ecb.Dispose();
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
