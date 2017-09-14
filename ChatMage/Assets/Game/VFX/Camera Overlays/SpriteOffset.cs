using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter)), ExecuteInEditMode]
public class SpriteOffset : MonoBehaviour
{
    public Vector2 Offset
    {
        get { return offset; }
        set { offset = value; ApplyOffset(); }
    }
    public Texture Texture
    {
        get { return texture; }
        set { texture = value; ApplyTexture(); }
    }
    public Color Color
    {
        get { return color; }
        set { color = value; ApplyColor(); }
    }
    public string SortingLayerName
    {
        get { return sortingLayer; }
        set { sortingLayer = value; ApplySortingLayer(); }
    }
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] Texture texture = null;
    [SerializeField] Color color = Color.white;
    [SerializeField] string sortingLayer = "Default";

    [SerializeField, HideInInspector] Material mat;

    private MeshRenderer sprRend;
    private MaterialPropertyBlock propB;

    private void Awake()
    {
        ApplyAll();
        
        //Set mesh to quad
        if(Application.isEditor && !Application.isPlaying)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Filter.sharedMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(gameObject);
        }
    }

    public void ApplyAll()
    {
        if (Render != null)
        {
            if (propB == null)
                propB = new MaterialPropertyBlock();

            Render.GetPropertyBlock(propB);

            propB.SetFloat("_OffsetX", offset.x);
            propB.SetFloat("_OffsetY", offset.y);
            propB.SetColor("_Color", color);
            if (texture != null)
                propB.SetTexture("_MainTex", texture);

            Render.sortingLayerName = sortingLayer;

            if (mat != null)
                ApplyMaterial();

            Render.SetPropertyBlock(propB);
        }
    }

    public Tweener DOColor(Color endValue, float duration)
    {
        return DOTween.To(() => Color, (x) => Color = x, endValue, duration);
    }

    public Tweener DOOffset(Vector2 endValue, float duration)
    {
        return DOTween.To(() => Offset, (x) => Offset = x, endValue, duration);
    }

    private void ApplyColor()
    {
        if (Render != null)
        {
            if (propB == null)
                propB = new MaterialPropertyBlock();

            Render.GetPropertyBlock(propB);
            propB.SetColor("_Color", color);
            Render.SetPropertyBlock(propB);
        }
    }

    private void ApplyTexture()
    {
        if (texture != null && Render != null)
        {
            if (propB == null)
                propB = new MaterialPropertyBlock();

            Render.GetPropertyBlock(propB);
            propB.SetTexture("_MainTex", texture);
            Render.SetPropertyBlock(propB);
        }
    }

    private void ApplyOffset()
    {
        if (Render != null)
        {
            if (propB == null)
                propB = new MaterialPropertyBlock();

            Render.GetPropertyBlock(propB);
            propB.SetFloat("_OffsetX", offset.x);
            propB.SetFloat("_OffsetY", offset.y);
            Render.SetPropertyBlock(propB);
        }
    }

    private void ApplyMaterial()
    {
        if (Render != null)
        {
            Render.sharedMaterial = mat;
        }
    }

    private void ApplySortingLayer()
    {
        if (Render != null)
        {
            Render.sortingLayerName = sortingLayer;
        }
    }

    MeshRenderer Render
    {
        get
        {
            if (sprRend == null)
                sprRend = GetComponent<MeshRenderer>();
            return sprRend;
        }
    }

    MeshFilter Filter
    {
        get
        {
            return GetComponent<MeshFilter>();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteOffset))]
public class SpriteOffsetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            (target as SpriteOffset).ApplyAll();
        }
    }
}
#endif