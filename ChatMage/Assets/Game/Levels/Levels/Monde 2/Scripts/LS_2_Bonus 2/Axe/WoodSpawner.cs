using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{

    public GameObject woodPrefab;
    public Transform spawnPoint;
    public event SimpleEvent woodCut;
    public float spawnRate;

    private float countdown;
    private Wood currentWood;
    private CCC.Utility.StatFloat worldTimeScale;

    void Start()
    {
        countdown = 0;
    }

    void Update()
    {
        if (worldTimeScale == null)
        {
            if (Game.Instance != null)
                worldTimeScale = Game.Instance.worldTimeScale;
        }
        else
        {
            if (currentWood == null)
            {
                if (countdown <= 0)
                    SpawnWood();

                countdown -= worldTimeScale * Time.deltaTime;
            }
        }
    }

    private void SpawnWood()
    {
        currentWood = Instantiate(woodPrefab, spawnPoint).GetComponent<Wood>();

        currentWood.OnDeath += delegate (Unit unit)
        {
            woodCut.Invoke();
            countdown = spawnRate;
            currentWood = null;
        };
    }
}
