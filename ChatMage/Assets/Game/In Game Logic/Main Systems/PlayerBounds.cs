using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [System.Serializable]
    public struct FourBoundStates
    {
        public bool top;
        public bool right;
        public bool left;
        public bool bottom;
        public FourBoundStates(bool top, bool right, bool left, bool bottom)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
    }
    [Header("Borders")]
    public bool enabledByDefault = false;
    public BoxCollider2D top;
    public BoxCollider2D bottom;
    public BoxCollider2D right;
    public BoxCollider2D left;

    void Awake()
    {
        if (enabledByDefault)
            EnableAll();
        else
            DisableAll();
    }

    public void Resize(float width, float z)
    {
        right.transform.localPosition = new Vector3(width / 2 + 0.5f, 0, z);
        left.transform.localPosition = new Vector3(-width / 2 - 0.5f, 0, z);
    }

    public void EnableAll()
    {
        top.enabled = true;
        bottom.enabled = true;
        right.enabled = true;
        left.enabled = true;
    }

    public void DisableAll()
    {
        top.enabled = false;
        bottom.enabled = false;
        right.enabled = false;
        left.enabled = false;
    }
    public void SetStates(FourBoundStates states)
    {
        top.enabled = states.top;
        bottom.enabled = states.bottom;
        right.enabled = states.right;
        left.enabled = states.left;
    }

}
