Shader "Custom/windowGrow" {
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}

		[Header(Emission)]
		_EmissionTex("Emission texture", 2D) = "gray" {}
		_EmiVal("Intensity", float) = 0.
		[HDR]_EmiColor("Color", color) = (1., 1., 1., 1.)
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

	struct v2f {
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.uv = v.texcoord;
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

	// Compute emission
	fixed4 emi = tex2D(_EmissionTex, i.uv).r * _EmiColor * _EmiVal;
	c.rgb += 2 * emi.rgb;
	if (c.a < 0.5)
		discard;
	return c;
	}

		ENDCG
	}
	}
}
