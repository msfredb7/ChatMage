using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameEvents : MonoBehaviour
{
    float timer = 0;
    public float GameTime { get { return timer; } }

    private class DelayedAction
    {
        public float at;
        public Action action;
    }

    LinkedList<DelayedAction> delayedActions = new LinkedList<DelayedAction>();

    void Update()
    {
        timer += Time.deltaTime * Game.instance.worldTimeScale;
        CheckActionList();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void CheckActionList()
    {
        while (delayedActions.First != null)
        {
            LinkedListNode<DelayedAction> node = delayedActions.First;

            if (node.Value.at <= timer)
            {
                node.Value.action();
                delayedActions.Remove(node.Value);
            }
            else
                break;
        }
    }

    public void AddDelayedAction(Action action, float delay)
    {
        //if (delay <= 0)
        //{
        //    action();
        //    return;
        //}

        DelayedAction da = new DelayedAction() { at = delay + timer, action = action };
        LinkedListNode<DelayedAction> node = delayedActions.First;

        if (node == null)
        {
            delayedActions.AddFirst(da);
        }
        else
            while (true)
            {
                if (node.Value.at > da.at)
                {
                    delayedActions.AddBefore(node, da);
                    break;
                }
                if (node.Next == null)
                {
                    delayedActions.AddAfter(node, da);
                    break;
                }
                node = node.Next;
            }
    }

    public T SpawnUnderUI<T>(T prefab) where T : MonoBehaviour
    {
        return Instantiate(prefab, Game.instance.ui.transform).GetComponent<T>();
    }
    public T SpawnUnderGame<T>(T prefab) where T : MonoBehaviour
    {
        return Instantiate(prefab, Game.instance.transform).GetComponent<T>();
    }
}
