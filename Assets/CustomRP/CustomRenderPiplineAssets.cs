using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CreateAssetMenu(menuName = "Rendering/CustomRenderPipline")]
public class CustomRenderPiplineAssets : RenderPipelineAsset
{
    [SerializeField] private bool useDynamicBatching = true;
    [SerializeField] private bool useGPUInstancing = true;
    [SerializeField] private bool useSRPBatcher = true;
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipline(useDynamicBatching,useGPUInstancing,useSRPBatcher);    
    }



}
