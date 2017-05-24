using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

    private PlayerStats player;
    public GameObject hearthCountainer;
    public GameObject hearth;

    private List<GameObject> hearths = new List<GameObject>();
    private List<GameObject> armor = new List<GameObject>();

    public void Init()
    {
        player = Game.instance.Player.GetComponent<PlayerStats>();
        player.onHit.AddListener(ChangeHP);
        player.onRegen.AddListener(ChangeHP);

        for (int i = 0; i < player.health.MAX; i++)
        {
            hearths.Add(Instantiate(hearth, hearthCountainer.transform));
            hearths[i].GetComponent<HearthScript>().Off();
        }

        // Set initial HP
        ChangeHP();
    }

    // vieu truc pas utiliser
    public void GiveHP(int amount)
    {
        if (amount <= 0)
            return;

        int realAmount = 0;
        if ((amount + player.health) >= player.health.MAX)
            realAmount = player.health.MAX;
        else
            realAmount = (amount + player.health);

        int startingHearth = player.health - 1;
        if (startingHearth < 0)
            startingHearth = 0;
        for (int i = startingHearth; i < realAmount; i++)
        {
            hearths[i].GetComponent<HearthScript>().On();
        }
    }

    void ChangeHP()
    {
        ChangeArmor();

        if (player.health == 0)
            ClearHearths();

        for (int i = 0; i < player.health; i++)
        {
            hearths[i].GetComponent<HearthScript>().On();
        }

        int end = player.health - 1;
        if (end < 0)
            end = 0;
        for (int i = (hearths.Count - 1); i > end ; i--)
        {
            hearths[i].GetComponent<HearthScript>().Off();
        }
    }

    void ChangeArmor()
    {
        if (player.armor == 0)
            RemoveArmor(armor.Count);

        if(player.armor < armor.Count)
        {
            RemoveArmor(armor.Count - player.armor);
        }
        else
        {
            AddArmor(player.armor - armor.Count);
        }
    }

    public void ChangeMaxHP()
    {
        int amount = player.health.MAX - hearthCountainer.transform.childCount;
        if (amount == 0)
            return;
        if (amount < 0)
        {
            for(int i = 0; i < (-1 * amount); i++)
            {
                GameObject deletedHearths = hearths[hearths.Count - 1];
                hearths.Remove(deletedHearths);
                Destroy(deletedHearths);
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject newHearth = Instantiate(hearth, hearthCountainer.transform);
                hearths.Add(newHearth);
                hearths[hearths.Count - 1].GetComponent<HearthScript>().Off();
            }
        }
    }

    public void ClearHearths()
    {
        for (int i = 0; i < hearths.Count; i++)
        {
            hearths[i].GetComponent<HearthScript>().Off();
        }
    }

    public void AddArmor(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newHearth = Instantiate(hearth, hearthCountainer.transform);
            armor.Add(newHearth);
            armor[armor.Count - 1].GetComponent<HearthScript>().On();
            newHearth.GetComponent<Image>().color = Color.black;
        }
    }

    public void RemoveArmor(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject deletedHearths = armor[armor.Count - 1];
            armor.Remove(deletedHearths);
            Destroy(deletedHearths);
        }
    }
}
