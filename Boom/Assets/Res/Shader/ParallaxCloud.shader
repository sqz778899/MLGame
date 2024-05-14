Shader "Boom/Parallax Cloud"
{
	Properties
	{
		_Color("Color",Color) = (1,1,1,1)
		_MainTex("MainTex",2D)="white"{}
		_Alpha("Alpha", Range(0,1)) = 0.5
		_Height("Displacement Amount",range(0,1)) = 0.15
		_HeightAmount("Turbulence Amount",range(0,2)) = 1
		_HeightTileSpeed("Turbulence Tile&Speed",Vector) = (1.0,1.0,0.05,0.0)
		_LightIntensity ("Ambient Intensity", Range(0,3)) = 1.0
		[Toggle] _UseFixedLight("Use Fixed Light", Int) = 1
		_FixedLightDir("Fixed Light Direction", Vector) = (0.981, 0.122, -0.148, 0.0)
	}
	
	SubShader
	{
		//当前为透明
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-50" "RenderPipeline" = "UniversalRenderPipeline" "LightMode" = "UniversalForward"}


		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		//UPR变量
		CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			float _Height;
			float4 _HeightTileSpeed;
			half _HeightAmount;
			half4 _Color;
			half _Alpha;
			half _LightIntensity;
			half4 _LightingColor;
			half4 _FixedLightDir;
			half _UseFixedLight;
		CBUFFER_END
		
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);

		//模型原始数据
		struct VertexInput
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
		    float4 texcoord1 : TEXCOORD1;
		    float4 texcoord2 : TEXCOORD2;
		    float4 texcoord3 : TEXCOORD3;
			half4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		//顶点与表面程序传递变狼
		struct VertexOutput
		{
			float4 vertexCS : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float3 normalDir : TEXCOORD2;
			float3 viewDirTS : TEXCOORD3;
			float4 posWorld : TEXCOORD4;
			half4 color : TEXCOORD5;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};
		ENDHLSL

		pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			VertexOutput  vert(VertexInput v)
			{
				//声明输出变量
				VertexOutput  o;
				//转换物体顶点空间
				VertexPositionInputs positionInputs = GetVertexPositionInputs(v.vertex.xyz);
				o.vertexCS = positionInputs.positionCS;
			
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex) + float2(frac(_Time.y *_HeightTileSpeed.zw));
				o.uv2 = v.texcoord * _HeightTileSpeed.xy;
				//获取转换切线空间矩阵 rotation
				float3 binormalOS = cross(v.normal, v.tangent.xyz) * v.tangent.w;
				float3x3 rotation = float3x3(v.tangent.xyz, binormalOS, v.normal);

				//世界视角方向（世界顶点-摄像机坐标） 转 表面切线空间
				o.viewDirTS = mul(rotation, GetObjectSpaceNormalizeViewDir(v.vertex));
				o.color = v.color;
				return o;
			}


			//表面程序
			float4 frag(VertexOutput  i) : SV_TARGET
			{
				//视角向量单位化
				float3 viewRay = normalize(i.viewDirTS);
				viewRay.xy *= _Height;
				//获得深度距离的绝对值
				viewRay.z = abs(viewRay.z)+ 0.4;

				float3 uv = float3(i.uv.xy,0);
				float3 uv2 = float3(i.uv2,0);

				const int linearStep = 2;
				const int binaryStep = 5;
				float4 T = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2.xy);;
				float h2 = T.a * _HeightAmount;
				// linear search
				float3 lioffset = viewRay / (viewRay.z * (linearStep+1));
				for(int k=0; k<linearStep; k++)
				{
				    float d = 1.0 -  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy).r * h2;
				    uv += lioffset * step(uv.z, d);
				}
				// binary search
				float3 biOffset = lioffset;
				for(int j=0; j<binaryStep; j++)
				{
				    biOffset = biOffset * 0.5;
				    float d = 1.0 - SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy).r * h2;
				    uv += biOffset * sign(d - uv.z);
				}
				half4 _Color = half4(1.0,1.0,1.0,1.0);
				half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy) * T * _Color;
				half Alpha = 0.9;
	
	
				 
				return half4(c.rgb,1);
				/*float4 MainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy);
				float3 minOffset = viewRay/(viewRay.z * _LayerStep);
				float finiNOise = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2.xy).r * MainTex.x;
				float3 prev_uv = uv;
				return float4(i.viewDirTS,1);
				while (finiNOise > uv.z)
				{
					uv += minOffset;
					finiNOise = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float4(uv.xy,0,0)).r * MainTex.r;
				}

				float d1 = finiNOise - uv.z;
				float d2 = finiNOise - prev_uv.z;
				float w = d1/(d1 - d2 + 0.0000001);
				uv = lerp(uv,prev_uv,w);
				half4 resultColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,uv.xy) * MainTex;

				half rangeClt = MainTex.a * resultColor.r + _Alpha * 0.75;
				half Alpha = abs(smoothstep(rangeClt,_Alpha,1.0));
				Alpha = Alpha*Alpha*Alpha*Alpha*Alpha;
				return half4(resultColor.rgb * _ColorDown.rgb * _MainLightColor.rgb,Alpha);*/
			}
			ENDHLSL
		}
	}

}
