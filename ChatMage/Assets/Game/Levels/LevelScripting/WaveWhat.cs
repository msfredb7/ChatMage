using FullInspector;
using System;
using System.Reflection;

namespace LevelScripting
{
    [System.Serializable]
    public class WaveWhat
    {
        public UnitPack[] spawnSequence;
        public string onSpawnMethod = "";
        public Callback[] progressCallbacks;

        [System.Serializable]
        public class Callback
        {
            public string levelEventName;
            [InspectorShowIf("useProgress"), InspectorRange(0, 1)]
            public float atProgress = 1;
            [InspectorHideIf("useProgress")]
            public int atKillCount;
            public bool useProgress = false;
        }

        public Unit[] GetSpawnSequence()
        {
            Unit[] sequence = new Unit[TotalUnits];
            int cursor = 0;

            for (int i = 0; i < spawnSequence.Length; i++)
            {
                for (int j = 0; j < spawnSequence[i].quantity; j++)
                {
                    sequence[cursor] = spawnSequence[i].unit;
                    cursor++;
                }
            }

            return sequence;
        }

        public UnitKillsProgress GetKillsProgress(LevelScript levelScript, bool removeListenersOnDestruction = false)
        {
            if (progressCallbacks == null || progressCallbacks.Length == 0)
                return null;

            UnitKillsProgress callbacker = new UnitKillsProgress(TotalUnits, removeListenersOnDestruction);

            for (int i = 0; i < progressCallbacks.Length; i++)
            {
                string eventMessage = progressCallbacks[i].levelEventName;
                Action action = delegate ()
                {
                    levelScript.ReceiveEvent(eventMessage);
                };

                if (progressCallbacks[i].useProgress)
                    callbacker.AddCallback(action, progressCallbacks[i].atProgress);
                else
                    callbacker.AddCallback(action, progressCallbacks[i].atKillCount);
            }

            return callbacker;
        }

        public Action<Unit> GetSpawnAction(LevelScript levelScript)
        {
            if (string.IsNullOrEmpty(onSpawnMethod))
                return null;

            Type type = levelScript.GetType();

            Type[] rq_parameters = new Type[] { typeof(Unit) };

            MethodInfo method = type.GetMethod(onSpawnMethod, rq_parameters);

            if (method == null)
                return null;

            return delegate (Unit unit)
                {
                    object[] param = new object[] { unit };
                    method.Invoke(levelScript, param);
                };
        }

        public int TotalUnits
        {
            get
            {
                int count = 0;
                for (int i = 0; i < spawnSequence.Length; i++)
                {
                    count += spawnSequence[i].quantity;
                }
                return count;
            }
        }
    }
}