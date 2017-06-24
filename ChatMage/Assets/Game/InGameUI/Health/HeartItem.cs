using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HeartItem : MonoBehaviour
{
    public enum HeartType { Full = 0, Empty = 1, Armor = 2 }
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Sprite armor;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Display(HeartType type)
    {
        switch (type)
        {
            case HeartType.Full:
                image.sprite = fullHeart;
                break;
            case HeartType.Empty:
                image.sprite = emptyHeart;
                break;
            case HeartType.Armor:
                image.sprite = armor;
                break;
        }

        image.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
    }
}
