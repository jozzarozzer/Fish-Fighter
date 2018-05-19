Shader "Custom/Surf/Water Caustics Surf" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_FColor ("Front Color", Color) = (1,1,1,1)
		_BColor ("Back Color", Color) = (1,1,1,1)
		_RColor ("Right Color", Color) = (1,1,1,1)
		_LColor ("Left Color", Color) = (1,1,1,1)
		_TColor ("Top Color", Color) = (1,1,1,1)
		_UColor ("Under Color", Color) = (1,1,1,1)

		_Darkness ("Darkness", Range(0.5,1)) = 1 //0.64 is completely neutral

		_HoriBool ("Horizontal?", float) = 1

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
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		//#pragma surface surf CelShadingForward
		#pragma surface surf Standard fullforwardshadows
		
		#pragma target 3.0

		half _Darkness;
		/*

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) 
		{
			half NdotL = dot(s.Normal, lightDir);
			if (NdotL <= 0.0) NdotL = 0;
			else NdotL = 1;
			half4 c;

			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
			c.rgb = s.Albedo * _Darkness; //removes shadows cast onto object

			 

			c.a = s.Alpha;
			return c;
		}
		*/
		sampler2D _MainTex;
		fixed4 _Color;
		float _Glossiness;
		float _Metallic;

		fixed4 _FColor;
		fixed4 _BColor;
		fixed4 _LColor;
		fixed4 _RColor;
		fixed4 _TColor;
		fixed4 _UColor;

		float _HoriBool;

		
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

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldNormal;
			float3 worldPos;
		};

		void surf(Input i, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			half3 worldNormal = WorldNormalVector(i, o.Normal);

			fixed4 col = (1,1,1,1);

			fixed3 Back = clamp(worldNormal.x, 0, 1) * _BColor;
			fixed3 Front = abs(clamp(worldNormal.x, -1, 0)) * _FColor;

			//fixed3 Left = (1 - abs(worldNormal.x)) * step( -clamp(worldNormal.z, -1, 0), 0) * _LColor;
			//fixed3 Right = (1 - abs(worldNormal.x)) * step( clamp(worldNormal.z, 0, 1), 0) * _RColor;

			fixed3 Left = clamp(worldNormal.z, 0, 1) * _LColor;
			fixed3 Right = abs(clamp(worldNormal.z, -1, 0)) * _RColor;

			fixed3 Hori = Front + Back + Right + Left;								

			fixed3 Top = clamp(worldNormal.y, 0, 1) * _TColor;
			fixed3 Under = -clamp(worldNormal.y, -1, 0) * _UColor;
			fixed3 Mid = (1- abs(worldNormal.y)) * Hori;

			if (_HoriBool == 1)
				col.rgb = Top + Under + Hori;
			else
				col.rgb = Top + Under + Mid;

			////////////////////////////////
			///*

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

				//col = tex2D(_MainTex, worldPos2);
				//col *= _Color;

				fixed cutoff = clamp(i.worldNormal.y, _YNormalCutoff, 1);

				col += caustics * cutoff;

			//*/

			////////////////////////////////
			

			fixed4 c = tex2D(_MainTex, i.uv_MainTex) * col;

			o.Albedo = c.rgb;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}



/*
			fixed4 col = (1,1,1,1);

			fixed3 Left = abs(worldNormal.x) * _LColor;
			fixed3 Right = (1 - abs(worldNormal.x)) * _RColor;

			fixed3 Hori = Left + Right;								

			fixed3 Top = abs(worldNormal.y) * _TColor;
			fixed3 Mid = (1- abs(worldNormal.y)) * Hori;

			col.rgb = Top + Mid;
			*/

