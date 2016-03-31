Shader "Custom/CutOffUnlit" {
    Properties {
       _Texture("Texture",2D) = "white"{}
       _Cutoff ("Alpha cutoff", Range (0,1)) = 0.5
    }
    SubShader {

       
       //Blend SrcAlpha OneMinusSrcAlpha
       //Tags {"Queue"="Transparent+100"   "IgnoreProjector"="True"  "RenderType"="Transparent"}
       LOD 200
       Pass {
            Lighting Off
       		ZWrite On
       		Cull Off
       		AlphaTest Greater [_Cutoff]
       		SetTexture [_Texture] {
                Combine Primary * Texture
            }
       }
    }
    
    FallBack "Unlit/Transparent"
}