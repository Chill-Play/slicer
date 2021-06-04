Shader "SuperSmash/Toon-Glass"
{
    Properties
    {
		[Header(Texture, transparency and tint)]
		[Space]_BaseColor("Color", Color) = (0.8,0.8,0.8,1)
		_MainTex("Texture", 2D) = "white" {}
		_Transparency("Transparency", Range(0,1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1.0 
		[Header(Gloss texture and specular color)]
		[Space][NoScaleOffset]_SpecularTex("Specular Texture", 2D) = "white" {}
		_SpecularColor("Specular Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_Gloss("Glossiness", Range(0,1)) = 0.25
		_GlossSoftness("Gloss Softness", Range(0,1)) = 0.5
		[Header(Rim Light)]
		[Space]_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimScale("Rim Scale", Range(0, 1)) = -0.145
		_RimSoftness("Rim Softness", Range(0, 1)) = 0.25
		//[Header(Shadow softness and color)]
		//[Space]_ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
		//_ShadowSoftness("Shadow Softness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
			"Queue" = "Transparent"
        }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull [_Cull]
		ZWrite [_ZWrite]


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

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
                UNITY_FOG_COORDS(5)
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                SHADOW_COORDS(2)
                float3 worldPos : TEXCOORD3;
				float3 viewDir : TEXCOORD1;
            };

			sampler2D _MainTex;
			sampler2D _SpecularTex;
			//float4 _SpecularTex_ST;
			float4 _MainTex_ST;
			float4 _BaseColor;
			float4 _SpecularColor;
			float _Transparency;
			float _Gloss;
			float _GlossSoftness;
			float4 _RimColor;
			float4 _RimDir;
			float _RimScale;
			float _RimSoftness;
			//float4 _ShadowColor;
			//float _ShadowSoftness;

            v2f vert (appdata v)
            {
                v2f o;
				o.viewDir = WorldSpaceViewDir(v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//
				// TODO: Reject Diffuse Shadows, Refine Rim and Spec;
				//
				float3 normal = normalize(i.worldNormal);
				//float NdotL = dot(_WorldSpaceLightPos0, normal);
				float shadow = SHADOW_ATTENUATION(i);
				//float lightIntensity = smoothstep(0, _ShadowSoftness, NdotL * shadow);
				//float4 lCol = lightIntensity * _LightColor0;
				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = max(0,dot(normal, halfVector));

				//Specular calculation and coloriztion (it's full of *magic numbers*)
				float specularIntensity = pow(NdotH, _Gloss * 128.0) * 2.0;
				float specularIntensitySmooth = smoothstep(0.5 - _GlossSoftness * 0.5, 0.5 + _GlossSoftness * 0.5, specularIntensity);
				fixed4 specularTex = tex2D(_SpecularTex, i.uv);
				float4 specular = (specularIntensitySmooth * _SpecularColor) * specularTex.r;
				specular *= _Transparency;

				// Rim calculation and colorization
				// Rdot = 1 - dot(viewDir, normal);
				//float rimScale = saturate(_RimScale * Rdot);
				//float rimSoftness = smoothstep(0, _RimSoftness, rimScale);
				//float4 rim = rimSoftness * _RimColor;

				// Sample the texture and apply tint
				fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;

				// Shadow color lerp
				float4 sColor = col;
				sColor *= _Transparency;


				return sColor + specular;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
