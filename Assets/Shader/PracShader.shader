Shader "Custom/PracShader" {
	Properties {
		_MainTexture ("Main Texture", 2D) = "white"{}
		
		
		[Header(Ambient)]
		_Ambient ("Intensity", Range(0,1)) = 0.1
		_AmbColor("Color", color) = (1,1,1,1)
		
		[Header(Diffuse)]
		_Diffuse ("Val", Range(0,1)) = 1
		_DifColor("Color", color) = (1,1,1,1)
		
		[Header(Specular)]
		[Toggle] _Spec("Enable", Float) = 0.
		_Shiness ("Shiness", Range(0.1, 100)) = 1.
		_SpecColor ("Specular Color", color) = (1,1,1,1)


		[Header(Emission)]
		_EmissionTex("Emission texture", 2D) = "gray" {}
		_EmiVal ("Intensity", Range(0,2)) = 0
		[HDR]_EmiColor("Color",color) = (1,1,1,1)

		[Header(CelShading)]
		[Toggle] _Cel("Enable", Float) = 0.
		_threshold("ThresHold", Range(1,20)) = 5

		[Header(Outline)]
		[Toggle]_Outline("Enable", float) = 0.
		_OutlineValue("Value", Range(0,1)) = 0.1
		_OutlineColor("Color", color) = (0,0,0,1)
		
	}

	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "LightMode" = "ForwardBase" }

		Pass{
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature __ _SPEC_ON
			#pragma shader_feature __ _CEL_ON


			#include "unitycg.cginc"

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

			sampler2D _MainTexture;
			fixed4 _LightColor0;
			fixed _threshold;

			fixed _Ambient;
			fixed4 _AmbColor;

			fixed _Diffuse;
			fixed4 _DifColor;


			fixed _Shiness;
			fixed4 _SpecColor;

			sampler2D _EmissionTex;
			fixed _EmiVal;
			fixed4 _EmiColor;

			float4 frag(v2f i):SV_Target{

				fixed4 c = tex2D(_MainTexture, i.uv);

				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 worldNormal = normalize(i.worldNormal);

				fixed4 amb = _Ambient * _AmbColor;



				float NdotL = max(0, dot(worldNormal, lightDir));

				//디퓨즈 카툰렌더링
				#if _CEL_ON
				NdotL = saturate(floor(NdotL * _threshold) / (_threshold - 0.5));
				#endif

				fixed4 dif = NdotL *  _LightColor0 * _Diffuse *_DifColor;
				fixed4 light = dif+amb;
				//스페큘러
				#if _SPEC_ON
                float3 HalfVector = normalize(lightDir + viewDir);
                float NdotH = max(0., dot(worldNormal, HalfVector));

				//스페큘러 카툰렌더링
				#if _CEL_ON
				if(any(NdotH > 0.99)){
					NdotH = 1;
				}else{
					NdotH = 0;
				}
				#endif

                fixed4 spec = pow(NdotH, _Shiness) * _LightColor0 * _SpecColor;
				spec.g = max(0, spec.g - 0.3);
				spec.b = max(0, spec.b - 0.3);

				light += spec;
				#endif
				
				c.rgb *= light.rgb;

				//에미션
				fixed4 emi = tex2D(_EmissionTex, i.uv) * _EmiColor *_EmiVal;
				c.rgba += emi.rgba;


				 if(c.a < 1) discard;
				 return c;
			}
			ENDCG
		}
		//Pass{
		//	Cull Front
		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#pragma shader_feature __ _OUTLINE_ON
		//	#include "unitycg.cginc"

		//	struct v2f{
		//		float4 pos :SV_POSITION;
		//	};
			
		//	float _OutlineValue;
		//	v2f vert(appdata_base v){
		//		v2f o;
		//		o.pos = UnityObjectToClipPos(v.vertex);
		//	#if _OUTLINE_ON

		//		float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV,v.normal);
		//		float2 offset = TransformViewToProjection(normal.xy);
		//		o.pos.xy += offset * o.pos.z * _OutlineValue;
		//	#endif
		//		return o;
		//	}
		//	fixed4 _OutlineColor;
		//	float4 frag(v2f i):SV_Target{

		//		fixed4 color;
		//	#if _OUTLINE_ON
		//	color = _OutlineColor;
		//	#endif
				
		//		return _OutlineColor;
		//	}
		//	ENDCG
		//}
	}
}
