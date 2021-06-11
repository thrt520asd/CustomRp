Shader"CustomRP/SimpleULint"{
    Properties
    {
        _BaseColor("Color",Color)= (1.0,1.0,1.0,1.0)
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("_Src Blend",Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("_Src Blend",Float) = 1
        [Enum(on,1,off,0)] _ZWrite("Z Write",Float) = 1 
    }
    
    SubShader{
        Pass{
            
            ZWrite [_ZWrite]
            Blend [_SrcBlend] [_DstBlend]
            HLSLPROGRAM
            #include "SimpleULint.hlsl"
            #pragma multi_compile_instancing
            #pragma vertex SimpleULintVertext
            #pragma fragment SimpleULintFragment
            ENDHLSL
        }
    }
}
