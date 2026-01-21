Shader "Custom/URP/HallwayDarknessFade"
{
    Properties
    {
        // Where the fade starts along the chosen axis (object space units)
        _Start ("Fade Start (Object Units)", Float) = 0

        // How wide the fade is (object space units). Must be > 0.
        _Width ("Fade Width (Object Units)", Float) = 1

        // 0 = no darkening, 1 = fully black at the end
        _Strength ("Darkness Strength", Range(0,1)) = 0.75

        // If 1, flips the fade direction
        _Invert ("Invert Fade", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Name "MultiplyFade"
            Tags { "LightMode"="UniversalForward" }

            // Multiply blend: final = src * dst
            Blend DstColor Zero

            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _Start;
                float _Width;
                float _Strength;
                float _Invert;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionOS  : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionOS  = IN.positionOS.xyz;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Fade axis: object-space Z by default (good for hallway end cards).
                // If your quad is oriented differently, switch to IN.positionOS.x
                float axis = IN.positionOS.z;

                // Avoid divide-by-zero if width is accidentally 0
                float w = max(_Width, 1e-5);

                // 0..1 mask across the mesh
                float t = saturate((axis - _Start) / w);

                // Optional invert
                t = lerp(t, 1.0 - t, saturate(_Invert));

                // Smooth the gradient a bit (nice soft fade)
                t = t * t * (3.0 - 2.0 * t); // smoothstep(0,1,t)

                // Compute multiply color:
                // 1 = no change, lower = darker. At t=1, multiplier = 1 - Strength.
                float darknessMul = lerp(1.0, 1.0 - _Strength, t);

                // Multiply blending uses RGB; alpha doesn’t matter much here
                return half4(darknessMul, darknessMul, darknessMul, 1.0);
            }
            ENDHLSL
        }
    }
}
