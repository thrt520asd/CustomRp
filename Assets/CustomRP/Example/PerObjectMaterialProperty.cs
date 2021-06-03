using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerObjectMaterialProperty : MonoBehaviour
{
    private static int baseColorId = Shader.PropertyToID(("_BaseColor"));
    private static int metallicId = Shader.PropertyToID(("_Metallic"));
    private static int smoothnessId = Shader.PropertyToID(("_Smoothness"));
    
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField, Range(0f, 1f)] private float metallic = 0f;
    [SerializeField, Range(0f, 1f)] private float smoothness = 0.5f;
    private static MaterialPropertyBlock _block;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (_block == null) _block = new MaterialPropertyBlock();
        _block.SetColor(baseColorId,baseColor);
        _block.SetFloat(smoothnessId,smoothness);
        _block.SetFloat(metallicId,metallic);
        GetComponent<Renderer>().SetPropertyBlock(_block);
    }
}
