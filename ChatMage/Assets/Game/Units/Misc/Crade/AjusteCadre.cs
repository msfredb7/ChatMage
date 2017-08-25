using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjusteCadre : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject bot;
    public GameObject top;

    private bool state;

    public void SetOpenings(bool leftOpen, bool rightOpen, bool botOpen, bool topOpen)
    {
        if (leftOpen)
            left.SetActive(false);
        else
            left.SetActive(true);

        if (rightOpen)
            right.SetActive(false);
        else
            right.SetActive(true);

        if (botOpen)
            bot.SetActive(false);
        else
            bot.SetActive(true);

        if (topOpen)
            top.SetActive(false);
        else
            top.SetActive(true);
    }

    public void SetPosition(float positionY)
    {
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
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
