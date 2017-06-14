using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CCC.Manager;
using CCC.Utility;
using UnityEngine.UI;
using FullInspector;

public class TestScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public ScreenShooter screenshot;
    public UnityStandardAssets.ImageEffects.BlurOptimized blur;

    //void Awake()
    //{
    //    blur.enabled = false;
    //    meshRenderer.enabled = false;
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            screenshot.Shoot(OnScreenShotTaken);
        }
    }

    void OnScreenShotTaken(RenderTexture texture)
    {
        RenderTexture newTexture = new RenderTexture(texture.width, texture.height, texture.depth);
        blur.RemoteRenderImage(texture, newTexture); 
        meshRenderer.material.mainTexture = newTexture;
        meshRenderer.enabled = true;
    }
}