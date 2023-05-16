using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class PortalRenderer : MonoBehaviour {

    [SerializeField] List<Portal> _portals = new List<Portal>();
    [SerializeField] private CustomPass _pass;

    private void Awake()
    {
        RenderPipelineManager.beginCameraRendering += RenderPortals;
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= RenderPortals;
    }

    public void AddToPortals(Portal pPortal)
    {
        if (!_portals.Contains(pPortal))
        {
            _portals.Add(pPortal);
        }
    }
    
    void RenderPortals(ScriptableRenderContext context, Camera camera)
    {
        for (int i = 0; i < _portals.Count; i++) {
            _portals[i].Render (context);
        }
    }
    

}