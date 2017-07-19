using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NAV_SmartMover : MonoBehaviour
{
    public abstract Vector2 Smartify(RaycastHit2D hit);

    public static Vector2 SmartifyMove(Vector2 start, Vector2 destination, float unitWidth)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, destination, 1 << Layers.NAVIGATION);

        if (hit.transform != null)
        {
            NAV_SmartMover sw = hit.transform.GetComponent<NAV_SmartMover>();
            if (sw != null)
            {
                return sw.Smartify(hit);
            }
        }

        return destination;
    }
}
