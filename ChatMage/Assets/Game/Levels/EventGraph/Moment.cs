using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    [System.Serializable]
    public class Moment
    {
        public List<Object> iEvents = new List<Object>();
        public UnityEvent unityEvent = new UnityEvent();

        public void Launch()
        {
            for (int i = 0; i < iEvents.Count; i++)
            {
                if (iEvents[i] != null)
                    (iEvents[i] as IEvent).Trigger();
            }
            unityEvent.Invoke();
        }

        public void AddIEvent(IEvent theEvent)
        {
            if (!(theEvent is Object))
                return;

            Object obj = theEvent as Object;
            if (iEvents.Contains(obj))
                return;
            iEvents.Add(obj);
        }

        public void RemoveIEvent(IEvent theEvent)
        {
            if (!(theEvent is Object))
                return;

            Object obj = theEvent as Object;
            iEvents.Remove(obj);
        }

        public void RemoveNulls()
        {
            for (int i = 0; i < iEvents.Count; i++)
            {
                if (iEvents[i] == null)
                {
                    iEvents.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ClearMoments()
        {
            iEvents.Clear();
        }
    }
}