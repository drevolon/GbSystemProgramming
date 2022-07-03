using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MyRenderPipeline : RenderPipeline
{
    private ScriptableRenderContext _context;
    private Camera _camera;

    private readonly CommandBuffer _commandBuffer=new CommandBuffer { name=bufferName };
    private const string bufferName = "My Camera Render";

    private CullingResults _cullingResults;

    private static readonly List<ShaderTagId> drawingShaderTagIds = new List<ShaderTagId>
    {
        new ShaderTagId("SRPDefaultUnlit")
    };
    #if UNITY_EDITOR
    private static readonly ShaderTagId[] _legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
    private static Material _errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
    private void DrawUnsupportedShaders()
    {
        var drawingSettings = new DrawingSettings(_legacyShaderTagIds[0], new SortingSettings(_camera))
        {
            overrideMaterial = _errorMaterial,
        };

        for (var i = 0; i < _legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, _legacyShaderTagIds[i]);
        }

        var filteringSettings = FilteringSettings.defaultValue;

        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
    }
    #endif
    


    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        _context = context;
        foreach (var camera in cameras)
        {
            Render(context, camera);
        };
    }

    private void Render(ScriptableRenderContext context, Camera camera)
    {
        _camera = camera;
       CommandBuffer commandBuffer = new CommandBuffer() { name = camera.name };
       commandBuffer.BeginSample("121");
       commandBuffer.SetGlobalColor("_GlobalColor", Color.red);
       _context.ExecuteCommandBuffer(commandBuffer);
       commandBuffer.Clear();

        if (!camera.TryGetCullingParameters(out var parameters)) return;
#if UNITY_EDITOR
        DrawUI();
#endif
        _cullingResults = _context.Cull(ref parameters);

        _context.SetupCameraProperties(_camera);

        var drawingSettings = CreateDrawingSettings(drawingShaderTagIds, SortingCriteria.CommonOpaque, out var sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        _context.DrawSkybox(_camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);

#if UNITY_EDITOR
        DrawUnsupportedShaders();
        DrawGizmos();
        
#endif
        // _commandBuffer.SetGlobalColor("BlueColor", Color.blue);
        // _context.ExecuteCommandBuffer(commandBuffer);
       commandBuffer.EndSample("121");
       context.ExecuteCommandBuffer(commandBuffer);
       commandBuffer.Clear();
        _context.Submit();
    }

    

   

    private DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTags, SortingCriteria sortingCriteria, out SortingSettings sortingSettings)
    {
        sortingSettings = new SortingSettings(_camera)
        {
            criteria=sortingCriteria
        };

        var drawingSettings = new DrawingSettings(shaderTags[0], sortingSettings);

        for (var i = 1; i < shaderTags.Count; i++)
        {
            drawingSettings.SetShaderPassName(i, shaderTags[i]);
        }

        return drawingSettings;
    }

    private void DrawGizmos()
    {
        if (!Handles.ShouldRenderGizmos()) return;
        _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
        _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);

    }

    private void DrawUI()
    {
        if (_camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
        }


    }
}