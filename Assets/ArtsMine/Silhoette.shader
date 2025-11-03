Shader "Custom/Silhouette"
{
    Properties
    {
        [MainColor] _SColor("SilhouetteColor", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }

        Pass
        {
            ZTest Greater
            Stencil {
                Ref 1
                Comp Equal
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _SColor;
            CBUFFER_END

            float4 vert(float4 i : POSITION) : SV_POSITION
            {
                return TransformObjectToHClip(i.xyz);
            }

            half4 frag() : SV_Target
            {
                return _SColor;
            }
            ENDHLSL
        }
    }
}
