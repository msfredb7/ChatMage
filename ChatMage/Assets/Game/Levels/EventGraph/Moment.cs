using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System;

namespace GameEvents
{
    [System.Serializable]
    public abstract class BaseMoment
    {
        [HideInInspector]
        public List<UnityEngine.Object> iEvents = new List<UnityEngine.Object>();

        public void AddIEvent(UnityEngine.Object obj)
        {
            if (iEvents.Contains(obj))
                return;
            iEvents.Add(obj);
        }

        public void RemoveIEvent(UnityEngine.Object obj)
        {
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

        public virtual bool HasListeners()
        {
            return iEvents.Count > 0;
        }

        public Type[] GetGenericTypes()
        {
            Type baseType = GetType().BaseType;
            if (baseType.IsGenericType)
                return baseType.GetGenericArguments();
            else
                return new Type[0];
        }
        public bool MatchWithGenericTypes(Type[] types)
        {
            Type[] myTypes = GetGenericTypes();

            if (myTypes.Length != types.Length)
                return false;

            for (int i = 0; i < myTypes.Length; i++)
            {
                if (!myTypes[i].Equals(types[i]))
                    return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Moment : BaseMoment
    {
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

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }

    [System.Serializable]
    public class Moment<T> : BaseMoment
    {
        public virtual void Launch(T value)
        {
            for (int i = 0; i < iEvents.Count; i++)
            {
                if (iEvents[i] != null)
                    (iEvents[i] as IEvent<T>).Trigger(value);
            }
        }
    }

    [System.Serializable]
    public class MomentEvent_bool : UnityEvent<bool> { }

    [System.Serializable]
    public class MomentBool : Moment<bool>
    {
        public MomentEvent_bool unityEvent = new MomentEvent_bool();
        public override void Launch(bool value)
        {
            base.Launch(value);
            unityEvent.Invoke(value);
        }

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }


    [System.Serializable]
    public class MomentEvent_float : UnityEvent<float> { }

    [System.Serializable]
    public class MomentFloat : Moment<float>
    {
        public MomentEvent_float unityEvent = new MomentEvent_float();
        public override void Launch(float value)
        {
            base.Launch(value);
            unityEvent.Invoke(value);
        }

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }

    [System.Serializable]
    public class MomentEvent_int : UnityEvent<int> { }

    [System.Serializable]
    public class MomentInt : Moment<int>
    {
        public MomentEvent_int unityEvent = new MomentEvent_int();
        public override void Launch(int value)
        {
            base.Launch(value);
            unityEvent.Invoke(value);
        }

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }

    [System.Serializable]
    public class MomentEvent_string : UnityEvent<string> { }

    [System.Serializable]
    public class MomentString : Moment<string>
    {
        public MomentEvent_string unityEvent = new MomentEvent_string();
        public override void Launch(string value)
        {
            base.Launch(value);
            unityEvent.Invoke(value);
        }

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }

    [System.Serializable]
    public class MomentEvent_unit : UnityEvent<Unit> { }

    [System.Serializable]
    public class MomentUnit : Moment<Unit>
    {
        public MomentEvent_unit unityEvent = new MomentEvent_unit();
        public override void Launch(Unit value)
        {
            base.Launch(value);
            unityEvent.Invoke(value);
        }

        public override bool HasListeners()
        {
            return unityEvent.GetPersistentEventCount() > 0 || base.HasListeners();
        }
    }
}