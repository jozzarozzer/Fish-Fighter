Shader "Custom/Water Caustics"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)

		_WaveTex ("Wave Texture", 2D) = "white" {}
		_WaveOffset ("Wave Offset", Vector) = (0,0,0)
		_WaveScale ("Wave Scale", float) = 1
		_WaveStrength ("Wave Strength", float) = 1
		_WaveSpeedX ("Wave Speed X", float) = 0
		_WaveSpeedY ("Wave Speed Y", float) = 0

		_CausticsTex ("Casutics Texture", 2D) = "white" {}
		_CausticsColor ("Caustics Color", Color) = (1,1,1,1)
		_Scale ("Scale", Range(0,50)) = 1
		_XAnimate ("X Animate", float) = 0
		_YAnimate ("Y Animate", float) = 0

		_DisplacementTex ("Displacement Texture", 2D) = "white" {}
		_Strength ("Strength", float) = 1
		_NoiseScale ("Noise Scale", float) = 1

		_XAnimateNoise ("X Animate Noise", float) = 0
		_YAnimateNoise ("Y Animate Noise", float) = 0

		_YNormalCutoff ("Y Normal Cutoff", Range(0,1)) = 0.05
	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

		Pass
		{

			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"



			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				half3 worldNormal : TEXCOORD2;
				float3 screenPos : TEXCOORD3;
			};

			float LightToonShading(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                //return floor(NdotL * _Threshold) / (_Threshold - 0.5);
				return NdotL;
            }

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);		
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Color;

			sampler2D _CausticsTex;
			fixed4 _CausticsColor;
			float _Strength;
			float _Scale;
			float _XAnimate;
			float _YAnimate;

			sampler2D _DisplacementTex;
			float _NoiseScale;
			float _XAnimateNoise;
			float _YAnimateNoise;

			sampler2D _WaveTex;
			float2 _WaveOffset;
			float _WaveScale;
			float _WaveStrength;
			float _WaveSpeedX;
			float _WaveSpeedY;
			
			fixed _YNormalCutoff;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 worldPos2 = float2 (i.worldPos.x, i.worldPos.z);

				////////

				half2 n = tex2D(_DisplacementTex, float2 ((worldPos2.x + _XAnimateNoise * _Time[1])/ _NoiseScale, worldPos2.y / _NoiseScale));
				half2 d = n * 2 -1;

				worldPos2 += d * _Strength;		
				
				////////

				half2 m = tex2D(_WaveTex, float2 ( (worldPos2.x + _WaveOffset.x + _WaveSpeedX * _Time[1]) / _WaveScale, (worldPos2.y + _WaveOffset.y + _WaveSpeedY * _Time[1]) / _WaveScale) );
				half2 e = m * 2 -1;

				worldPos2 += e * _WaveStrength;

				//worldPos2 = saturate(worldPos2);
				
				////////

				float2 causticsCoord = float2( (worldPos2.x + _XAnimate * _Time[1]) / _Scale, (worldPos2.y + _YAnimate * _Time[1]) / _Scale);
				
				float4 caustics = tex2D(_CausticsTex, causticsCoord);

				caustics *= _CausticsColor;

				////////

				fixed4 col = tex2D(_MainTex, worldPos2);
				col *= _Color;

				fixed cutoff = clamp(i.worldNormal.y, _YNormalCutoff, 1);

				col += caustics * cutoff;
				
				return col;
			}
			ENDCG
		}
	}
}
