using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsDisplay_Item : MonoBehaviour
{
    public Image image;
    public Sprite defaultSprite;

    public void Display(Item item)
    {
        gameObject.SetActive(true);
        image.sprite = item.ingameIcon ?? defaultSprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
