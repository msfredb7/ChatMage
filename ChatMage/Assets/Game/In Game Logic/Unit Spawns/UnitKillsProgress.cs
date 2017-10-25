using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitKillsProgress
{
    public class Callback
    {
        public Action action;
        public float atProgress;
        public int atKillCount;
        private bool useProgress = false;
        public Callback(Action action, float atProgress)
        {
            useProgress = true;
            this.atProgress = atProgress;
            this.action = action;
        }
        public Callback(Action action, int atKillCount)
        {
            useProgress = false;
            this.atKillCount = atKillCount;
            this.action = action;
        }

        public bool Evaluate(float progress, int killCount)
        {
            if (useProgress)
            {
                if (progress >= atProgress)
                {
                    action();
                    return true;
                }
                return false;
            }
            else
            {
                if (killCount >= atKillCount)
                {
                    action();
                    return true;
                }
                return false;
            }
        }
    }

    bool isInfinite;
    int count;
    int killCount;
    int unitsAlive = 0;
    LinkedList<Callback> callbacks;

    public int KillCount
    {
        get { return killCount; }
    }

    public float Progress
    {
        get
        {
            if (isInfinite)
                return 0;
            if (count <= 0)
                return 1;
            return (float)killCount / (float)count;
        }
    }

    /// <summary>
    /// Infinite units
    /// </summary>
    public UnitKillsProgress()
    {
        isInfinite = true;
    }

    /// <summary>
    /// Finite amount of units
    /// </summary>
    public UnitKillsProgress(int amountOfUnits)
    {
        count = amountOfUnits;
    }

    public void AddCallback(Callback callback)
    {
        if (callbacks == null)
            callbacks = new LinkedList<Callback>();

        callbacks.AddLast(callback);

        CheckCallback(callbacks.Last);
    }
    public void AddCallback(Action action, float atProgress)
    {
        AddCallback(new Callback(action, atProgress));
    }
    public void AddCallback(Action action, int atKillcount)
    {
        AddCallback(new Callback(action, atKillcount));
    }

    public void RegisterUnit(Unit unit)
    {
        if (unit == null)
        {
            //Une unit a fail de spawn
            count--;
            CheckAllCallbacks();
        }
        else
        {
            unit.OnDeath += Unit_onDeath;
            unitsAlive++;
        }
    }

    public void NoMoreUnitsWillSpawn()
    {
        count = unitsAlive + killCount;
        isInfinite = false;
        CheckAllCallbacks();
    }

    private void Unit_onDeath(Unit unit)
    {
        unitsAlive--;
        killCount++;
        CheckAllCallbacks();
    }

    void CheckAllCallbacks()
    {
        if (callbacks == null)
            return;

        LinkedListNode<Callback> node = callbacks.First;

        while (node != null)
        {
            LinkedListNode<Callback> next = node.Next;
            CheckCallback(node);
            node = next;
        }
    }
    void CheckCallback(LinkedListNode<Callback> node)
    {
        if (node.Value.Evaluate(Progress, killCount))
            callbacks.Remove(node);
    }
}
