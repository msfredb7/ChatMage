using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    private PlayerStats playerStats;
    public GameObject hearthCountainer;
    public GameObject hearth;

    private List<HearthScript> hearths = new List<HearthScript>();
    private List<HearthScript> armor = new List<HearthScript>();

    public void Init()
    {
        playerStats = Game.instance.Player.playerStats;
        playerStats.health.onSet.AddListener(ChangeHP);
        playerStats.armor.onSet.AddListener(ChangeArmor);
        playerStats.health.onMaxSet.AddListener(ChangeMaxHP);

        // Set initial HP
        ChangeMaxHP(playerStats.health.MAX);
        ChangeHP(playerStats.health);
        ChangeArmor(playerStats.armor);
    }

    void ChangeHP(int newAmount)
    {
        if (newAmount == 0)
            ClearHearths();

        for (int i = 0; i < newAmount; i++)
        {
            hearths[i].On();
        }

        int end = newAmount - 1;
        if (end < 0)
            end = 0;
        for (int i = (hearths.Count - 1); i > end; i--)
        {
            hearths[i].Off();
        }
    }

    void ChangeArmor(int newAmount)
    {
        if (newAmount == 0)
            RemoveArmor(armor.Count);

        if (newAmount < armor.Count)
        {
            RemoveArmor(armor.Count - newAmount);
        }
        else
        {
            AddArmor(newAmount - armor.Count);
        }
    }

    public void ChangeMaxHP(int newAmount)
    {
        int delta = newAmount - hearthCountainer.transform.childCount;
        if (delta == 0)
            return;
        if (delta < 0)
        {
            for (int i = 0; i < (-1 * delta); i++)
            {
                HearthScript deletedHearths = hearths[hearths.Count - 1];
                hearths.Remove(deletedHearths);
                Destroy(deletedHearths.gameObject);
            }
        }
        else
        {
            for (int i = 0; i < delta; i++)
            {
                HearthScript newHearth = Instantiate(hearth.gameObject, hearthCountainer.transform).GetComponent<HearthScript>();
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
            HearthScript newHearth = Instantiate(hearth, hearthCountainer.transform).GetComponent<HearthScript>();
            armor.Add(newHearth);
            armor[armor.Count - 1].On();
            newHearth.GetComponent<Image>().color = Color.black;
        }
    }

    public void RemoveArmor(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            HearthScript deletedHearths = armor[armor.Count - 1];
            armor.Remove(deletedHearths);
            Destroy(deletedHearths.gameObject);
        }
    }
}
