Shader "Custom/Scan Lines"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScanTex ("Scanline Texture", 2D) = "white" {}
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
			sampler2D _ScanTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;				

				float offsetHeight = frac(_Time[1] / 2) * 2;
				//if (offsetHeight >= 1) offsetHeight = 0 + (offsetHeight - 1);

				if (i.uv.y > offsetHeight && i.uv.y < offsetHeight + 0.05) 
					col = tex2D(_MainTex, i.uv + float2(-0.2, 0)) + 0.2;
				else 
					col = tex2D(_MainTex, i.uv);

				col *= tex2D(_ScanTex, float2(i.worldPosition.x, i.worldPosition.y) );
				col += 0.05; //scanlines make it look darker, so this is to brighten it up

				return col;
			}
			ENDCG
		}
	}
}
