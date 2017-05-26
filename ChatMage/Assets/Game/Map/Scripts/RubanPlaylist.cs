using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RubanPlaylist
{
    public string name;
    public List<Ruban> rubans = new List<Ruban>();

    private Ruban[] activeRubans = new Ruban[2];
    private float currentHeight = 0;
    private float totalLength = -1;
    private int activeRubansCount = 0;
    private int nextRubanIndex = 0;
    private Action<float> onEnd = null;
    private Action onComplete = null;
    private bool ending = true;
    private double currentRubanDeltaHeight = 0;
    [System.NonSerialized]
    public bool shouldLoop;
    public double deltaHeightAnchor;
    public Action<float> onBoutDuRouleau;
    public bool fromATransition = false;

    public void End(Action<float> onReadyForNextPlaylist, Action onComplete)
    {
        onEnd = onReadyForNextPlaylist;
        this.onComplete = onComplete;
        ending = true;
    }
    public void EndImmediately()
    {
        DeactivateAll();
    }

    public float GetTotalHeight()
    {
        if (totalLength >= 0)
            return totalLength;

        totalLength = 0;
        for (int i = 0; i < rubans.Count; i++)
        {
            totalLength += rubans[i].length;
        }
        return totalLength;
    }

    void DeactivateAll()
    {
        for (int i = 0; i < activeRubansCount; i++)
        {
            activeRubans[i].gameObject.SetActive(false);
            activeRubans[i] = null;
        }
        activeRubansCount = 0;
    }

    public void StartAt(float height)
    {
        //DeactivateAll();
        ending = false;
        onComplete = null;
        onEnd = null;
        nextRubanIndex = 0;
        activeRubansCount = 0;
        currentRubanDeltaHeight = height;

        ActivateNewRuban(height, true);
    }

    void ActivateNewRuban(float height, bool onTop)
    {
        while (nextRubanIndex >= rubans.Count)
            nextRubanIndex -= rubans.Count;

        if (onTop)
        {
            activeRubans[activeRubansCount] = rubans[nextRubanIndex];
            rubans[nextRubanIndex].gameObject.SetActive(true);
            rubans[nextRubanIndex].PutAt(height, true);
        }
        else
        {
            if (activeRubans[1] != null)
                DeactivateLastRuban();

            activeRubans[1] = activeRubans[0]; // 0 -> 1

            nextRubanIndex -= 2;
            while (nextRubanIndex < 0)
                nextRubanIndex += rubans.Count;

            activeRubans[0] = rubans[nextRubanIndex];
            activeRubans[0].gameObject.SetActive(true);
            activeRubans[0].PutAt(height - activeRubans[0].length);
            currentRubanDeltaHeight = activeRubans[0].GetBottomHeight() - currentHeight;

            nextRubanIndex++;
        }

        activeRubansCount++;
        nextRubanIndex++;
    }

    void DeactivateFirstRuban()
    {
        activeRubans[0].gameObject.SetActive(false);    //Deactivate
        activeRubans[0] = activeRubans[1];              //0 <- 1
        activeRubansCount--;                            //Decrease Count
        activeRubans[1] = null;                         //Put 'null' at end
        if (activeRubans[0] != null)
            currentRubanDeltaHeight = activeRubans[0].GetBottomHeight() - currentHeight;
        else
            currentRubanDeltaHeight = 0;
    }

    void DeactivateLastRuban()
    {
        activeRubans[1].gameObject.SetActive(false);
        activeRubans[1] = null;
        activeRubansCount--;
        nextRubanIndex--;
    }

    /// <summary>
    /// Returns the top height.
    /// </summary>
    public void UpdatePos(double heightAnchor)
    {
        currentHeight = (float)(heightAnchor + deltaHeightAnchor);
        //currentHeight += (float)heightAnchor;

        if (activeRubans[0] != null)
        {
            activeRubans[0].PutAt(currentHeight + (float)currentRubanDeltaHeight);
        }
        if (activeRubans[1] != null)
        {
            activeRubans[1].PutAt(activeRubans[0].GetTopHeight());
        }

        OnProgressChange();
    }

    void OnProgressChange()
    {
        //Le premier ruban active arrive à sa fin ?
        if (activeRubans[0].GetProgress() == 1)
        {
            //Gotta activate new ruban ?
            if (activeRubansCount == 1)
            {
                if (ending)
                {
                    if (onEnd != null)
                    {
                        onEnd(activeRubans[0].GetTopHeight());
                        onEnd = null;
                    }
                }
                else
                {
                    //Fin du rouleau
                    if (nextRubanIndex == rubans.Count)
                    {
                        if (onBoutDuRouleau != null)
                        {
                            onBoutDuRouleau(activeRubans[0].GetTopHeight());
                        }
                        //Transition vers une nouvelle playlist ou on loop ?
                        if (shouldLoop)
                            ActivateNewRuban(activeRubans[0].GetTopHeight(), true);
                        else
                        {
                            OnProgressChange();
                            return;
                        }
                    }
                    else
                        ActivateNewRuban(activeRubans[0].GetTopHeight(), true);
                }
            }

            //Remove old ruban ?
            if (activeRubans[0].HasExitedScreen())
            {
                if (ending && activeRubansCount == 1)
                {
                    if (onComplete != null)
                    {
                        onComplete();
                        onComplete = null;
                    }
                }
                DeactivateFirstRuban();
            }
        }
        else if (!fromATransition)
        {
            if (activeRubans[0].GetProgress() == -1)
            {
                ActivateNewRuban(activeRubans[0].GetBottomHeight(), false);
            }
            else
            {
                //Remove top ruban ?
                if (activeRubansCount > 1 && activeRubans[1].HasExitedScreen())
                {
                    DeactivateLastRuban();
                }
            }
        }
    }
}