// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/snow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Bump ("Bump", 2D) = "bump" {}
		_Snow ("level of snow", Range(1, -1)) = 1
		_SnowColor("Snow Color", Color) = (1,1,1,1)
		_SnowDirection("Snow Direction", Vector) = (0,1,0)
		_SnowDepth("Depth of snow", Range(0,0.1)) = 0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Bump;
		float _Snow;
		fixed4 _SnowColor;
		fixed4 _SnowDirection;
		float _SnowDepth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Bump;
			float3 worldNormal;
			INTERNAL_DATA
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert (inout appdata_full v)
		{
		
			fixed4 sn = mul(_SnowDirection, unity_WorldToObject);
			if(dot(v.normal, sn.xyz) >= _Snow)
			v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
		}



		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			o.Normal = UnpackNormal (tex2D (_Bump, IN.uv_Bump));
			if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>=_Snow)
				o.Albedo = _SnowColor.rgb;
			else
				o.Albedo = c.rgb * _Color;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
