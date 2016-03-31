  Shader "Diffuse/HalfInvert" {
    Properties {
      _Color ("Main Color", Color) = (1,1,1,1)
	  _MainTex ("Texture", 2D) = "white" {}
	  
      _BoundaryPlane ("BoundaryPlane (x, y, z)", vector) = (0,1,0)
      _CentrePoint ("CentrePoint (x, y, z)", vector) = (0,0,0)
      _BoundaryOffset ("BoundaryOffset",float) = 0
    }
    
    
    SubShader {
    	Tags { "RenderType"="Opaque" }
		LOD 200

  
	      
	      
	      Cull Back
	      
	      CGPROGRAM
	      #pragma surface surf BlinnPhong
	      #pragma debug
	      fixed4 _LowerColor;
	      fixed4 _Color;
	      fixed3 _BoundaryPlane;
	      fixed3 _CentrePoint;
	      float _BoundaryOffset;
	      
	      struct Input {
	          float2 uv_MainTex;
	          float3 worldPos;
	      };
	      sampler2D _MainTex;
	      
	      void surf (Input IN, inout SurfaceOutput o) {
	      		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	          if (_BoundaryOffset -dot((IN.worldPos - _CentrePoint),_BoundaryPlane)>0){
		          //fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		          o.Albedo = tex.rgb * _Color.rgb;
		          o.Alpha = tex.a * _Color.a;
	          }else{
		          o.Albedo = (1,1,1) - tex.rgb * _Color.rgb;
		          o.Alpha = tex.a * _Color.a;
	          }
	          
	      }
	      ENDCG

    } 

    Fallback "Diffuse"
  }