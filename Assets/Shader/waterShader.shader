Shader "Custom/waterShader" {

	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}

		[Header(Ambient)]
		_Ambient("Intensity", Range(0., 1.)) = 0.1
		_AmbColor("Color", color) = (1., 1., 1., 1.)

		[Header(Diffuse)]
		_Diffuse("Val", Range(0., 1.)) = 1.
		_DifColor("Color", color) = (1., 1., 1., 1.)

		[Header(Specular)]
		[Toggle] _Spec("Enabled?", Float) = 0.
		_Shininess("Shininess", Range(0.1, 10)) = 1.
		_SpecColor("Specular color", color) = (1., 1., 1., 1.)

		[Header(Emission)]
		_EmissionTex("Emission texture", 2D) = "gray" {}
		_EmiVal("Intensity", float) = 0.
		[HDR]_EmiColor("Color", color) = (1., 1., 1., 1.)

		[Header(Spakle)]
		_SparkleSpeed("Speed", float) = 0.0
		_SparkleStrength("Strength", Range(0.0, 1.0)) = 0.0

		[Header(Waves)]
		_WaveSpeed("Speed", float) = 0.0
		_WaveStrength("Strength", Range(0.0, 1.0)) = 0.0
	}

		SubShader
		{
			Pass
			{
				Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" "LightMode" = "ForwardBase" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

			// Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
				#pragma shader_feature __ _SPEC_ON

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float4 texcoord0 : TEXCOORD0;
					float4 texcoord1 : TEXCOORD1;
				};
				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv_bump : TEXCOORD1;
					fixed4 light : COLOR0;
				};

				fixed4 _LightColor0;

				// Diffuse
				fixed _Diffuse;
				fixed4 _DifColor;

				//Specular
				fixed _Shininess;
				fixed4 _SpecColor;

				//Ambient
				fixed _Ambient;
				fixed4 _AmbColor;


				sampler2D _BumpMap;

				float _SparkleStrength;
				float _SparkleSpeed;

				float _WaveStrength;
				float _WaveSpeed;


				// Deformation uv map
				float3 movement(float3 pos, float2 uv) {
					float sinOff = (pos.x + pos.y + pos.z) * _SparkleStrength;
					float t = _Time.y * _SparkleSpeed;
					float fx = uv.x;
					float fy = uv.y;
					pos.x += sin(t + sinOff) * fx ;
					pos.y += sin(t + sinOff) * cos(t + sinOff) * fy;
					pos.z += sin(t + sinOff) * fx * fy;
					return pos;
				}

				// Deformate vertex
				float4 moveVertex(float4 pos, float2 uv) {
					float sinOff = (pos.x + pos.y + pos.z) * _WaveStrength;
					float t = _Time.y * _WaveSpeed;
					float fx = uv.x;
					float fy = uv.x * uv.y;
					pos.x += sin(t * 1.45 + sinOff) * fx * 0.5;
					pos.y -= cos(t * 3.12 + sinOff) * fx * 0.5 - fy * 0.9;
					pos.z -= sin(t * 2.2 + sinOff) * fx * 0.2;
					return pos;
				}

				float3 normalBefore;

				v2f vert(appdata v)
				{
					v2f o;
					// World position
					float4 worldPos = mul(unity_ObjectToWorld, moveVertex(v.vertex, v.texcoord0));

					// Clip position
					o.pos = mul(UNITY_MATRIX_VP, worldPos);

					// Light direction
					float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);


					float3 normalMap = tex2Dlod(_BumpMap, v.texcoord1);
					// Compute the new normal;
					float3 pos0 = movement(float3(normalMap.x, normalMap.y, normalMap.z), v.texcoord1);
					float3 pos1 = movement(float3(normalMap.x + 0.01, normalMap.y, normalMap.z), v.texcoord1);
					float3 pos2 = movement(float3(normalMap.x, normalMap.y, normalMap.z + 0.01), v.texcoord1);

					// Normal in model space
					float3 normal = cross(normalize(pos2 - pos0), normalize(pos1 - pos0));
					normalBefore = normal;

					// Normal in WorldSpace
					float3 worldNormal = normalize(mul(normalBefore, (float3x3)unity_WorldToObject));

					// Camera direction
					float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos.xyz);

					// Compute ambient lighting
					fixed4 amb = _Ambient * _AmbColor;

					// Compute the diffuse lighting
					fixed4 NdotL = max(0., dot(worldNormal, lightDir) * _LightColor0);
					fixed4 dif = NdotL * _Diffuse * _LightColor0 * _DifColor;

					o.light = dif + amb;

					// Compute the specular lighting
				#if _SPEC_ON
					float3 refl = reflect(-lightDir, worldNormal);
					float RdotV = max(0., dot(refl, viewDir));
					fixed4 spec = pow(RdotV, _Shininess) * _LightColor0 * ceil(NdotL) * _SpecColor;

					o.light += spec;
				#endif

					o.uv = v.texcoord0;

					return o;
				}

				sampler2D _MainTex;

				// Emission
				sampler2D _EmissionTex;
				fixed4 _EmiColor;
				fixed _EmiVal;

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, i.uv);
					c.rgb *= i.light;



					// Compute emission
					fixed4 emi = tex2D(_EmissionTex, i.uv).r * _EmiColor * _EmiVal;
					c.rgb += emi.rgb;

					return c;
				}

			ENDCG
		}
	}
}