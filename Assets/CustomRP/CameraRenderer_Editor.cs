#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRender
{
    partial void DrawGizmos();
    partial void DrawUnsupportedShaders();
    partial void PrepareForSceneWindow();
    partial void PrepareBuffer();
#if UNITY_EDITOR

    static ShaderTagId[] legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM"),
    };
    static Material errorMaterial;
    

    partial void DrawUnsupportedShaders()
    {
        if (!errorMaterial)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        var drawingSetting = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera)) { overrideMaterial = errorMaterial };
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            //设置ShaderPassName
            drawingSetting.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSetting = FilteringSettings.defaultValue;
        //绘制不支持的物体
        context.DrawRenderers(cullingResults, ref drawingSetting, ref filteringSetting);
    }
    
    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareForSceneWindow()
    {
        if(camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }
    string SampleName { get; set; }
    partial void PrepareBuffer()
    {
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName=camera.name;
        Profiler.EndSample();
    }
#else
    const string SampleName = bufferName;
#endif
}