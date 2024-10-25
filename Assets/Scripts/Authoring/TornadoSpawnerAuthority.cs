using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class TornadoSpawnAuthority : MonoBehaviour
{
    public enum Teams { TeamA = 0 , TeamB = 1  };

    public GameObject prefab;
    public float SpawnRate = 0.5f;
    
    // Create an entity that has the prefabs 
    private class TornadoAuthoringBaker : Baker<TornadoSpawnAuthority>
    {
        public override void Bake(TornadoSpawnAuthority authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);
            
            AddComponent(entity, new TornadoSpawner
            {
                Prefab  = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                SpawnPosition =  authoring.transform.position,
                SpawnRate =  authoring.SpawnRate,
                NextSpawnTime = 0.0f
            });
        }
    }
}
public struct TornadoSpawner : IComponentData
{
    public Entity Prefab;
    public float NextSpawnTime;
    public float3 SpawnPosition;
    public float SpawnRate;
    
}
