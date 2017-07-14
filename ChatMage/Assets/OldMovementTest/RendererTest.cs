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

    private MeshFilter meshFilter;

    private TrailMesh trailMesh;
    //private Vector3 verticies;
    //private Vector3 

    void ApplyMesh()
    {
        if (CheckResources())
        {
            meshFilter.sharedMesh = trailMesh.mesh;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            trailMesh.RemoveLastSegment();
        }
    }

    bool CheckResources()
    {
        if(meshFilter == null)
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

        ApplyMesh();
    }
}
