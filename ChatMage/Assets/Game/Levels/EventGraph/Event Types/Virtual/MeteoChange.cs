using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    [MenuItem("Meteo/Turn On-Off"), DefaultNodeName("MeteoChange")]
    public class MeteoChange : VirtualEvent, IEvent
    {
        public bool meteoState = true;
        public float duration = 1.5f;

        public void Trigger()
        {
            MeteoPlayer meteo = Game.instance.map.meteo;
            if(meteo != null)
            {
                if (meteoState)
                {
                    meteo.Show(duration);
                }
                else
                {
                    meteo.Hide(duration);
                }
            }
        }

        public override string NodeLabel()
        {
            return "Meteo " + (meteoState ? "ON" : "OFF");
        }

        public override Color GUIColor()
        {
            return Colors.METEO;
        }
    }

}