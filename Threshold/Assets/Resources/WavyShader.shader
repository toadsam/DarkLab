Shader "Custom/URPWavyShader"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _HeightMap("Height Map", 2D) = "black" {}
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _MetallicGlossMap("Metallic", 2D) = "white" {}
        _Frequency("Frequency", Float) = 1.0
        _Amplitude("Amplitude", Float) = 0.1
        _Speed("Speed", Float) = 1.0
        _CustomTime("Custom Time", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);
            TEXTURE2D(_HeightMap);
            SAMPLER(sampler_HeightMap);
            TEXTURE2D(_OcclusionMap);
            SAMPLER(sampler_OcclusionMap);
            TEXTURE2D(_MetallicGlossMap);
            SAMPLER(sampler_MetallicGlossMap);
            float _Frequency;
            float _Amplitude;
            float _Speed;
            float _CustomTime;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);

                // Apply wavy effect to UV coordinates
                OUT.uv = IN.uv;
                OUT.uv.y += sin(_CustomTime * _Speed + OUT.uv.x * _Frequency) * _Amplitude;

                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangentOS.w;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample textures
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 normalMap = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv));
                half occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, IN.uv).r;
                half4 metallicGloss = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, IN.uv);

                // Transform normal map to world space
                half3 worldNormal = normalize(normalMap.x * IN.tangentWS + normalMap.y * IN.bitangentWS + normalMap.z * IN.normalWS);

                // Create SurfaceData
                SurfaceData surfaceData;
                surfaceData.albedo = baseColor.rgb;
                surfaceData.normalWS = worldNormal;
                surfaceData.occlusion = occlusion;
                surfaceData.metallic = metallicGloss.r;
                surfaceData.smoothness = metallicGloss.a;

                // Lighting calculation
                half3 viewDirection = normalize(_WorldSpaceCameraPos - TransformObjectToWorld(IN.positionHCS).xyz);
                half4 color = UniversalFragmentBlinnPhong(surfaceData, worldNormal, viewDirection);

                color.a = baseColor.a;
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}