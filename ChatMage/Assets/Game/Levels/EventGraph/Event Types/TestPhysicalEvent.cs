using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysicalEvent : BasePhysicalEvent
{
    public Moment onUnitKilled = new Moment();

    public List<SomeClass> someList = new List<SomeClass>();

    [System.Serializable]
    public class SomeClass
    {
        public Moment someMoment = new Moment();
        public string id;
    }

    public override void GetAdditionalMoments(out Moment[] moments, out string[] names)
    {
        names = new string[someList.Count];
        moments = new Moment[someList.Count];

        for (int i = 0; i < someList.Count; i++)
        {
            names[i] = "Some Class " + i;
            moments[i] = someList[i].someMoment;
        }
    }

    public override Color DefaultColor()
    {
        return new Color(0.7f, 1, 1, 1);
    }

    public override string DefaultLabel()
    {
        return "Test Physical";
    }
}
