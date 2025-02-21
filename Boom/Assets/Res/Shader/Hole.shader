Shader "Custom/CircularHoleShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 0, 0, 1)
        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.2
        _EdgeSoftness ("Edge Softness", Float) = 0.05
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
            float4 _HoleScreenPos;
            float _HoleRadius;
            float _EdgeSoftness;

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
                float2 holeScreen = float2(_HoleScreenPos.x/_ScreenParams.x,_HoleScreenPos.y/_ScreenParams.y);
                // 计算当前像素到洞位置的世界空间距离
                float distanceFromCenter = distance(i.uv, holeScreen);

                float alpha = smoothstep(_HoleRadius - _EdgeSoftness, _HoleRadius + _EdgeSoftness, distanceFromCenter);
                //float alpha = lerp(0,_HoleRadius + _EdgeSoftness,distanceFromCenter);
                // 如果当前像素的距离小于给定的半径，则设置透明
                /*if (distanceFromCenter < _HoleRadius)
                {
                    return float4(0, 0, 0, 0);  // 完全透明
                }*/

                //return half4(ss,1,1);
                
                // 否则显示纯色
                return _Color * alpha;
                
            }

            ENDHLSL
        }
    }

    Fallback "Diffuse"
}
