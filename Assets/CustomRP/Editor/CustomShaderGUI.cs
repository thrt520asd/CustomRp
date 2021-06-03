
using UnityEditor;
using UnityEngine;

public class CustomShaderGUI:ShaderGUI
{
    private MaterialEditor _editor;
    private Material[] materials;
    private MaterialProperty[] _properties;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);
        _editor = materialEditor;
        materials = materialEditor.targets as Material[];
        this._properties = properties;
    }

    void SetProperty(string name,float value)
    {
        FindProperty(name, _properties).floatValue = value;
    }

    void SetKeyword(string keyword,bool enabled)
    {
        foreach (var mat in materials)
        {
            if (enabled)
            {
                mat.EnableKeyword(keyword);
            }
            else
            {
                mat.DisableKeyword(keyword);
            }
            
        }
    }

    void SetProperty(string name, string keyword, bool val)
    {
        SetProperty(name,val?1.0f:0f);
        SetKeyword(keyword,val);
    }
    
    
}
