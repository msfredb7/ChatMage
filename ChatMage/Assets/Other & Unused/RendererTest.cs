using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
public class RendererTest : MonoBehaviour
{
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
    public Vector3[] newNormals;
    public float width = 1;

    [Header("Animation")]
    public Texture[] sprites;
    public float animationSpeed = 1;

    private MeshFilter meshFilter;

    private TrailMesh trailMesh;
    private int nextSpriteIndex = 1;


    void ApplyMesh()
    {
        if (CheckResources())
        {
            meshFilter.sharedMesh = trailMesh.mesh;
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        trailMesh.RemoveLastSegment();
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        trailMesh.AdjustLastSegment(width*=0.9f);
    //    }

    //}

    bool CheckResources()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
                return false;
        }
        if (trailMesh == null)
        {
            trailMesh = new TrailMesh(1, Vector2.zero, width);
            if (trailMesh == null)
                return false;
        }
        return true;
    }

    void OnEnable()
    {
        trailMesh = null;
        if (!CheckResources())
            return;


        //var mesh = new Mesh();

        //meshFilter.mesh = mesh;
        for (int i = 0; i < transform.childCount; i++)
        {
            trailMesh.AddSegment(transform.GetChild(i).localPosition, width);
        }

        newVertices = trailMesh.vertices;
        newUV = trailMesh.uv;
        newNormals = trailMesh.normals;
        newTriangles = trailMesh.triangles;

        StopAllCoroutines();
        if (Application.isPlaying)
            StartCoroutine(SpriteAnimation());

        ApplyMesh();
    }

    void SetSprite(int index)
    {
        if (nextSpriteIndex >= sprites.Length)
            nextSpriteIndex -= sprites.Length;

        if (nextSpriteIndex > 0)
            GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", sprites[nextSpriteIndex]);

        nextSpriteIndex++;
    }

    IEnumerator SpriteAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / animationSpeed);

            SetSprite(nextSpriteIndex);
        }
    }
}
