#ifndef SIMPLE_ULINT_INCLUDED
#define SIMPLE_ULINT_INCLUDED

#include "Common.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
CBUFFER_END

float4 SimpleULintVertext(float3 positionOS : POSITION) : SV_POSITION
{
    return float4(positionOS,1.0);
}

float4 SimpleULintFragment():SV_TARGET
{
    return _BaseColor;
}

#endif 