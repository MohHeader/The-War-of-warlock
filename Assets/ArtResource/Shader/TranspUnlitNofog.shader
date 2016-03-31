Shader "Custom/TranspUnlitNoFog" {
    Properties {
       _Color("Color & Transparency", Color) = (0, 0, 0, 0.5)
       _Texture("Texture",2D) = "white"{}
    }
    SubShader {
       Lighting Off
       Fog { Mode Off }
       ZWrite Off
       Cull Back
       Blend SrcAlpha OneMinusSrcAlpha
       Tags {"Queue"="Transparent+50"  "IgnoreProjector"="True"  "RenderType"="Transparent"}
       LOD 200
       Color[_Color]
       
       Pass {
       		SetTexture [_Texture] {
                Combine Primary * Texture
            }
       }
    }
    
    FallBack "Unlit/Transparent"
}