using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class Shortcuts
    {
        private TutorialScene modules;
        public Shortcuts(TutorialScene modules)
        {
            this.modules = modules;
        }

        /// <summary>
        /// Seulement un effet INGAME
        /// </summary>
        public void TimeFreeze()
        {
            if(Game.instance != null)
            {
                Game.instance.gameRunning.LockUnique("tutsc"); // Short pour 'Tuto shorcuts'
            }
        }

        /// <summary>
        /// Seulement un effet INGAME
        /// </summary>
        public void TimeUnFreeze()
        {
            if (Game.instance != null)
            {
                Game.instance.gameRunning.UnlockAll("tutsc"); // Short pour 'Tuto shorcuts'
            }
        }

        /// <summary>
        /// 1.Disable les inputs
        /// <para>2.Deplace le spotlight</para>
        /// <para>3.Proxy le bouton</para>
        /// <para>4.Invoke le bouton automatiquement</para>
        /// </summary>
        public void SpotlightOnButton(Button button, Action additionalAction = null)
        {
            modules.inputDisabler.DisableInput();
            modules.spotlight.On(button.transform.position);

            modules.proxyButton.Proxy(button, delegate ()
            {
                modules.inputDisabler.EnableInput();
                modules.spotlight.Off();
                if (additionalAction != null)
                    additionalAction();
            });
        }
    }
}
