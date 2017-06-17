using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ColorAdjustments : MonoBehaviour
{
    const string SHADERNAME = "Custom/HSVRangeShader";

    [Range(0, 1)]
    public float affectRangeMin = 0;
    [Range(0, 1)]
    public float affectRangeMax = 1;
    [Range(0, 1)]
    public float hueShift = 0;
    [Range(-1, 1)]
    public float saturation = 0;
    [Range(-1, 1)]
    public float value = 0;
    [Range(-1, 1)]
    public float alpha = 0;
    [HideInInspector]
    public Shader hsvShader;

    void Awake()
    {
        if (Application.isPlaying)
            return;

        hsvShader = Shader.Find(SHADERNAME);
        if (hsvShader == null)
            throw new System.Exception("Could not find the shader: " + SHADERNAME);

        if (GetComponent<Image>() != null)
            GetComponent<Image>().material = new Material(hsvShader);
        else if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sharedMaterial = new Material(hsvShader);

        Apply();
    }

    public void Verify()
    {
        if (affectRangeMax < affectRangeMin)
            affectRangeMax = affectRangeMin;
    }

    public void Apply()
    {
        Verify();

        Image image = GetComponent<Image>();
        SpriteRenderer sprRenderer = GetComponent<SpriteRenderer>();

        Material mat;
        if (image != null)
            mat = image.material;
        else
            mat = sprRenderer.sharedMaterial;

        if (mat.shader != Shader.Find(SHADERNAME))
            mat = new Material(hsvShader);

        mat.SetFloat("_HSVRangeMin", affectRangeMin);
        mat.SetFloat("_HSVRangeMax", affectRangeMax);
        mat.SetVector("_HSVAAdjust", new Vector4(hueShift, saturation, value, alpha));
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(ColorAdjustments))]
public class ColorAdjustmentsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
            (target as ColorAdjustments).Apply();
    }
}
#endif
