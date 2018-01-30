using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;

using UnityEngine.Events;
using CCC.Utility;
using UnityEngine.UI;

public class Fred_TestScript : MonoBehaviour
{
    public Sprite icon;
    public ItemsDisplay_Controller controller;
    public List<Image> images = new List<Image>();

    void Start()
    {
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            images.Add(controller.AddItem(icon));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            controller.RemoveItem(images[0]);
            images.RemoveAt(0);
        }
    }
}