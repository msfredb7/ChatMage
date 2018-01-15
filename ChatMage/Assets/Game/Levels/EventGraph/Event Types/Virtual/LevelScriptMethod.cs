using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Level/Script Method"), DefaultNodeName("LS Method")]
    public class LevelScriptMethod : VirtualEvent, IEvent, IEvent<string>, IEvent<int>, IEvent<bool>, IEvent<float>, IEvent<Unit>
    {
        public string methodName;


        public override string NodeLabel()
        {
            return "Call: " + methodName;
        }

        public override Color GUIColor()
        {
            return Colors.LEVEL_SCRIPT;
        }

        public void Trigger()
        {
            Call(new Type[0], new object[0]);
        }

        public void Trigger(float f)
        {
            Call(new Type[] { typeof(float) }, new object[] { f });
        }

        public void Trigger(int i)
        {
            Call(new Type[] { typeof(int) }, new object[] { i });
        }

        public void Trigger(bool b)
        {
            Call(new Type[] { typeof(bool) }, new object[] { b });
        }

        public void Trigger(string s)
        {
            Call(new Type[] { typeof(string) }, new object[] { s });
        }

        public void Trigger(Unit u)
        {
            Call(new Type[] { typeof(Unit) }, new object[] { u });
        }

        void Call(Type[] rq_parameters, object[] param)
        {
            if (string.IsNullOrEmpty(methodName))
                return;

            Type type = Game.Instance.levelScript.GetType();

            MethodInfo method = type.GetMethod(methodName, rq_parameters);

            if (method == null)
                return;

            method.Invoke(Game.Instance.levelScript, param);
        }
    }
}
