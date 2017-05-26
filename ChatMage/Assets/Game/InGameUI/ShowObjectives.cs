using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectives : MonoBehaviour {

    private List<GameObject> objectives = new List<GameObject>();
    public GameObject objectivesCountainer;
    public GameObject textPrefab;

    public Objective AddObjective(string text)
    {
        GameObject newObjective = Instantiate(textPrefab, objectivesCountainer.transform);
        newObjective.GetComponent<Objective>().SetObjective(text);
        objectives.Add(newObjective);
        newObjective.GetComponent<Objective>().UpdateObjective();
        return newObjective.GetComponent<Objective>();
    }

    public Objective AddObjective(string text, int currentResult)
    {
        GameObject newObjective = Instantiate(textPrefab, objectivesCountainer.transform);
        newObjective.GetComponent<Objective>().SetObjective(text, currentResult);
        objectives.Add(newObjective);
        newObjective.GetComponent<Objective>().UpdateObjective();
        return newObjective.GetComponent<Objective>();
    }

    public void ModifyObjective(Objective objective, string text)
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if(objectives[i].GetComponent<Objective>() == objective)
            {
                objectives[i].GetComponent<Objective>().SetObjective(text);
                objectives[i].GetComponent<Objective>().UpdateObjective();
            }
        }
    }

    public void ModifyObjective(Objective objective, string text, int currentResult)
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].GetComponent<Objective>() == objective)
            {
                objectives[i].GetComponent<Objective>().SetObjective(text, currentResult);
                objectives[i].GetComponent<Objective>().UpdateObjective();
            }
        }
    }

    public void UpdateDisplay()
    {
        foreach (GameObject child in objectivesCountainer.transform)
        {
            child.GetComponent<Objective>().UpdateObjective();
        }
    }
}
