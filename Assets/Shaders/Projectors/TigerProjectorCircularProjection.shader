Shader "Projector/Indicators/TigerCircularProjection" {
  Properties {
    _MainColor ("Main Color", Color) = (1,1,1,1)
	_ProjectTex ("Shape", 2D) = "" {} 
	_Intensity("Intensity", float) = 1.5
  }
  
  Subshader {
    Tags {"Queue"="Transparent"}
    Pass {
      ZWrite Off
      AlphaTest Greater 0
      ColorMask RGB
      Blend SrcAlpha OneMinusSrcAlpha
      Offset -1, -1
  
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog

      #include "UnityCG.cginc"

      #define PI 3.1415926

      fixed4 _MainColor;      
      float4x4 unity_Projector;
      sampler2D_half _ProjectTex;
	  half _Intensity;

      fixed gt_than(float x, float y) {
        return max(sign(x - y), 0);
      }

      fixed ls_than(float x, float y) {
        return max(sign(y - x), 0);
      }

      struct vInput {
        float4 vertex : POSITION;
		float3 normal : NORMAL;
        fixed2 texcoord : TEXCOORD0;        
      };

      struct vOutput {
        float4 uvMain : TEXCOORD0;
		fixed4 projectionInfo : TEXCOORD1;  
        UNITY_FOG_COORDS(2)
        float4 pos : SV_POSITION;
      };

      vOutput vert (vInput v)
      {
        vOutput o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uvMain = mul(unity_Projector, v.vertex);        
		o.projectionInfo.x = dot(half3(0,1,0), mul( unity_ObjectToWorld, float4( v.normal, 0 ) ).xyz);
		o.projectionInfo.yzw = step(0.5, o.projectionInfo.x);;

        UNITY_TRANSFER_FOG(o,o.pos);
        return o;
      }

      fixed4 frag (vOutput i) : SV_Target
      {
        fixed4 main = tex2Dproj(_ProjectTex, UNITY_PROJ_COORD(i.uvMain));
		main *= _MainColor;
		main *=  i.projectionInfo.y;

        UNITY_APPLY_FOG_COLOR(i.fogCoord, main, fixed4(0,0,0,0));

        return main *_Intensity;
      }
      ENDCG
    }
  }
}
