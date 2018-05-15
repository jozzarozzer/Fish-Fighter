// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unlit/DepthShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_DepthMult ("Depth Multiplier", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		//Zrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float depth : DEPTH;
			};			

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _DepthMult;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    float depth = clamp(i.depth * _DepthMult, 0, 1);
				float invert = 1 - depth;
				fixed4 col = fixed4(invert, invert, invert, depth) * _Color;

				return col;
			}
			ENDCG
		}
	}
}
