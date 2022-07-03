using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/MyRenderPipeLineAsset")]
public class MyRenderPipeLineAsset : RenderPipelineAsset
{
    public int value;
    protected override RenderPipeline CreatePipeline()
    {
        return new MyRenderPipeline();
    }
}
