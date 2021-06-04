Shader "SuperSmash/Toon-Character"
{
    Properties
    {
		[Header(Texture and tint)]
		[Space]_BaseColor("Color", Color) = (0.8,0.8,0.8,1)
		_MainTex ("Texture", 2D) = "white" {}
		[Header(Gloss and specular color)]
		[Space]_SpecularColor ("Specular Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_Gloss("Glossiness", Range(0,1)) = 0.25
		_GlossSoftness("Gloss Softness", Range (0,1)) = 0.5
		[Header(Rim Light)]
		[Space]_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimScale("Rim Scale", Range(-1, 1)) = -0.145
		_RimSoftness("Rim Softness", Range(0, 1)) = 0.25
		[Header(Shadow softness and color)]
		[Space]_ShadowColor ("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _ShadowSoftness ("Shadow Softness", Range(0, 1)) = 0.5
		[Header(Blinking Color)]
		[Space]_BlinkColor ("Blink Color", Color) = (1.0,0.0,0.0,1.0)
		//_Blink ("Blink", Range (0,1)) = 0.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                SHADOW_COORDS(2)
                float3 worldPos : TEXCOORD3;
				float3 viewDir : TEXCOORD1;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float4 _BaseColor;
			float4 _SpecularColor;
			float _Gloss;
			float _GlossSoftness;
			float4 _RimColor;
			float4 _RimDir;
			float _RimScale;
			float _RimSoftness;
            float4 _ShadowColor;
            float _ShadowSoftness;
			float4 _BlinkColor;
			fixed _Blink;

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
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, _ShadowSoftness, NdotL * shadow);
				float4 lCol = lightIntensity * _LightColor0;
				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = max(0,dot(normal, halfVector));

				//Specular calculation and coloriztion (it's full of *magic numbers*)
				float specularIntensity = pow(NdotH, _Gloss * 128.0) * 2.0; 
				float specularIntensitySmooth = smoothstep(0.5 - _GlossSoftness * 0.5, 0.5 + _GlossSoftness * 0.5, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				// Rim calculation and colorization
				float4 Rdot = 1 - dot(viewDir, normal);
				float rimScale = saturate(_RimScale + (Rdot * NdotL));
				float rimSoftness = smoothstep(0, _RimSoftness, rimScale);
				float4 rim = rimSoftness * _RimColor;

                // Sample the texture and apply tint
				fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;

				// Shadow color lerp
				float4 sColor = lerp(col, _ShadowColor, (_ShadowColor.a * (1-lCol)));

				// Blinking color lerp
				float4 finalColor = lerp(sColor, _BlinkColor, _Blink);
                
                return finalColor + specular + rim;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
