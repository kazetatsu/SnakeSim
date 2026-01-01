// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

Shader "Custom/SilhouetteMark"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }

        Pass
        {
            ColorMask 0
            Stencil {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 vert(float4 i : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(i.xyz);
            }

            half4 frag() : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
