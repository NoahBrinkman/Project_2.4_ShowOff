using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class Portal : MonoBehaviour
{
    //Matrix4x4 m= red. localToWorIdMatrix * blue.worldToLocalMatrix * playerCam.localToWorldMatrix;

    [SerializeField] private Portal _linkedPortal;

     public MeshRenderer ScreenMesh;
    
    [SerializeField] private Camera portalCam;
    [SerializeField] private Camera playerCam;
    
    private RenderTexture _viewTexture;

    private void Awake()
    {
        ScreenMesh = GetComponent<MeshRenderer>();
        portalCam.enabled = false;
    }


    void CreateViewTexture()
   {
       if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
       {
           if (_viewTexture != null)
           {
               _viewTexture.Release();
           }

           _viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
           portalCam.targetTexture = _viewTexture;
           _linkedPortal.ScreenMesh.material.mainTexture = _viewTexture;
           Debug.Log(_linkedPortal.ScreenMesh.material.mainTexture);
       }
   }


    public void Render(ScriptableRenderContext context)
    {
        ScreenMesh.enabled = false;
        CreateViewTexture();
        Matrix4x4 m= transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3),m.rotation);
        portalCam.SubmitRenderRequest(context);
        ScreenMesh.enabled = true;
    }
}
