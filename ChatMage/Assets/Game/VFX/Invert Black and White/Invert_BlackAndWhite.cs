using System.Collections;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

[ExecuteInEditMode]
public class Invert_BlackAndWhite : MonoBehaviour
{
    public Material material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
