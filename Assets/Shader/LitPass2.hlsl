#ifndef CUSTOM_LIT_PASS_INCLUDED
#define CUSTOM_LIT_PASS_INCLUDED
#include "Surface.hlsl"
#include "Common.hlsl"
#include "Lighting.hlsl"
TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);
struct Attributes
{
    float3 positionOS : POSITION;
    float2 baseUV : TEXCOORD0;
    //表面法线
    float3 normalOS:NORMAL;
    // UNITY_VERTEX_INPUT_INSTANCE_ID
    
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 baseUV : VAR_BASE_UV;
    float3 normalWS:VAR_NORMAL;
    // UNITY_VERTEX_INPUT_INSTANCE_ID
};

CBUFFER_START(UnityPerMaterial)
    float4 _BaseColor;
    float4 _BaseMap_ST;
CBUFFER_END
Varyings LitPassVertex(Attributes input)
{
    Varyings output;
    // UNITY_SETUP_INSTANCE_ID(input)
    // UNITY_TRANSFER_INSTANCE_ID(input,output);
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    
    output.positionCS = TransformObjectToHClip(positionWS);
    // float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_BaseMap_ST);
    float4 baseST =  _BaseMap_ST;//UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_BaseMap_ST);
    output.baseUV = input.baseUV*baseST.xy+baseST.zw;
    output.normalWS = TransformObjectToWorldNormal(input.normalOS);
    return output;
}
float4 LitPassFragment(Varyings input):SV_Target{
    // UNITY_SETUP_INSTANCE_ID(input);
    float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,input.baseUV);
    float4 baseColor = _BaseColor;//UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial,_BaseColor);
    float4 base = baseMap*baseColor;
    Surface surface;
    surface.normal = normalize(input.normalWS);
    surface.color = base.rgb;
    surface.alpha = base.a;
    float3 color = GetLighting(surface);
    return float4(0,0,0,0);
    return float4(color,surface.alpha);
    
}

#endif