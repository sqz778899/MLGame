Shader "UI/BorderGlowScroll"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _GlowTex ("GlowTex", 2D) = "white" {}       // 流光贴图
        _GlowColor ("GlowColor", Color) = (1,1,1,1)
        _Speed ("Scroll Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _GlowTex;
            float4 _GlowColor;
            float _Speed;
            float4 _MainTex_ST;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 glowUV : TEXCOORD1;
            };

            float _TimeY;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.glowUV = float2(v.uv.x + _Time.y * _Speed, v.uv.y);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 glow = tex2D(_GlowTex, i.glowUV) * _GlowColor;
                return col + glow;
            }
            ENDCG
        }
    }
}

