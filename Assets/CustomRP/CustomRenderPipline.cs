
using UnityEngine;
using UnityEngine.Rendering;
public class CustomRenderPipline : RenderPipeline
{
    CameraRender render = new CameraRender();
    private bool useDynamicBatching = true;
    private bool useGPUInstancing = true;
    public CustomRenderPipline(bool useDynamicBatching,bool useGPUInstancing,bool useSrpBatcher)
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSrpBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
    }
    
    
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var cam in cameras)
        {
            render.Render(context, cam,useDynamicBatching,useGPUInstancing);
            
        }
    }
}

