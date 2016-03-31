Shader "CrossSection/Solid" {

Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	
	_SectionColor ("Section Color", Color) = (1,0,0,1)
    _SectionPlane ("SectionPlane (x, y, z)", vector) = (0.707,0,-0.2)
    _SectionPoint ("SectionPoint (x, y, z)", vector) = (0,0,0)
    _ClipOffset ("ClipOffset",float) = 0
}

   SubShader {

      Pass {
         Cull FRONT // cull only front faces
         
         CGPROGRAM 
         #include "UnityCG.cginc"
         #pragma vertex vert
         #pragma fragment frag
         fixed4 _Color;
         
 		 fixed4 _SectionColor;
 		 fixed3 _SectionPlane;
	     fixed3 _SectionPoint;
	     float _ClipOffset;
  		 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 wpos : TEXCOORD0;
         };
 
         vertexOutput vert(appdata_base input) {
            vertexOutput output;
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.wpos = mul (_Object2World, input.vertex).xyz;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR  {
         	if(_ClipOffset -dot((input.wpos - _SectionPoint),_SectionPlane) < 0) discard;
         	return _SectionColor;
         }
         ENDCG  
         
      }
     
     Pass {
         Cull BACK // cull only front faces
         
         CGPROGRAM 
         #include "UnityCG.cginc"
         #pragma vertex vert
         #pragma fragment frag
         fixed4 _Color;
         
 		 fixed4 _SectionColor;
 		 fixed3 _SectionPlane;
	     fixed3 _SectionPoint;
	     float _ClipOffset;
  		 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 wpos : TEXCOORD0;
         };
 
         vertexOutput vert(appdata_base input) {
            vertexOutput output;
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
            output.wpos = mul (_Object2World, input.vertex).xyz;
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR  {
         	if(_ClipOffset -dot((input.wpos - _SectionPoint),_SectionPlane) < 0) discard;
         	return _Color;
         }
         ENDCG  
         
      }
   }
}