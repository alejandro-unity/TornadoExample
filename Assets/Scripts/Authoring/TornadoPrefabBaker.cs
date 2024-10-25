using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

class TornadoBaker : MonoBehaviour
{
    public float speed = 1f;
    public float heightSpeed = 1f;
    public float radius = 2f;
}

class TornadoBakerBaker : Baker<TornadoBaker>
{
    public override void Bake(TornadoBaker authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new TornadoMovementData
        {
            speed = authoring.speed,
            heightSpeed = authoring.heightSpeed,
            radius = authoring.radius,
            height = 0,
            angle = 0
        });   
        
        AddComponent(entity, new URPMaterialPropertyBaseColor
                {
                    Value    = new float4(0,0,1,1)
                });   
    }
}

public struct TornadoMovementData : IComponentData
{
    public float speed;
    public float heightSpeed;
    public float radius;
    public float height;
    public float angle;
}
