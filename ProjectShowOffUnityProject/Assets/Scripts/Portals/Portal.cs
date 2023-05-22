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
      //  ScreenMesh = GetComponent<MeshRenderer>();
        RenderPipelineManager.beginCameraRendering += Render;
        //portalCam.enabled = false;
        //CreateViewTexture();
        
    }


    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= Render;
    }

    private void LateUpdate()
    {
     //   Render();
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
           _linkedPortal.ScreenMesh.material.SetTexture("_MainTex", _viewTexture);
           //_linkedPortal.ScreenMesh.material.mainTexture = _viewTexture;
           Debug.Log(_linkedPortal.ScreenMesh.material.mainTexture);
       }
   }

    private void OnPreRender()
    {
      //  Render();
    }
    public void Render()
    {
        if (!CameraUtility.VisibleFromCamera (_linkedPortal.ScreenMesh, playerCam)) {
            return;
        }

        ScreenMesh.enabled = false;
        CreateViewTexture();
        Matrix4x4 m= transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3),m.rotation);
       // portalCam.Render();
        //portalCam.SubmitRenderRequest();
        ScreenMesh.enabled = true;
    }
    public void Render(ScriptableRenderContext arg1, Camera arg2)
    {
        if (!CameraUtility.VisibleFromCamera (_linkedPortal.ScreenMesh, playerCam)) {
            return;
        }

        ScreenMesh.enabled = false;
        CreateViewTexture();
        Matrix4x4 m= transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3),m.rotation);
        //portalCam.Render();
        //portalCam.SubmitRenderRequest(arg1);
        ScreenMesh.enabled = true;
    }
    }


