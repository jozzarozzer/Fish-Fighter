Shader "Unlit/Waves"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_FadeGradient ("Fade Gradient", 2D) = "white" {}
		_Center ("Center", Vector) = (0,0,0)
		_LineWidth ("Line Width", Range(0.001, 0.01)) = 0.005
		_TimeDamp ("Time Damp", float) = 2
		_FadeAmount ("Fade Amount", float) = 500
		_FadeAmount2 ("Fade Amount 2", Range(0.0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		//Zrite Off
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPosition : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			sampler2D _FadeGradient;
			float3 _Center;
			float _LineWidth;
			float _TimeDamp;
			float _FadeAmount;
			float _FadeAmount2;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
		
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 waves = tex2D(_MainTex, float2((i.worldPosition.x + 100)/200, -i.worldPosition.z/100 - _Time[1]/20));
				float _offset = frac(_Time[1]/_TimeDamp) * 1/4;

				float d = distance(_Center, i.worldPosition);

				float radiusMult = (_Center.z / 150);

				float dN = 1 - saturate(d / (200 * radiusMult));

				dN 
				= 				
				step(0.25 - _offset, dN) * step(dN, 0.25 + _LineWidth - _offset) + 
				step(0.50 - _offset, dN) * step(dN, 0.50 + _LineWidth - _offset) + 
				step(0.75 - _offset, dN) * step(dN, 0.75 + _LineWidth - _offset) +
				step(1 - _LineWidth - _offset, dN) * step(dN, 1 - _offset) 
				;

				fixed4 fade = tex2D(_FadeGradient, float2 (i.uv.x, i.uv.y));
				fixed4 col = (1,1,1,1);
				col *= _Color;

				col.rgb += fixed3
				(
					clamp(dN * (-d/_FadeAmount + _FadeAmount2), 0, 1), 
					clamp(dN * (-d/_FadeAmount + _FadeAmount2), 0, 1), 
					clamp(dN * (-d/_FadeAmount + _FadeAmount2), 0, 1)
				);
				//col.rgb += waves.rgb * fade.rgb;

				//200 - clamp(_Time[1], 0, 60)

				return col;
			}
			ENDCG
		}
	}
}
