using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace CCC.Manager
{
    public class DelayManager : BaseManager<DelayManager>
    {
        //static public Coroutine CallTo(Action action, float delay, bool realTime = false)
        //{
        //    if (instance == null)
        //    {
        //        Debug.LogError("Tried to call a delay, but the manager is null. Was it properly loaded by MasterManager ?");
        //        return null;
        //    }
        //    return instance.StartCoroutine(instance.DelayedCallTo(action, delay, realTime));
        //}
        static public Coroutine LocalCallTo(Action action, float delay, MonoBehaviour source, bool realTime = false)
        {
            if (instance == null)
            {
                Debug.LogError("Tried to call a delay, but the manager is null. Was it properly loaded by MasterManager ?");
                return null;
            }
            return source.StartCoroutine(instance.DelayedCallTo(action, delay, realTime));
        }

        //static public void CancelAll()
        //{
        //    instance.StopAllCoroutines();
        //}

        //static public void Cancel(Coroutine coroutine)
        //{
        //    instance.StopCoroutine(coroutine);
        //}

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public override void Init()
        {
            CompleteInit();
        }

        IEnumerator DelayedCallTo(Action action, float delay, bool realTime)
        {
            if (realTime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);
            
            action.Invoke();
        }
    }
}
