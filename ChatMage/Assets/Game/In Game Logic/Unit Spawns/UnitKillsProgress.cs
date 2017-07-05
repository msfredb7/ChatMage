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

    int count;
    int killCount;
    Unit[] units;
    bool cleanup;
    LinkedList<Callback> callbacks;

    public int KillCount
    {
        get { return killCount; }
    }
    public float Progress
    {
        get { return (float)killCount / (float)count; }
    }

    public UnitKillsProgress(int amountOfUnits, bool removeListenersOnDestruction = false)
    {
        count = amountOfUnits;
        units = new Unit[amountOfUnits];
        cleanup = removeListenersOnDestruction;
    }

    ~UnitKillsProgress()
    {
        if (cleanup)
            for (int i = 0; i < units.Length; i++)
            {
                if (units[i] != null)
                    units[i].onDeath -= Unit_onDeath;
            }
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
        unit.onDeath += Unit_onDeath;
    }

    private void Unit_onDeath(Unit unit)
    {
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
