Shader "TEST/TESTIMAGE"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplacementTex ("Displacement Texture", 2D) = "white" {}
		_Strength ("Distortion Strength", float) = 1
		_NoiseScale ("Displacement Scale", float) = 1
		_XAnimate ("X Animate", float) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				float4 worldPosition : TEXCORRD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _DisplacementTex;
			float _Strength;
			float _NoiseScale;
			float _XAnimate;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;

				half2 n = tex2D(_DisplacementTex, float2 ((i.uv.x + _XAnimate * _Time[1])/ _NoiseScale, i.uv.y / _NoiseScale));
				half2 d = n * 2 -1;

				i.uv += d * _Strength;
				
				if (i.uv.x > 1 || i.uv.x < 0 || i.uv.y > 1 || i.uv.y < 0)
					return (0,0,0,0);
				else				

				i.uv = saturate(i.uv);
				
				col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
