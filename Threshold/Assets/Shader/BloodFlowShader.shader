Shader "Custom/BloodFlowShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Float) = 1.0
        _DistortionStrength ("Distortion Strength", Float) = 0.1
        _BloodColor ("Blood Color", Color) = (0.7, 0, 0, 1)
        _FlowNoise ("Flow Noise", 2D) = "white" {}
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
                float4 _BloodColor;
                float _FlowSpeed;
                float _DistortionStrength;
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                TEXTURE2D(_FlowNoise);
                SAMPLER(sampler_FlowNoise);
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
                // UV 좌표에 노이즈와 시간에 따라 왜곡 효과 적용
                float2 noiseUV = input.uv + SAMPLE_TEXTURE2D(_FlowNoise, sampler_FlowNoise, input.uv).rg * _DistortionStrength;
                noiseUV.y += _Time.y * _FlowSpeed;

                // 텍스처 샘플링 및 색상 적용
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, noiseUV);
                baseColor.rgb *= _BloodColor.rgb;

                return baseColor;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
