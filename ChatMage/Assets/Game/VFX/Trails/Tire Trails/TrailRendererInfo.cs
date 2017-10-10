using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererInfo : MonoBehaviour
{
    public TrailRenderer trailRenderer;

    void Update()
    {
        try
        {
            int i = trailRenderer.GetPositions(new Vector3[1]);
            print("stopped: " + i);
        }
        catch (System.Exception)
        {
            print("running");
        }
    }
}
