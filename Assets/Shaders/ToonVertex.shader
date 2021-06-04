Shader "Custom/ToonVertex"
{
    Properties
    {
        _MainColor("Color", Color) = (0.8,0.8,0.8,1)
        _ShadowColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _ShadowSoftness("Shadow Softness", Range(0, 1)) = 0.5

        [Toggle(USE_SPECULAR)]
        _UseSpecular("Use Specular", Float) = 0 
        _SpecularColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _SpecularShininess("Specular Shininess", Range(0, 1)) = 0.5
        _SpecularSoftness("Specular Softness", Range(0.01, 1)) = 1

        [Toggle(USE_RIM)]
        _UseRim("Use Rim", Float) = 0

        _RimColor("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimShininess("Rim Shininess", Range(0, 1)) = 0.5
          

    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
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
            #pragma shader_feature USE_MAIN_TEXTURE
            #pragma shader_feature USE_SPECULAR
            #pragma shader_feature USE_RIM
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 vertexColor : COLOR;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertexColor : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                SHADOW_COORDS(2)
                float3 worldPos : TEXCOORD3;
                float3 viewDir : TEXCOORD4;
            };

            float4 _MainColor;
            float4 _ShadowColor;
            float _ShadowSoftness;
            float4 _SpecularColor;
            float _SpecularShininess;
            float _SpecularSoftness;
            float4 _RimColor;
            float _RimShininess;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.vertexColor = v.vertexColor;
                TRANSFER_SHADOW(o)
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }



            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = _MainColor;

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);


                color *= i.vertexColor;


                //lighting
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, _ShadowSoftness, NdotL * shadow);
                float4 finalColor = lerp(_ShadowColor * color, color, lightIntensity);


                //Specular
                #if USE_SPECULAR
                float specularIntensity = pow(NdotH, 10000 * _SpecularShininess);
                float specularIntensitySmooth = smoothstep(0.005, _SpecularSoftness, specularIntensity);
                float4 specular = (_SpecularColor * specularIntensitySmooth) * _SpecularColor.a;
                finalColor += specular;
                #endif

                #if USE_RIM
                float4 rimDot = 1 - dot(viewDir, normal);
                rimDot = pow(rimDot, 10 * _RimShininess);
                finalColor += _RimColor * rimDot * _RimColor.a;
                #endif


                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor; //rimDot* pow(NdotL, 1) + specular + finalColor;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
