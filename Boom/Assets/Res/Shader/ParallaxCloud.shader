Shader "Boom/Parallax Cloud"
{
	Properties
	{
		[Header(Color)]
		[HDR]_ColorUp("顶部颜色",Color) = (1,1,1,1)
		[HDR]_ColorDown("底部颜色",Color) = (0,0,0,1)
		[Space(10)]
		[Header(Parallax)]
		_MainTex("噪波贴图",2D) = "white"{}
		_Height("深度",range(0,1)) = 0.15
		_LayerStep("层数",Range(1,256)) = 16
		[Space(10)]
		[Header(Show)]
		_DistanceMax("最大距离",Range(0,1)) = 0.5
		_DistanceMin("最小距离",Range(0,1)) = 0.5

		[Header(Animation)]
		_MoveSpeedX("贴图移动X",Range(0,1)) = 0.5
		_MoveSpeedY("贴图移动Y",Range(0,1)) = 0.5
		
		_Alpha("Alpha",Range(0,1)) = 0.5
	}
	
	SubShader
	{
		//当前为透明
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent-50" "RenderPipeline" = "UniversalRenderPipeline" "LightMode" = "UniversalForward"}


		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		//UPR变量
		CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;//纹理重复
			float4 _MainTex_TexelSize;//纹理像素数据（1/x,1/y,x,y）
			float _Height;//高度
			float4 _ColorUp;//顶部颜色
			float4 _ColorDown;//底部颜色
			float _LayerStep;//层数
			float _DistanceMax;//UV距离限制
			float _DistanceMin;//UV距离限制
			float _MoveSpeedX;
			float _MoveSpeedY;
			float _Alpha;
		CBUFFER_END

			//贴图数据
			TEXTURE2D(_MainTex);
		//贴图容器
		SAMPLER(sampler_MainTex);

		//模型原始数据
		struct VertexInput
		{
			float4 vertexOS : POSITION;//模型顶点坐标
			float3 normalOS : NORMAL;//顶点保存的物体空间的法线数据
			float4 tangentOS : TANGENT;//顶点保存的物体空间的切线数据
			float2 uv : TEXCOORD0;//顶点保存的UV数据
			float2 uv2 : TEXCOORD1;//顶点保存的UV数据

		};

		//顶点与表面程序传递变狼
		struct VertexOutput
		{
			float4 vertexCS : SV_POSITION;
			float4 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float3 viewDirTS : TEXCOORD2;
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
				VertexPositionInputs positionInputs = GetVertexPositionInputs(v.vertexOS.xyz);
				//顶点的裁切空间
				o.vertexCS = positionInputs.positionCS;
				//贴图重复
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = v.uv;
				o.uv2 = v.uv2;
				//获取转换切线空间矩阵 rotation
				float3 binormalOS = cross(v.normalOS, v.tangentOS.xyz) * v.tangentOS.w;
				float3x3 rotation = float3x3(v.tangentOS.xyz, binormalOS, v.normalOS);

				//世界视角方向（世界顶点-摄像机坐标） 转 表面切线空间
				o.viewDirTS = mul(rotation, GetObjectSpaceNormalizeViewDir(v.vertexOS));
				//输出
				return o;
			}


			//表面程序
			float4 frag(VertexOutput  i) : SV_TARGET
			{
				//视角向量单位化
				float3 viewRay = normalize(i.viewDirTS);
				viewRay.xy *= _Height;
				//获得深度距离的绝对值
				viewRay.z += 0.4;

				float3 uv = float3(i.uv.xy,0);
				float3 uv2 = float3(i.uv2,0);
				
				float4 MainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv.xy);
				float3 minOffset = viewRay/(viewRay.z * _LayerStep);
				float finiNOise = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2.xy).r * MainTex.x;
				float3 prev_uv = uv;

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
				return half4(resultColor.rgb * _ColorDown.rgb * _MainLightColor.rgb,Alpha);

				//获取整数层
				_LayerStep = ceil(_LayerStep);
				//对距离翻倍,获取0~1范围的偏移步长
				float3 _Offset = viewRay / (viewRay.z * _LayerStep);
				//初始化亮度为最大值
				float d = 1.0;
				//组织UV，Z为平面距离
				float3 _UV3 = float3(i.uv.xy+float2(_MoveSpeedX, _MoveSpeedY)*_Time.x,0);
				//构建抖动顺序矩阵
				float2x2 thresholdMatrix =
				{ 1.0 / 5.0,  3.0 / 5.0,
				  4.0 / 5.0,  2.0 / 5.0};
				//创建遮罩矩阵
				float2x2 _RowAccess = { 1,0,0,1};
				//获取贴图像素坐标
				float2 pos = _MainTex_TexelSize.zw * i.uv.xy*2;
				//依据动态抖动计算透明度
				float Dither = thresholdMatrix[fmod(pos.x, 2)] * _RowAccess[fmod(pos.y, 2)] / _LayerStep;
				//图像亮度与(当前深度+像素抖动）对比
				while (d > _UV3.z - Dither)
				{
					//探索下一层图像
					_UV3 += _Offset;
					//使用新的UV获取图像，翻转亮度
					d = 1 - SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, _UV3.xy,0).r;
				}
				//获得偏移后的贴图纹理
				float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _UV3.xy);
				//渐变着色
				c = lerp(_ColorDown, _ColorUp, c.r);
				//uv计算距离衰减
				float distance = length(i.uv.zw*2-1);
				distance =1 - smoothstep(
					min(_DistanceMin, _DistanceMax),
					max(_DistanceMin, _DistanceMax),
					distance);
				//透明混合衰减
				c.a *= distance;
				return c;

			}
			ENDHLSL
		}
	}

}
