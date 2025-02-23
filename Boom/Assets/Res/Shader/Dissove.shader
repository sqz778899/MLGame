Shader "Custom/2D_Dissolve"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _DissolveTexture ("Dissolve Mask", 2D) = "white" { }
        _DissolveAmount ("Dissolve Amount", Range(-1, 1)) = 0.5
        _EdgeWidth ("Edge Width", Range(0, 1)) = 0.1
        //添加方向控制
        _Flip("Flip Dissolve Dir",Int) = 0
        _DissolveDirection ("Dissolve Direction", Vector) = (1, 0, 0, 0) // 默认 X 轴方向
        _EdgeSoftness ("Edge Softness", Range(0, 1)) = 0.5 // 添加柔边控制
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
            TEXTURE2D(_DissolveTexture);
            SAMPLER(sampler_DissolveTexture);
            
            half _DissolveAmount;
            half _EdgeWidth;
            half4 _DissolveDirection; // 新增的溶解方向属性
            half _EdgeSoftness; // 柔边度控制
            int _Flip;

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
                half4 col2 = SAMPLE_TEXTURE2D(_DissolveTexture,sampler_DissolveTexture,i.uv);
                float dissolve = col2.a;

                float dissolveWithDirection = 0;
                if (_Flip == 0)
                {
                    dissolveWithDirection = lerp(1,0,dot(i.uv, _DissolveDirection.xy));
                }
                else
                {
                    dissolveWithDirection = _Flip - lerp(1,0,dot(i.uv, _DissolveDirection.xy));
                }
                dissolve -= dissolveWithDirection;
                //dissolve = saturate(dissolve);

                // 基于UV计算四个方向的边缘距离
                float leftEdge = i.uv.x;
                float rightEdge = 1.0 - i.uv.x;
                float topEdge = i.uv.y;
                float bottomEdge = 1.0 - i.uv.y;

                // 计算最小边缘距离（越接近边缘，Alpha越低）
                float edgeDistance = min(min(leftEdge, rightEdge), min(topEdge, bottomEdge));

                // 根据距离和柔边度调整Alpha值
                float edgeAlpha = smoothstep(_EdgeSoftness, 0.0, edgeDistance);

                half alpha = 0;
                if (dissolve < _DissolveAmount){alpha = 0;}
                else{alpha = smoothstep(_DissolveAmount - _EdgeWidth, _DissolveAmount + _EdgeWidth, dissolve);}
                
                //return dissolveWithDirection.rrrr;
                alpha *= (1-edgeAlpha);
                return half4(col2.rgb,alpha);
            }   

            ENDHLSL
        }
    }

    Fallback "Diffuse"
}
