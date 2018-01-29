using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TUT_SwitchTutorial : BaseTutorial
    {
        private MedievalSwitch switche;

        protected override void Cleanup()
        {
        }

        protected override void OnStart()
        {
            switche = Game.Instance.map.mapping.GetTaggedObject("switch").GetComponent<MedievalSwitch>();
        }

        public void FocusOnSwitch(Action OnComplete)
        {
            modules.shorcuts.TimeFreeze();

            modules.spotlight.OnWorld(switche.gameObject.transform.position, delegate ()
            {
                modules.textDisplay.SetBottom();
                modules.textDisplay.DisplayText("This is a Switch. " +
                    "You can hit it in order to activate things. " +
                    "Use them wisely to make your way throught the levels. ", true);
                modules.proxyButton.ProxyScreen(DefocusOnSwitch);
            });
        }

        public void DefocusOnSwitch()
        {
            modules.textDisplay.HideText(delegate ()
            {
                modules.spotlight.Off(delegate () { modules.shorcuts.TimeUnFreeze(); });
            });
        }
    }
}
