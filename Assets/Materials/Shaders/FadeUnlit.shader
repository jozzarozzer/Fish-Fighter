Shader "Unlit/FadeUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_WorldPosSlider ("World Pos Slider", Range(0,75)) = 0
		_YOffset ("Y Offset", float) = 0
		_LColor ("Left Color", Color) = (1,1,1,1)
		_RColor ("Right Color", Color) = (1,1,1,1)
		_TColor ("Top Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		//Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				half3 worldNormal : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _WorldPosSlider;
			float _YOffset;

			fixed4 _LColor;
			fixed4 _RColor;
			fixed4 _TColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col;
				half3 normalFlip = half3 (i.worldNormal.r + i.worldNormal.g + i.worldNormal.b, i.worldNormal.r + i.worldNormal.g + i.worldNormal.b, i.worldNormal.r + i.worldNormal.g + i.worldNormal.b);
				if (i.worldNormal.x <= -0.001)
					col.rgb = _LColor;
				else if (i.worldNormal.z <= -0.001)
					col.rgb = _RColor;
				else
					col.rgb = _TColor;
				//col.rgb = normalFlip;
				
				//col = _Color;
				col.a = clamp(1 + (i.worldPosition.y + _YOffset)/_WorldPosSlider, 0, 1);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
