using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace PrefabTornado
{
    public partial struct TornadoSpawnSystem : ISystem
    {
        public Random random;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
            state.RequireForUpdate<TornadoSpawner>();
            // its ok to have a config entity to use a singleton pattern
            //state.RequireForUpdate<Config>();
            
            random = new Random((uint)state.WorldUnmanaged.Time.ElapsedTime + 1234);
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<TornadoSpawner> spawner in SystemAPI.Query<RefRW<TornadoSpawner>>())
            {
                ProcessSpawner(ref state, spawner );
            }
           
        }

        private void ProcessSpawner(ref SystemState state, RefRW<TornadoSpawner> spawner)
        {
            // If the next spawn time has passed.
            if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                // Spawns a new entity and positions it at the spawner.
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                // LocalPosition.FromPosition returns a Transform initialized with the given position.
                var pos = spawner.ValueRO.SpawnPosition +  new float3(random.NextFloat() , 0, random.NextFloat());
                var colorRandom = random.NextFloat3();
                state.EntityManager.SetComponentData(newEntity,LocalTransform.FromPosition(pos));
                state.EntityManager.SetComponentData(newEntity, new URPMaterialPropertyBaseColor {Value = new float4(colorRandom.x, random.NextFloat(), random.NextFloat(), 1)});
                
                // Resets the next spawn time.
                spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
            }

        }
    }
}