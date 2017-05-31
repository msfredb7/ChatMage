using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAnimation : MonoBehaviour {

    public GameObject background;
    public Transform trf;
    public RectTransform rct;
    public float limit;
    public float drag = 0.5f;
    public float deceleration = 2;
    public float speed;
    public float speedReduction = 100;
    public RectTransform canvas;

    private bool wasTouching = false;
    private Vector2 inputInitialPos;
    private Vector2 backgroundInitialPos;

    void Start()
    {
        rct = background.GetComponent<RectTransform>();
        trf = background.GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (!wasTouching)
            {
                inputInitialPos = touchDeltaPosition;
                wasTouching = true;
            }
            else
            {
                Vector2 newBackgroundDisplacement = touchDeltaPosition - inputInitialPos;
                background.transform.position += new Vector3(newBackgroundDisplacement.x, newBackgroundDisplacement.y);
            }
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            /*
            var force = endPos - startPos;
            force.z = force.magnitude;
            force /= (Time.time - startTime);

            rigidbody.AddForce(force * factor);
            */
        }
        else if (Input.GetMouseButton(0))
        {
            // Get movement of the mouse since last frame
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.Scale(new Vector2(1/canvas.localScale.x, 1/canvas.localScale.y));

            if (!wasTouching)
            {
                backgroundInitialPos = rct.anchoredPosition;
                inputInitialPos = mousePosition;
                wasTouching = true;
            }
            else
            {
                // Displacement
                Vector2 backgroundDisplacement = new Vector2 (mousePosition.x - inputInitialPos.x,0);
                speed = backgroundDisplacement.x / speedReduction;
                rct.anchoredPosition = backgroundInitialPos + backgroundDisplacement;

                // Out of bound?
                if (trf.localPosition.x > 0)
                    rct.anchoredPosition = new Vector2(0,0);
                if (trf.localPosition.x < limit)
                    rct.anchoredPosition = new Vector2(limit, 0);
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            if (wasTouching)
                wasTouching = false;
            else
            {
                rct.anchoredPosition += new Vector2(speed, 0);
                speed /= deceleration;
                if (Mathf.Abs(speed) < drag)
                    speed = 0;

                // Out of bound?
                if (trf.localPosition.x > 0)
                    rct.anchoredPosition = new Vector2(0, 0);
                if (trf.localPosition.x < limit)
                    rct.anchoredPosition = new Vector2(limit, 0);
            }
        }
    }
}
