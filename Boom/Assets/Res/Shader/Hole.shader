Shader "Custom/CircularHoleShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 0, 0, 1)
        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.2
    }

    SubShader
    {
         Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;
            float4 _HoleWorldPos;
            float _HoleRadius;

            // World to Object transformation
            float4x4 _ObjectToWorld;
            float4x4 _WorldToCamera;
            float4x4 _WorldToObject;

            struct Attributes
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.position = TransformObjectToHClip(input.position.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // 将世界坐标转换为对象空间坐标
                //float3 worldPos = mul(_ObjectToWorld, float4(i.uv, 0.0, 1.0)).xyz;
                //float3 holeViewSpace = TransformWorldToView(_HoleWorldPos).xyz;
                float4 holeViewCS = TransformWorldToHClip(_HoleWorldPos);

                float4 ndc = holeViewCS * 0.5f;
                float2 ss = float2(ndc.x, ndc.y * _ProjectionParams.x) + ndc.w;
                //float4 screenPos = ComputeScreenPos(holeViewCS);
                float2 oo = (ndc*2 - 1).xy;

                float2 screenPos = (ndc.xy + float2(1.0, 1.0)) * 0.5 * float2(_ScreenParams.x , _ScreenParams.y);

                // 计算当前像素到洞位置的世界空间距离
                float distanceFromCenter = distance(i.uv*2 - 1, ndc.xy);

                // 如果当前像素的距离小于给定的半径，则设置透明
                if (distanceFromCenter < 0.1)
                {
                    return float4(0, 0, 0, 0);  // 完全透明
                }

                //return half4(ss,1,1);
                
                // 否则显示纯色
                return _Color;
                
            }

            ENDHLSL
        }
    }

    Fallback "Diffuse"
}
