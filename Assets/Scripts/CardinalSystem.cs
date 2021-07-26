using System.Diagnostics;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;
using Unity.Burst;

public class CardinalSystem : SystemBase
{
    private EntityQuery m_CardinalQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private bool m_initialized;
   
    protected override void OnCreate()
    {
        base.OnCreate();

        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_CardinalQuery = GetEntityQuery(typeof(CardinalType));

        RequireForUpdate(m_CardinalQuery);
    }

    protected override void OnUpdate()
    {   
        if(m_initialized)
        {
            return;
        }

        m_initialized = true;
        var settings = GetSingleton<CardinalType>();
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());
        var groundFab = settings.ground;
        var spread = settings.baseGroundSpread;
        var variance = settings.groundVariance;

        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        Job.WithCode(() =>
        {
            for (int i = 0; i < settings.baseGroundCount; i++)
            {
                
                var e = commandBuffer.Instantiate(groundFab);
                var s = rand.NextFloat(1, variance);
                // commandBuffer.SetComponent(e, new Scale { Value =  });
                commandBuffer.SetComponent(e, new Translation { Value = new float3(rand.NextFloat(-1f * spread, spread), rand.NextFloat(0, spread), rand.NextFloat(-1f * spread, spread)) });
                //commandBuffer.SetComponent(e, new NonUniformScale { Value = new float3(s, s, s) });
            }
        }).Schedule();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}
