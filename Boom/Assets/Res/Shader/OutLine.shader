Shader "Unlit/OutLine"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _OutLine ("OutLine", Float) = 1
        [HDR]_Color ("OutLine Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        
        Cull Off

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _BaseColor;
            half4 _Color;
            half _OutLine;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;

                float2 uv_up = i.uv + _MainTex_ST * float2(0,1) * _OutLine;
                float2 uv_down = i.uv + _MainTex_ST * float2(0,-1) * _OutLine;
                float2 uv_left = i.uv + _MainTex_ST * float2(-1,0) * _OutLine;
                float2 uv_right = i.uv + _MainTex_ST * float2(1,0) * _OutLine;

                float w = tex2D(_MainTex, uv_up).a * tex2D(_MainTex, uv_down).a
                * tex2D(_MainTex, uv_left).a * tex2D(_MainTex, uv_right).a;
       
                col.rgb = lerp(_Color.rgb,col.rgb,w);
              
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
