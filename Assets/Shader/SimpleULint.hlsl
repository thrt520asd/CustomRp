#ifndef SIMPLE_ULINT_INCLUDED
#define SIMPLE_ULINT_INCLUDED

#include "Common.hlsl"

// CBUFFER_START(UnityPerMaterial)
// float4 _BaseColor;
// CBUFFER_END


struct Attributes
{
    float3 positionOS : POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS:SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4,_BaseColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

Varyings SimpleULintVertext(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input,output);
    output.positionCS = TransformObjectToHClip(input.positionOS);
    return output;
}

float4 SimpleULintFragment(Varyings input):SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input);
    return UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_BaseColor);
}

#endif 