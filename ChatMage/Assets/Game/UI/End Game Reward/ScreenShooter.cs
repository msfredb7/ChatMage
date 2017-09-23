using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShooter : MonoBehaviour
{

    Action<RenderTexture> resultCallback;

    void Awake()
    {
        enabled = false;
    }

    public void Shoot(Action<RenderTexture> resultCallback)
    {
        enabled = true;
        this.resultCallback = resultCallback;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //new texture
        RenderTexture rndTexture = new RenderTexture(src.width, src.height, src.depth);

        //copy screen to texture
        Graphics.Blit(src, rndTexture);

        //callback with result
        if (resultCallback != null)
            resultCallback(rndTexture);

        //standard blit
        Graphics.Blit(src, dest);

        enabled = false;
    }
}
