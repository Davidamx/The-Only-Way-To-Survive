// Upgrade NOTE: 20180223 change projAngle treshold from 0.9 to 0.5

Shader "Projector/Indicators/FillRadial" {
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
     // AlphaTest Greater 0
      ColorMask RGB
      Blend SrcAlpha OneMinusSrcAlpha
      Offset -1, -1
  
      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
      //#pragma multi_compile_fog

      #include "UnityCG.cginc"

      #define PI 3.1415926

      fixed _Fill;
      fixed4 _MainColor;      
      fixed4 _FillColor;      
      float4x4 unity_Projector;
      sampler2D _ProjectTex;
	  half3 projNormal;
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
		half projAngle : TEXCOORD1;  
		float currentAngle : TEXCOORD2;
       // UNITY_FOG_COORDS(3)
        float4 pos : SV_POSITION;
      };

      vOutput vert (vInput v)
      {
        vOutput o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uvMain = mul(unity_Projector, v.vertex);        
		o.projAngle = dot(half3(0,1,0), mul( unity_ObjectToWorld, float4( v.normal, 0 ) ).xyz);

		fixed4 center = fixed4(0.5, 0.5, o.uvMain.z, o.uvMain.w);
		fixed4 up = fixed4(0.5, 1.0, o.uvMain.z, o.uvMain.w) - center;
		fixed4 current = o.uvMain - center;
		o.currentAngle = acos(dot(up, current) / (length(up) * length(current))) * (180 / PI);
        //UNITY_TRANSFER_FOG(o,o.pos);
        return o;
      }

      fixed4 frag (vOutput i) : SV_Target
      {
        float FillAngle = _Fill * 360;
      
        fixed4 main = tex2Dproj(_ProjectTex, UNITY_PROJ_COORD(i.uvMain));
       // fixed4 fill = tex2Dproj(_ProjectTex, UNITY_PROJ_COORD(i.uvMain));

        main *= _MainColor;
       // fill *= _FillColor;

        fixed fillBlit = 0;
        fillBlit += gt_than(i.uvMain.x, 0.5) * gt_than(FillAngle, i.currentAngle);
        fillBlit += ls_than(i.uvMain.x, 0.5) * ls_than(360 - FillAngle, i.currentAngle);
        fixed mainBlit = -(sign(fillBlit) - 1);

        fixed4 res = fixed4(0, 0, 0, 0);
		fixed stepv = step(0.5, i.projAngle);
        res += main * fixed4(mainBlit, mainBlit, mainBlit, mainBlit);
        //res += fill * fixed4(fillBlit, fillBlit, fillBlit, fillBlit);
		res *=  stepv;

        //UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0,0,0,0));

        return res*_Intensity;
      }
      ENDCG
    }
  }
}
