using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using System.Reflection;

using UnityEngine.Events;
using CCC.Utility;
using UnityEngine.UI;

public class Fred_TestScript : MonoBehaviour
{
    [System.Serializable]
    public struct Element
    {
        public string name;
        public float value;
    }

    public List<Element> elements = new List<Element>();
    public List<Element> results = new List<Element>();
    public int pickCount = 500;

    void Start()
    {
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            var player = Game.Instance.Player;
            player.vehicle.Attacked(player.playerCarTriggers.frontCol.info, 1, null);
        }
    }
}