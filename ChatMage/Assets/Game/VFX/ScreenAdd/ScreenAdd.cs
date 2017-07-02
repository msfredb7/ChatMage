using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenAdd : PostEffectsBase
{
    public Color color = Color.black;
    public Shader shader;
    private Material mat = null;

    void Awake()
    {
        shader = Shader.Find("CCC/ScreenAdd");
    }

    public override bool CheckResources()
    {
        CheckSupport(false);

        mat = CheckShaderAndCreateMaterial(shader, mat);

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (CheckResources() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }

        mat.SetColor("_Color", color);
        //mat.SetVector("_UV_Transform", UV_Transform);
        //mat.SetFloat("_Intensity", intensity);
        //mat.SetTexture("_Overlay", texture);
        Graphics.Blit(source, destination, mat);
    }
}