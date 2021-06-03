﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshBall:MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID("_BaseColor");
    private static int metallicId = Shader.PropertyToID("_Metallic");
    private static int smoothnessId = Shader.PropertyToID("_Smoothness");
    [SerializeField] private Mesh mesh = default;
    [SerializeField] private Material material = default;

    private Matrix4x4[] matrices = new Matrix4x4[1023];
    private Vector4[] baseColors = new Vector4[1023];
    private float[] metallics = new float[1023];
    private float[] smoothness = new float[1023];

    private MaterialPropertyBlock block;

    private void Awake()
    {
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f,
                Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                Vector3.one * Random.Range(0.5f, 1.5f));
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
            metallics[i] = Random.value < 0.25f ? 1f: 0f;
            smoothness[i] = Random.Range(0.05f, 0.95f);
        }
        
    }

    void Update()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId,baseColors);
            block.SetFloatArray(smoothnessId,smoothness);
            block.SetFloatArray(metallicId,metallics);
        }
        Graphics.DrawMeshInstanced(mesh,0,material,matrices,1023,block);
    }
}
