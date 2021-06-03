
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string bufferName = "Lighting";

    private CommandBuffer buffer = new CommandBuffer()
    {
        name = bufferName
    };

    private const int maxDirLightCount = 4;
    private static int dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    private static int dirLightColorId = Shader.PropertyToID("_DirectionalLightColors");
    private static int dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirections");
    private CullingResults cullingResults;
    private static Vector4[] dirLightColors = new Vector4[maxDirLightCount];
    private static Vector4[] dirLightDirections = new Vector4[maxDirLightCount];
    public void Setup(ScriptableRenderContext context,CullingResults cullingResults)
    {
        this.cullingResults = cullingResults;
        buffer.BeginSample(bufferName);
        SetupLights();
        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;
        int dirLgihtCount = 0;
        for (int i = 0; i < visibleLights.Length; i++)
        {
            VisibleLight light = visibleLights[i];
            if (light.lightType == LightType.Directional)
            {
                SetupDirectionalLight(dirLgihtCount++,ref light);
                if (dirLgihtCount >= maxDirLightCount)
                {
                    break;
                }
            }
        }
        buffer.SetGlobalInt(dirLightCountId,dirLgihtCount);
        buffer.SetGlobalVectorArray(dirLightColorId,dirLightColors);
        buffer.SetGlobalVectorArray(dirLightDirectionId,dirLightDirections);
    }
    
    private void SetupDirectionalLight(int index,ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        //方向 矩阵的第三列为光源的前向向量
        dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
}
