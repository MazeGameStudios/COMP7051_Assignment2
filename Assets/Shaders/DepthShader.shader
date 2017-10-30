Shader "MazeGameStudios/MyFogShader" {
	// Adapted from tutorials on Unity website

	Properties
	{
		// textures 
		_MainTex("Texture", 2D) = "white" {}

	// phong model 
	_DayRatio("Day Ratio", Range(0,1)) = 1
		_AmbientNightColor("Ambient Night Color", Color) = (0,0,0,0)
		_AmbientDayColor("Ambient Day Color", Color) = (1,1,1,1)
		_AmbientLighIntensity("Ambient Light Intensity", Range(0.0, 1.0)) = 1.0

		_DiffuseDirection("Diffuse Light Direction", Vector) = (0.1,0.1,0.1,1)
		_DiffuseColor("Diffuse Light Color", Color) = (0,0,0,1)
		_DiffuseIntensity("Diffuse Light Intensity", Range(0.0, 1.0)) = 1.0

		_SpecularPosition("Specular Light Position", Vector) = (0.1,0.1,0.1,1)
		_SpecularColor("Specular Light Color", Color) = (1,1,1,1)
		_SpecularIntensity("Specular Light Intensity", Range(0.0, 1.0)) = 1.0
		_SpecularShininess("Shininess", Float) = 10

		// fog 
		_FogColor("Fog Color", Color) = (0,0,0,0)
		_FogStart("Fog Start", Float) = 1
		_FogEnd("Fog End", Float) = 20
		_Distance("Distance", Range(0.0, 100)) = 1.0
	}

		SubShader
	{
		Pass
	{
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		// Shadows 
#include "UnityCG.cginc"
#include "Lighting.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"

		// Fog 
#pragma multi_compile_fog

		float _DayRatio;
	float4 _AmbientDayColor;
	float4 _AmbientNightColor;
	float _AmbientLighIntensity;
	float3 _DiffuseDirection;
	float4 _DiffuseColor;
	float _DiffuseIntensity;
	float4 _SpecularPosition;
	float4 _SpecularColor;
	float _SpecularIntensity;
	float _SpecularShininess;
	float4 _FogColor;
	float _FogEnd;
	float _FogStart;

	struct v2f
	{
		// Shadow 
		float2 uv : TEXCOORD0;
		SHADOW_COORDS(1) // put shadows data into TEXCOORD1
			fixed3 diff : COLOR0;
		fixed3 ambient : COLOR1;
		float4 pos : SV_POSITION;
		// Phong 
		float3 normal : NORMAL;
		float4 posWorld : TEXCOORD2;
		float3 normalDir : TEXCOORD3;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		half3 worldNormal = UnityObjectToWorldNormal(v.normal);
		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		o.diff = nl * _LightColor0.rgb;
		o.ambient = ShadeSH9(half4(worldNormal,1));
		// compute shadows data
		TRANSFER_SHADOW(o)

			// phong model 
			// o.position = UnityObjectToClipPos(v.vertex);
			o.normal = v.normal;
		float4x4 modelMatrix = unity_ObjectToWorld;
		float4x4 modelMatrixInverse = unity_WorldToObject;
		o.posWorld = mul(modelMatrix, v.vertex);
		o.normalDir = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
		return o;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f i) : SV_Target
	{
		// Texture + Shadow 
		fixed4 col = tex2D(_MainTex, i.uv);
	fixed shadow = SHADOW_ATTENUATION(i);
	fixed3 lighting = i.diff * shadow + i.ambient;
	col.rgb *= lighting;

	// phong 
	float4 diffuse = saturate(dot(_DiffuseDirection, i.normal));

	float3 specularReflection;
	float4x4 modelMatrixInverse = unity_WorldToObject;
	float3 normalDirection = normalize(mul(float4(i.normal, 0.0), modelMatrixInverse).xyz);

	float3 viewDirection = normalize(_WorldSpaceCameraPos - i.posWorld.xyz);
	float3 lightDirection;
	float attenuation;
	if (0.0 == _SpecularPosition.w) // directional light?
	{
		attenuation = 1.0; // no attenuation
		lightDirection = normalize(_SpecularPosition.xyz);
	}
	else // point or spot light
	{
		float3 vertexToLightSource =
			_SpecularPosition.xyz - i.posWorld.xyz;
		float distance = length(vertexToLightSource);
		attenuation = 1.0 / distance; // linear attenuation 
		lightDirection = normalize(vertexToLightSource);
	}

	if (dot(normalDirection, lightDirection) < 0.0)
		// light source on the wrong side?
	{
		specularReflection = float3(0.0, 0.0, 0.0);
		// no specular reflection
	}
	else // light source on the right side
	{
		specularReflection = attenuation * _SpecularColor.rgb * pow(max(0.0, dot(
			reflect(-lightDirection, normalDirection),
			viewDirection)), _SpecularShininess);
	}

	fixed4 phong = saturate((lerp(_AmbientNightColor, _AmbientDayColor, _DayRatio) * _AmbientLighIntensity)	// ambient
		+ (diffuse * _DiffuseColor * _DiffuseIntensity)						// diffuse 
		+ (_SpecularIntensity * float4(specularReflection,1)));				// specular 

																			// fog 
	float dist = distance(_WorldSpaceCameraPos, i.posWorld);
	float fogRatio = 1 - saturate((_FogEnd - dist) / (_FogEnd - _FogStart));

	// Combine                
	return (phong * col) * (1 - fogRatio) + (_FogColor) * (fogRatio);
	}

		// Notes: saturate clamps between 0-1

		ENDCG
	}

		Pass
	{
		Tags{ "LightMode" = "ShadowCaster" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#include "UnityCG.cginc"

		struct v2f {
		V2F_SHADOW_CASTER;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			return o;
	}

	float4 frag(v2f i) : SV_Target
	{
		SHADOW_CASTER_FRAGMENT(i)
	}
		ENDCG
	}
	}
}
