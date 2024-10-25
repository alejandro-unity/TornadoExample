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
        
        foreach (var (transform, movement) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<TornadoMovementData>>())
        {
            movement.ValueRW.angle  += movement.ValueRO.speed * deltaTime;
            movement.ValueRW.height += movement.ValueRO.heightSpeed * deltaTime;
            
            var x = math.cos(movement.ValueRO.angle) * (movement.ValueRO.radius );
            var z = math.sin(movement.ValueRO.angle) * (movement.ValueRO.radius );
            var y = movement.ValueRO.height;
            
            float3 newPosition = new float3(x, y, z);
            transform.ValueRW.Position = newPosition;
        }
        
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
