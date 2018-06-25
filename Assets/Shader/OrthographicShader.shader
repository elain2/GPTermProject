Shader "Custom/OrthographicShader" {
	Properties {

		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[Header(Ambient)]
		_Ambient ("Intensity", Range(0,1)) = 0.1
		_AmbColor("Color", color) = (1,1,1,1)
		
		[Header(Diffuse)]
		_Diffuse ("Val", Range(0,1)) = 1
		_DifColor("Color", color) = (1,1,1,1)

		[Header(Interact)]
		_CamDistanceValue("distance",float) = 10
		_MaxXValue("xValue", float) = 10
		_MaxZValue("zValue", float) = 10
		
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "LightMode" = "ForwardBase"  }
		LOD 200
		Pass{
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma vertex vert
		#pragma fragment frag

		#include "unitycg.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		struct v2f{
			float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 worldPos : TEXCOORD1;
            float3 worldNormal : TEXCOORD2;
		};

		v2f vert(appdata_base v){
			v2f o;

			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = normalize(mul(v.normal,(float3x3)unity_WorldToObject));
			o.uv = v.texcoord;

			return o;
		}

		sampler2D _MainTex;
		fixed4 _LightColor0;
		fixed _Ambient;	
		fixed4 _AmbColor;
		fixed _CamDistanceValue;
		fixed _MaxXValue;
		fixed _MaxZValue;


		fixed _Diffuse;
		fixed4 _DifColor;

		float4 frag(v2f i):SV_Target{
			fixed4 c = tex2D(_MainTex, i.uv);

			float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

			float3 worldNormal = normalize(i.worldNormal);
			
			float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);

			float camDistance = distance(_WorldSpaceCameraPos, i.worldPos);


			//camDistance = max(0, camDistance + (_CamDistanceValue - camDistance));



			float norDistance = min(0.75, (float)camDistance/(_CamDistanceValue * 1.5));
			norDistance = 1 - norDistance;


			fixed4 amb = _Ambient * _AmbColor;


				
			float NdotL = max(0, dot(worldNormal, lightDir));
				
			fixed4 dif = NdotL *  _LightColor0 * _Diffuse *_DifColor;
			fixed4 light = dif+amb;

			c.rgb *= light.rgb;

			c = fixed4(c.r * norDistance, c.g * norDistance, c.b * norDistance,1);

			if(c.a < 1) discard;
			return c;
		}

		ENDCG
		}
	}
	FallBack "Diffuse"
}
