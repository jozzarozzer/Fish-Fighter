﻿Shader "Custom/Fade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_WorldPosSlider ("World Pos Slider", Range(0,100)) = 0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200

		Zwrite On
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _WorldPosSlider;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb + (IN.worldPos.y/_WorldPosSlider) * float4(0.73,0.73,0.73,0);

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a + (IN.worldPos.y/_WorldPosSlider);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
