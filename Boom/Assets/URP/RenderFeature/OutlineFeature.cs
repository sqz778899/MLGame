using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    class OutlineRenderPass : ScriptableRenderPass
    {
        readonly List<ShaderTagId> s_shaderTagIds = new List<ShaderTagId>()
        {
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("UniversalForward"),
            new ShaderTagId("UniversalForwardOnly"),
        };
        static readonly int s_ShaderProp_OutlineMask = Shader.PropertyToID("_OutlineMask");
        readonly Material outlineMaterial;
        readonly FilteringSettings m_filteringSettings;
        readonly MaterialPropertyBlock m_propertyBlock;
        RTHandle m_OutlineMaskRT;
        
        public OutlineRenderPass(Material _outlineMaterial)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            outlineMaterial = _outlineMaterial;
            m_filteringSettings = new FilteringSettings(RenderQueueRange.all, renderingLayerMask:2);
            m_propertyBlock = new MaterialPropertyBlock();
        }

        public void Dispose()
        {
            m_OutlineMaskRT?.Release();
            m_OutlineMaskRT = null;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ResetTarget();
            var des = renderingData.cameraData.cameraTargetDescriptor;
            des.msaaSamples = 1;
            des.depthBufferBits = 0;
            des.colorFormat = RenderTextureFormat.ARGB32;
            RenderingUtils.ReAllocateIfNeeded(ref m_OutlineMaskRT,des,name:"_OutLineMaskRT");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Outline Command");
            
            //draw all the objects thar need the outline effect
            cmd.SetRenderTarget( m_OutlineMaskRT);
            cmd.ClearRenderTarget(true, true, Color.clear);
            var drawingSettings = CreateDrawingSettings(s_shaderTagIds, ref renderingData, SortingCriteria.None);
            var rendererListParams = new RendererListParams(renderingData.cullResults,drawingSettings,m_filteringSettings);
            var list = context.CreateRendererList(ref rendererListParams);
            cmd.DrawRendererList(list);
            
            cmd.SetRenderTarget( renderingData.cameraData.renderer.cameraColorTargetHandle);
            m_propertyBlock.SetTexture(s_ShaderProp_OutlineMask,m_OutlineMaskRT);
            cmd.DrawProcedural(Matrix4x4.identity, outlineMaterial,
                0,MeshTopology.Triangles,3,1,m_propertyBlock);
            
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField]
    Material m_outlineMaterial;
    OutlineRenderPass m_OutlineRenderPassPass;
    
    bool IsMaterialValid => m_outlineMaterial&& m_outlineMaterial.shader 
                                             && m_outlineMaterial.shader.isSupported;

    /// <inheritdoc/>
    public override void Create()
    {
        if (!IsMaterialValid) return;
       
        m_OutlineRenderPassPass = new OutlineRenderPass(m_outlineMaterial);
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_OutlineRenderPassPass == null) return;
        renderer.EnqueuePass(m_OutlineRenderPassPass);
    }

    protected override void Dispose(bool disposing)
    {
        m_OutlineRenderPassPass?.Dispose();
    }
}


