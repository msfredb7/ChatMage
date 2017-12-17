using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;

public class Fred_TestScript : MonoBehaviour
{
    public int samples = 50;
    public int tries = 300;
    public PseudoRand random = new PseudoRand(0.3f, 1f);

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int prediction = (tries * random.SuccessRate).RoundedToInt();
            int ecarts = 0;
            for (int u = 0; u < samples; u++)
            {
                int successes = 0;
                for (int i = 0; i < tries; i++)
                {
                    if (random.PickResult())
                        successes++;
                }
                ecarts += (successes - prediction).Abs();
            }
            print("Ecart moyen: " + ((float)ecarts / samples));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            print(random.PickResult());
        }
    }
}