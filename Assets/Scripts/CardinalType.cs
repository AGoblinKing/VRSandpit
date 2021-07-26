using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct CardinalType : IComponentData
{
    public Entity ground;
    public UInt32 baseGroundCount;
    public UInt32 baseGroundSpread;
    public float groundVariance;
}
