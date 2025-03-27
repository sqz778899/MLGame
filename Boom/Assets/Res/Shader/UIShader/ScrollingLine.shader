Shader "UI/Unlit/LineEnergyGrow"
{
    Properties
    {
        _MainTex ("Energy Flow Texture", 2D) = "white" {}
        _ColorGray ("Locked Color", Color) = (0.5,0.5,0.5,1)
        _ColorFill ("Filled Color", Color) = (1,1,1,1)
        _ColorFlow ("Flow Color", Color) = (0,1,1,1)
        _FlowHead ("Flow Head", Float) = 0.0
        _FlowWidth ("Flow Width", Range(0.01,1)) = 0.1
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _EdgeSharpness ("Edge Sharpness", Range(0.1, 10.0)) = 1.5

        _ColorMask ("Color Mask", Float) = 15
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
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
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _ColorGray;
            fixed4 _ColorFill;
            fixed4 _ColorFlow;
            float _FlowHead;
            float _FlowWidth;
            float _ScrollSpeed;
            float _EdgeSharpness;

            float4 _ClipRect;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.worldPosition = v.vertex;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float u = i.uv.x;
                float v = i.uv.y;
                float head = _FlowHead;
                float tail = head - _FlowWidth;

                float yCenter = abs(i.uv.y - 0.5) * 2.0;
                float yFade = 1.0 - pow(yCenter, _EdgeSharpness);

                fixed4 finalColor;

                if (u <= tail)
                {
                    finalColor = _ColorFill * yFade;
                }
                else if (u <= head)
                {
                    float t = saturate((u - tail) / (_FlowWidth + 0.0001));

                    float uvOffset = frac(i.uv.x + _Time.y * _ScrollSpeed);
                    fixed4 flowTex = tex2D(_MainTex, float2(uvOffset, v));

                    float wave = sin((u - tail) * 60 - _Time.y * 20) * 0.5 + 0.5;
                    float pulse = 0.8 + wave * 0.4;

                    fixed4 blendColor = lerp(_ColorFill, _ColorFlow * pulse, t);
                    finalColor = blendColor * flowTex * yFade;
                }
                else
                {
                    finalColor = _ColorGray * yFade;
                }

                #ifdef UNITY_UI_CLIP_RECT
                finalColor.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (finalColor.a - 0.001);
                #endif

                return finalColor * i.color;
            }
            ENDCG
        }
    }
}