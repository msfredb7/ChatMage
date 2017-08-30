using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Units/OnDeath"), DefaultNodeName("Units")]
    public class OnDeath : FIVirtualEvent
    {
        public List<Unit> unitsToListen;
        public _WaveWhat.Callback[] callbacks;

        public override void OnGameReady()
        {
            UnitKillsProgress callbacker = new UnitKillsProgress(unitsToListen.Count);

            //Add callbacks
            foreach (_WaveWhat.Callback cb in callbacks)
            {
                if (cb.useProgress)
                    callbacker.AddCallback(cb.moment.Launch, cb.atProgress);
                else
                    callbacker.AddCallback(cb.moment.Launch, cb.atKillCount);
            }

            //Register units
            foreach (Unit unit in unitsToListen)
            {
                callbacker.RegisterUnit(unit);
            }
        }

        public override Color GUIColor()
        {
            Color red = Colors.WAVES;
            red.r *= 0.9f;
            return red;
        }

        public override string NodeLabel()
        {
            return "OnDeath: " + name;
        }


        public override void GetAdditionalMoments(out BaseMoment[] moments, out string[] names)
        {
            int count = 0;
            if (callbacks != null)
                count = callbacks.Length;

            moments = new Moment[count];
            names = new string[count];

            for (int i = 0; i < count; i++)
            {
                _WaveWhat.Callback callback = callbacks[i];
                moments[i] = callback.moment;
                if (callback.useProgress)
                    names[i] = "at " + Mathf.RoundToInt(callback.atProgress * 100) + "%";
                else
                    names[i] = "at " + callback.atKillCount + " kills";
            }
        }
    }

}