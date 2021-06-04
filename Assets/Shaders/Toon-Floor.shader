Shader "SuperSmash/Toon-Floor"
{
    Properties
    {
		[Header(Texture and tint)]
		[Space]_BaseColor("Color", Color) = (0.8,0.8,0.8,1)
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
		_Tiling("Tiling", Float) = 0.25
		[Header(Shadow softness and color)]
		[Space]_ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
		_ShadowSoftness("Shadow Softness", Range(0, 1)) = 0.5
		[Header(Fog parameters)]
		[Space]_FogOffset("Fog Offset (Y)", Float) = 60.0
		_FogEdge("Fog Edge (Y)", Float) = 140.0
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,0.5)
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
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Tiling;
			float4 _BaseColor;
			float4 _ShadowColor;
			float _ShadowSoftness;
            float4 _FogColor;
			float _FogOffset;
			float _FogEdge;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                TRANSFER_SHADOW(o)
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float shadow = SHADOW_ATTENUATION(i);
				float lightIntensity = smoothstep(0, _ShadowSoftness, NdotL * shadow);
				float4 lCol = lightIntensity * _LightColor0;

				// Sample the texture and apply tint
				fixed4 col = tex2D(_MainTex, i.worldPos.xz * _Tiling) * _BaseColor;

				// Shadow color lerp
				float4 sColor = lerp(col, _ShadowColor, (_ShadowColor.a * (1 - lCol)));

				// Fog parameters and color lerp
				float fog = ((-i.worldPos.y - _FogOffset) / _FogEdge);
				float4 finalColor = lerp(sColor, _FogColor, clamp(fog, 0, 1));

                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
