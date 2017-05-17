using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthDisplay : MonoBehaviour {

    private PlayerStats player;
    public GameObject hearthCountainer;
    public GameObject hearth;

    private List<GameObject> hearths = new List<GameObject>();

    public LevelScript defaultLevelScript;

    void Start()
    {
        if (Scenes.SceneCount() == 1)
        {
            MasterManager.Sync(delegate ()
            {
                Scenes.Load("Framework", LoadSceneMode.Additive, DebugInit);
            });
        }
    }

    void DebugInit(Scene scene)
    {
        Framework framework = Scenes.FindRootObject<Framework>(scene);
        framework.Init(defaultLevelScript);
    }

    public void Init()
    {
        player = Game.instance.Player.GetComponent<PlayerStats>();
        player.onHit.AddListener(ChangeHP);

        for (int i = 0; i < player.health.MAX; i++)
        {
            hearths.Add(Instantiate(hearth, hearthCountainer.transform));
            hearths[i].GetComponent<HearthScript>().Off();
        }

        // Set initial HP
        GiveHP(player.health);
    }

    public void GiveHP(int amount)
    {
        if (amount <= 0)
            return;

        int realAmount = 0;
        if ((amount + player.health) >= player.health.MAX)
            realAmount = player.health.MAX;
        else
            realAmount = (amount + player.health);

        for (int i = player.health-1; i < realAmount; i++)
        {
            hearths[i].GetComponent<HearthScript>().On();
        }
    }

    void ChangeHP()
    {

    }
}
