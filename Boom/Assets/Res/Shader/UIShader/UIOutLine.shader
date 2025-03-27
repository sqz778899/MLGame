Shader "Unlit/UIOutLine"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        _OutLine ("OutLine", Float) = 1
        [HDR]_OutLineColor ("OutLine Color", Color) = (1,1,1,1)
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
           Name "Default"
            HLSLPROGRAM
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
             
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "UnityUI.cginc"
            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
		    SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float4 _Color;
            float4 _TextureSampleAdd;
            float4 _ClipRect;
            half _OutLine;
            half4 _OutLineColor;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                o.worldPosition = v.vertex;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
               
                o.texcoord  = TRANSFORM_TEX(v.texcoord , _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            inline float UnityGet2DClipping (in float2 position, in float4 clipRect)
            {
                float2 inside = step(clipRect.xy, position.xy) * step(position.xy, clipRect.zw);
                return inside.x * inside.y;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 color = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,i.texcoord )+ _TextureSampleAdd) * i.color;

                float2 uv_up = i.texcoord + _MainTex_ST.xy * float2(0,1) * _OutLine;
                float2 uv_down = i.texcoord + _MainTex_ST.xy * float2(0,-1) * _OutLine;
                float2 uv_left = i.texcoord + _MainTex_ST.xy * float2(-1,0) * _OutLine;
                float2 uv_right = i.texcoord + _MainTex_ST.xy * float2(1,0) * _OutLine;

                float w = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,uv_up).a *
                    SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, uv_down).a*
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,uv_left).a *
                    SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv_right).a;
                
                color.rgb = lerp(_OutLineColor.rgb,color.rgb,w);
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                
                return color;
            }
            ENDHLSL
        }
    }
}
