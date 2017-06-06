Shader "CCC/ZaWarudoEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_InvRayon("Rayon Inverse Start", float) = 1
		_InvRayonE("Rayon Inverse End", float) = 2
		_ColShift("Color Shift", Range(0,1)) = 0
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
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _InvRayon;
			float _InvRayonE;
			float _ColShift;

			float3 rgb2hsv(float3 c) {
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
				float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			float3 hsv2rgb(float3 c) {
				c = float3(c.x, clamp(c.yz, 0.0, 1.0));
				float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 couleur = tex2D(_MainTex, i.uv);

				float x = i.uv.x-0.5;
				float y = i.uv.y-0.5;
				float d = sqrt((x*x) + (y*y));

				if (d*_InvRayon < 1 && d*_InvRayonE > 1)
				{
					//effet !
					float3 hsv = rgb2hsv(couleur.rgb);
					hsv.x += _ColShift;
					float3 rgb = hsv2rgb(hsv);
					couleur.rgb = rgb;
					couleur = 1 - couleur;
				}

				return couleur;
			}
			ENDCG
		}
	}
}
