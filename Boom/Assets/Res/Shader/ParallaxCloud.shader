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
		//UNITY_DEFINE_FIXED_ARRAY(float4, _MyVectors, 100);
		float4 _UnLockNodeCenters[30];
		float  _UnLockNodeRadiuss[30];
		float _FadeRanges[30];
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
			ZWrite On
			
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
				o.posWorld.xyz = positionInputs.positionWS;
			
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex) + float2(frac(_Time.y *_HeightTileSpeed.zw));
				o.uv2 = v.texcoord * _HeightTileSpeed.xy;
				//获取转换切线空间矩阵 rotation
				float3 binormalOS = cross(v.normal, v.tangent.xyz) * v.tangent.w;
				float3x3 rotation = float3x3(v.tangent.xyz, binormalOS, v.normal);
				o.normalDir = TransformObjectToWorldNormal(v.normal);
				//世界视角方向（世界顶点-摄像机坐标） 转 表面切线空间
				o.viewDirTS = mul(rotation, GetObjectSpaceNormalizeViewDir(v.vertex));
				o.color = v.color;
				return o;
			}
			
			float4 frag(VertexOutput  i) : SV_TARGET
			{
				//视角向量单位化
				float3 viewRay = normalize(i.viewDirTS*-1);
				viewRay.xy *= _Height;
				//获得深度距离的绝对值
				viewRay.z = abs(viewRay.z)+0.2;

				float3 shadeP = float3(i.uv.xy,0);
				float3 shadeP2 = float3(i.uv2,0);

				float linearStep = 16;
				float4 T = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, shadeP2.xy);
				float h2 = T.a * _HeightAmount;
				
				// linear search
				float3 lioffset = viewRay / (viewRay.z * linearStep);
				float d = 1.0 -  SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, shadeP.xy,0).a * h2;
				float3 prev_d = d;
				float3 prev_shadeP = shadeP;
				
				while(d > shadeP.z)
				{
					prev_shadeP = shadeP;
					shadeP += lioffset;
					prev_d = d;
					d = 1.0 - SAMPLE_TEXTURE2D_LOD(_MainTex,sampler_MainTex, shadeP.xy,0).a * h2;
				}

				float d1 = d - shadeP.z;
				float d2 = prev_d - prev_shadeP.z;
				float w = d1 / (d1 - d2);
				shadeP = lerp(shadeP, prev_shadeP, w);

				half4 c = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,shadeP.xy) * T * _Color;
				half Alpha = lerp(c.a, 1.0, _Alpha) * i.color.r;
				
				float3 normal = normalize(i.normalDir);
				half3 lightDir1 = normalize(_FixedLightDir.xyz);
				half3 lightDir2 = _MainLightPosition.xyz;
				half3 lightDir = lerp(lightDir2, lightDir1, _UseFixedLight);
				float NdotL = max(0,dot(normal,lightDir));
				half3 lightColor = _MainLightColor.rgb;
                half3 finalColor = c.rgb*(NdotL*lightColor + 1.0);

				for (int j = 0; j < 30 ; j++)
				{
					float3 center = _UnLockNodeCenters[j];
					half radius = _UnLockNodeRadiuss[j];
					half fade = _FadeRanges[j];
					float dist = distance(i.posWorld, center.xyz);
					if (dist > radius && dist < (radius + fade))
				    {
				        // 使用 smoothstep 函数来计算平滑的过渡
				        float edge1 = radius;
				        float edge2 = radius + fade;
				        Alpha *= smoothstep(edge1, edge2, dist); // 由外向内过渡至透明
				    }
					else if (dist <= radius)
				    {
				        // 如果在球体内部，那么完全透明
				        Alpha = 0.0;
				    }
				}
                return half4(finalColor.rgb,Alpha);
	
			}
			ENDHLSL
		}
	}

}
