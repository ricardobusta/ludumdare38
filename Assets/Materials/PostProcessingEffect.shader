Shader "Hidden/PostProcessingEffect"
{
	Properties
	{
		_MainTex ("From Camera Texture", 2D) = "white" {}
		_AltTex ("Pattern Texture", 2D) = "white" {}
		_Repeat ("Repeat", Vector) = (0,0,0,0) 
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _AltTex;
			float4 _Repeat;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_AltTex, i.uv*_Repeat.xy);
				// just invert the colors
				col = col*col2;
				return col;
			}
			ENDCG
		}
	}
}
