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
            results = new List<Element>(elements);

            //Reset results
            for (int i = 0; i < results.Count; i++)
            {
                var r = results[i];
                r.value = 0;
                results[i] = r;
            }

            for (int u = 0; u < pickCount; u++)
            {
                Lottery<Element> lottery = new Lottery<Element>();

                for (int i = 0; i < elements.Count; i++)
                {
                    lottery.Add(elements[i], elements[i].value);
                }

                string namePicked = lottery.Pick().name;

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].name == namePicked)
                    {
                        var r = results[i];
                        r.value += 1f / pickCount;
                        results[i] = r;
                    }
                }
            }
        }
    }
}