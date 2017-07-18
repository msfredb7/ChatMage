using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMesh
{
    public Mesh mesh;

    public int[] triangles;
    public Vector3[] vertices;
    public Vector2[] uv;
    public Vector3[] normals;

    private LinkedList<HalfQuad> halfQuads = new LinkedList<HalfQuad>();
    private float uvPerWorldUnit = 0.5f;
    private float currentUVDist = 0;

    public TrailMesh(float uvPerWorldUnit, Vector2 firstPoint, float firstWidth)
    {
        mesh = new Mesh();
        AddSegment(firstPoint, firstWidth);
        this.uvPerWorldUnit = uvPerWorldUnit;
    }

    public void AddSegment(Vector2 point, float width)
    {
        float vLength = 0;
        float angle = 0;
        if (halfQuads.Count == 0)
        {
            angle = 0;
        }
        else
        {
            HalfQuad previous = halfQuads.Last.Value;
            Vector2 v = point - previous.center;

            vLength = v.magnitude;

            angle = v.ToAngle();

            if (halfQuads.Count == 1)
            {
                //Influence previous 100%
                previous.ChangeAngle(angle);
            }
            else
            {
                //Influence previous 50%
                previous.ChangeAngle((previous.angle.ToVector() + angle.ToVector()).ToAngle());
            }
        }

        currentUVDist += vLength * uvPerWorldUnit;

        if (angle < 0)
            angle += 360;

        HalfQuad newHalfQuad = new HalfQuad();
        newHalfQuad.center = point;
        newHalfQuad.ApplyAngle(angle, width);
        newHalfQuad.uvX = currentUVDist;

        halfQuads.AddLast(newHalfQuad);

        if (halfQuads.Count > 1)
            ApplyMesh(true);
    }

    public void RemoveLastSegment()
    {
        if (halfQuads.Count <= 1)
            return;

        halfQuads.RemoveFirst();
        ApplyMesh(false);
    }

    public void AdjustFirstSegment(Vector2 newPos)
    {

    }

    public void AdjustLastSegment(float size)
    {
        LinkedListNode<HalfQuad> node = halfQuads.First;
        HalfQuad hq = node.Value;
        hq.ApplyWidth(size*0);

        vertices[0] = hq.a;
        vertices[1] = hq.b;
        uv[0] = Vector2.zero;
        uv[1] = Vector2.zero;


        node = node.Next;
        hq = node.Value;

        hq.ApplyWidth(size*0.333f);

        vertices[2] = hq.a;
        vertices[3] = hq.b;


        node = node.Next;
        hq = node.Value;

        hq.ApplyWidth(size*0.666f);

        vertices[4] = hq.a;
        vertices[5] = hq.b;


        ApplyVerticles();
        ApplyUV();
    }

    private void ApplyMesh(bool ajout)
    {
        /*int[]*/
        triangles = new int[(halfQuads.Count - 1) * 6];
        /*Vector3[]*/
        vertices = new Vector3[halfQuads.Count * 2];
        /*Vector2[]*/
        uv = new Vector2[vertices.Length];
        /*Vector3[]*/
        normals = new Vector3[vertices.Length];

        LinkedListNode<HalfQuad> node = halfQuads.First;

        int i = 0;
        while (node != null)
        {
            HalfQuad hq = node.Value;
            int x = 2 * i;
            int y = 2 * i + 1;

            //Vertices
            vertices[x] = hq.a;
            vertices[y] = hq.b;

            //Uv
            uv[x] = new Vector2(hq.uvX, 1);
            uv[y] = new Vector2(hq.uvX, 0);

            //Normals
            normals[x] = -Vector3.forward;
            normals[y] = -Vector3.forward;

            //Triangles
            if (i < halfQuads.Count - 1)
            {
                triangles[i * 6] = x;
                triangles[i * 6 + 1] = x + 2;
                triangles[i * 6 + 2] = x + 1;
                triangles[i * 6 + 3] = x + 1;
                triangles[i * 6 + 4] = x + 2;
                triangles[i * 6 + 5] = x + 3;
            }

            node = node.Next;
            i++;
        }

        if (!ajout)
            mesh.triangles = triangles;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        if (ajout)
            mesh.triangles = triangles;
    }

    private void ApplyVerticles()
    {
        mesh.vertices = vertices;
    }
    private void ApplyUV()
    {
        mesh.uv = uv;
    }

    private class HalfQuad
    {
        public Vector2 a;
        public Vector2 b;
        public float angle;
        public Vector2 center;
        public float uvX;

        public void ApplyAngle(float angle, float width)
        {
            a = (angle + 90).ToVector() * width + center;
            b = (angle - 90).ToVector() * width + center;
            this.angle = angle;
        }

        public void ApplyWidth(float width)
        {
            a = (a - center).normalized * width + center;
            b = (b - center).normalized * width + center;
        }

        public void ChangeAngle(float newAngle)
        {
            float delta = newAngle - angle;
            angle = newAngle;
            a = (a - center).Rotate(delta) + center;
            b = (b - center).Rotate(delta) + center;
        }
    }
}
