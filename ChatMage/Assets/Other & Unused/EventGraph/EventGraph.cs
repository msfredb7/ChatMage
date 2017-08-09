using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGraph : MonoBehaviour
{
    [Header("NE PAS TOUCHER AU LISTES")]
    [ReadOnly(forwardToChildren = false)]
    public List<BaseVirtualEvent> virtualEvents;
    [ReadOnly(forwardToChildren = false)]
    public List<BasePhysicalEvent> physicalEvents;

    public event SimpleEvent onEventsAddedOrRemoved;

    public bool IsVirtualNameAvailable(string name)
    {
        if (virtualEvents == null)
            return true;

        for (int i = 0; i < virtualEvents.Count; i++)
        {
            if (virtualEvents[i].name == name)
                return false;
        }
        return true;
    }

    public T CreateAndAddVirtualEvent<T>(string name, Vector2 position) where T : BaseVirtualEvent
    {
        if (!IsVirtualNameAvailable(name))
            throw new System.Exception("Event name taken");

        T newEvent = ScriptableObject.CreateInstance<T>();
        newEvent.name = name;
        newEvent.MoveToPos(position);

        virtualEvents.Add(newEvent);

        if (onEventsAddedOrRemoved != null)
            onEventsAddedOrRemoved();
        return newEvent;
    }

    public void RemoveVirtualEvent(BaseVirtualEvent theEvent)
    {
        if (virtualEvents.Remove(theEvent))
        {
            if (onEventsAddedOrRemoved != null)
                onEventsAddedOrRemoved();
        }
    }

    public T AddPhysicalEvent<T>(T existingEvent) where T : BasePhysicalEvent
    {
        physicalEvents.Add(existingEvent);
        existingEvent.graph = this;

        if (onEventsAddedOrRemoved != null)
            onEventsAddedOrRemoved();
        return existingEvent;
    }

    public void RemovePhysicalEvent(BasePhysicalEvent theEvent)
    {
        if (physicalEvents.Remove(theEvent))
        {
            if (onEventsAddedOrRemoved != null)
                onEventsAddedOrRemoved();
        }
    }

    public void RemoveNulls()
    {
        for (int i = 0; i < virtualEvents.Count; i++)
        {
            if(virtualEvents[i] == null)
            {
                virtualEvents.RemoveAt(0);
                i--;
            }
        }
        for (int i = 0; i < physicalEvents.Count; i++)
        {
            if (physicalEvents[i] == null)
            {
                physicalEvents.RemoveAt(0);
                i--;
            }
        }
    }

}
