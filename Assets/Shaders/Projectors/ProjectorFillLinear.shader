// Upgrade NOTE: 20180223 change projAngle treshold from 0.9 to 0.5

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'

Shader "Projector/Indicators/FillLinear" {
  Properties {
    _MainColor ("Main Color", Color) = (1,1,1,1)
    _FillColor ("Fill Color", Color) = (1,1,1,1)
    _ProjectTex ("Shape", 2D) = "" {}    
    _Fill ("Fill", Range (0,1)) = 0
	_Intensity("Intensity", float) = 1.5
  }
  
  Subshader {
    Tags {"Queue"="Transparent"}
    Pass {
      ZWrite Off
      AlphaTest Greater 0
      ColorMask RGB
      Blend SrcAlpha One//MinusSrcAlpha
      Offset -1, -1
  
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog

      #include "UnityCG.cginc"
  
      fixed _Fill;
      fixed4 _MainColor;      
      fixed4 _FillColor;      
      float4x4 unity_Projector;
      sampler2D _ProjectTex;
	  half3 projNormal;
	  half _Intensity;

      struct vInput {
        float4 vertex : POSITION;
		float3 normal : NORMAL;
        fixed2 texcoord : TEXCOORD0;        
      };

      struct vOutput {
        float4 uvMain : TEXCOORD0;
		half projAngle : TEXCOORD1;     
		UNITY_FOG_COORDS(2)
        float4 pos : SV_POSITION;
		
      };

      vOutput vert (vInput v)
      {
        vOutput o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uvMain = mul(unity_Projector, v.vertex);        
		o.projAngle = dot(half3(0,1,0), mul( unity_ObjectToWorld, float4( v.normal, 0 ) ).xyz);
        UNITY_TRANSFER_FOG(o,o.pos);
        return o;
      }

      fixed4 frag (vOutput i) : SV_Target
      {
        fixed4 main = tex2Dproj(_ProjectTex, UNITY_PROJ_COORD(i.uvMain));
        fixed4 fill = tex2Dproj(_ProjectTex, UNITY_PROJ_COORD(i.uvMain));

        main *= _MainColor;
        fill *= _FillColor;

        fixed mainBlit = max(0, sign(i.uvMain.y - _Fill));
        fixed fillBlit = max(0, sign(_Fill - i.uvMain.y));
		
        fixed4 res = fixed4(0, 0, 0, 0);

		fixed stepv = step(0.5, i.projAngle);

        res += main * fixed4(mainBlit, mainBlit, mainBlit, mainBlit);
        res += fill * fixed4(fillBlit, fillBlit, fillBlit, fillBlit);

		res *=  stepv;

        UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0,0,0,0));

        return res*_Intensity;
      }
      ENDCG
    }
  }
}
