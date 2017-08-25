using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteCadre : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject bot;
    public GameObject top;

    public void EnableSides(bool leftSide, bool rightSide, bool bottomSide, bool topSide)
    {
        left.SetActive(leftSide);
        right.SetActive(rightSide);
        bot.SetActive(bottomSide);
        top.SetActive(topSide);
    }

    public void SetPaddings(float leftSide, float rightSide, float bottomSide, float topSide)
    {
        float hfH = GameCamera.DEFAULT_SCREEN_HEIGHT / 2 + 0.5f;    //On ajoute .5f pour compenser l'epaisseur des GameObjects
        float hfW = GameCamera.DEFAULT_SCREEN_WIDTH / 2 + 0.5f;
        left.transform.localPosition = new Vector3(-hfW + leftSide, 0, 0);
        right.transform.localPosition = new Vector3(hfW - rightSide, 0, 0);
        bot.transform.localPosition = new Vector3(0, -hfH + bottomSide, 0);
        top.transform.localPosition = new Vector3(0, hfH - topSide, 0);
    }

    public void CenterTo(float positionY)
    {
        transform.position = new Vector3(0, positionY, 0);
    }

    public void Appear()
    {
        gameObject.SetActive(true);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
