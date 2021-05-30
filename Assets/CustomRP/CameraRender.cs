using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    Camera camera;
    ScriptableRenderContext context;
    const string bufferName = "Render Camera";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };
    CullingResults cullingResults;
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    private static ShaderTagId litShaderTagId = new ShaderTagId("CustomLit");
    private Lighting lighting = new Lighting();
    public void Render(ScriptableRenderContext context, Camera camera,bool useDynamicBatching,bool useGPUInstancing)
    {
        this.context = context;
        this.camera = camera;
#if UNITY_EDITOR
        PrepareForSceneWindow();
        PrepareBuffer();
#endif
        if (!Cull())
        {
            return;
        }
        Setup();
        lighting.Setup(context);
        DrawVisibleGeometry(useDynamicBatching,useGPUInstancing);
#if UNITY_EDITOR
        DrawUnsupportedShaders();
        DrawGizmos();
#endif

        Submit();
    }

    

    bool Cull()
    {
        ScriptableCullingParameters p;
        if (camera.TryGetCullingParameters(out p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }
        return false;
    }

    void Setup()
    {
        context.SetupCameraProperties(camera);
        //get clear flas
        CameraClearFlags flags = camera.clearFlags;
        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags == CameraClearFlags.Color, flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
        //buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(SampleName);
        ExecuteBuffer();


    }
    
    public void DrawVisibleGeometry(bool useDynamicBatching,bool useGPUInstancing)
    {

        //绘制顺序和相机
        var sortSetting = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        //shader pass 和排序
        var drawingSetings = new DrawingSettings(unlitShaderTagId, sortSetting)
        {
            enableInstancing = useGPUInstancing,
            enableDynamicBatching =  useDynamicBatching,
        };
        drawingSetings.SetShaderPassName(1,litShaderTagId);
        var filteringSetting = new FilteringSettings(RenderQueueRange.opaque);
        //绘制不透明物体
        context.DrawRenderers(cullingResults, ref drawingSetings, ref filteringSetting);
        //绘制天空盒
        context.DrawSkybox(camera);
        //绘制透明物体
        sortSetting.criteria = SortingCriteria.CommonTransparent;
        drawingSetings.sortingSettings = sortSetting;
        filteringSetting.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cullingResults, ref drawingSetings, ref filteringSetting);
    }

    public void Submit()
    {
        buffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

}

