﻿#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED
#include "Surface.hlsl"
#include "Light.hlsl"
float3 IncomingLight(Surface surface,Light light)
{
    return saturate(dot(surface.normal,light.direction))*light.color;
}

float3 GetLighting(Surface surface,BRDF brdf,Light light)
{
    return IncomingLight(surface,light)*DirectBRDF(surface,brdf,light);
}
float3 GetLighting(Surface surface,BRDF brdf)
{
    //可见光照明结果累加
    float3 color = 0.0;
    for(int i=0;i<GetDirectionalLightCount();i++)
    {
        color+=GetLighting(surface,brdf, GetDirectionalLight(i));
    }
    return color;
    // return GetLighting(surface,GetDirectionalLight());
}
#endif