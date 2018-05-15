Shader "Custom/Surf/ColoredSurf" {
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

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShadingForward
		
		#pragma target 3.0

		half _Darkness;

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

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _FColor;
		fixed4 _BColor;
		fixed4 _LColor;
		fixed4 _RColor;
		fixed4 _TColor;
		fixed4 _UColor;

		float _HoriBool;

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldNormal;
		};

		void surf(Input i, inout SurfaceOutput o) {
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
			

			fixed4 c = tex2D(_MainTex, i.uv_MainTex) * col;

			o.Albedo = c.rgb;
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

