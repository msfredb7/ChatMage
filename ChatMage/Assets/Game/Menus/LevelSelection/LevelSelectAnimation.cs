using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAnimation : MonoBehaviour {

    public GameObject background;
    public Transform trf;
    public RectTransform rct;
    public List<float> limit = new List<float>();
    public float drag = 0.5f;
    public float deceleration = 2;
    public float speed;
    public float speedReduction = 100;
    public RectTransform canvas;

    private bool wasTouching = false;
    private Vector2 inputInitialPos;
    private Vector2 backgroundInitialPos;
    public int limitIndex = 0;

    void Start()
    {
        rct = background.GetComponent<RectTransform>();
        trf = background.GetComponent<Transform>();

        // Formule pour trouver l'emplace de depart de la camera
        limit[0] = (-306.91f * Mathf.Pow(Camera.main.aspect,2)) + (335.58f * Camera.main.aspect) + 372.96f;
        rct.anchoredPosition = new Vector2(limit[0], 0);
    }

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get movement of the mouse since last frame
            Vector2 fingerPosition = Input.GetTouch(0).position;
            fingerPosition.Scale(new Vector2(1 / canvas.localScale.x, 1 / canvas.localScale.y));

            if (!wasTouching)
            {
                // Initialisation du deplacement
                backgroundInitialPos = rct.anchoredPosition;
                inputInitialPos = fingerPosition;
                wasTouching = true;
            }
            else
            {
                // Displacement
                Vector2 backgroundDisplacement = new Vector2(fingerPosition.x - inputInitialPos.x, 0);
                speed = backgroundDisplacement.x / speedReduction;
                rct.anchoredPosition = backgroundInitialPos + backgroundDisplacement;

                // Out of bound?
                if (trf.localPosition.x > 0)
                    rct.anchoredPosition = new Vector2(0, 0);
                if (trf.localPosition.x < limit[limitIndex])
                    rct.anchoredPosition = new Vector2(limit[limitIndex], 0);
            }
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (wasTouching)
                wasTouching = false;
            else
            {
                // Continue d'avancer meme quand on lache
                rct.anchoredPosition += new Vector2(speed, 0);
                // Ralentissement
                speed /= deceleration;
                if (Mathf.Abs(speed) < drag)
                    speed = 0;

                // Out of bound?
                if (trf.localPosition.x > 0)
                    rct.anchoredPosition = new Vector2(0, 0);
                if (trf.localPosition.x < limit[limitIndex])
                    rct.anchoredPosition = new Vector2(limit[limitIndex], 0);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            // Get movement of the mouse since last frame
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.Scale(new Vector2(1/canvas.localScale.x, 1/canvas.localScale.y));

            if (!wasTouching)
            {
                // Initialisation du deplacement
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
                if (trf.localPosition.x < limit[limitIndex])
                    rct.anchoredPosition = new Vector2(limit[limitIndex], 0);
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            if (wasTouching)
                wasTouching = false;
            else
            {
                // Continue d'avancer meme quand on lache
                rct.anchoredPosition += new Vector2(speed, 0);
                // Ralentissement
                speed /= deceleration;
                if (Mathf.Abs(speed) < drag)
                    speed = 0;

                // Out of bound?
                if (trf.localPosition.x > 0)
                    rct.anchoredPosition = new Vector2(0, 0);
                if (trf.localPosition.x < limit[limitIndex])
                    rct.anchoredPosition = new Vector2(limit[limitIndex], 0);
            }
        }
    }

    public void SetLimitIndex(int index)
    {
        limitIndex = index;
    }
}
