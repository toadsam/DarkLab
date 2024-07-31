Shader "Custom/URPWavyShader"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveFrequency ("Wave Frequency", Float) = 1.0
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
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
            #pragma target 4.5

            // 필요한 인클루드 파일들
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _WaveSpeed;
                float _WaveFrequency;
                float _WaveAmplitude;
                TEXTURE2D(_BaseMap);
                SAMPLER(sampler_BaseMap);
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                output.worldPos = TransformObjectToWorld(input.positionOS).xyz;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 파동 효과 적용
                float wave = sin(_WaveFrequency * (input.worldPos.x + _Time.y * _WaveSpeed)) * _WaveAmplitude;
                input.uv.y += wave;

                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                return baseColor;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
