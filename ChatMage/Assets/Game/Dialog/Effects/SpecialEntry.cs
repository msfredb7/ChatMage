using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.UI;
using DG.Tweening;

namespace Dialoguing.Effect
{
    [System.Serializable]
    public class SpecialEntry : Effect
    {
        public enum EntryType
        {
            fromBottom = 0,
            fadeIn = 2
        }

        public EntryType entryType;
        public bool leftSide = true;
        public float animDuration = 1;

        private Characters charac;

        // Important de mettre cet effet AVANT le changement de sprite
        public override void Apply(DialogDisplay display, Reply reply)
        {
            charac = display.characters;

            charac.specialEntryMode = true;

            switch (entryType)
            {
                case EntryType.fromBottom:
                    EntryFromBottom();
                    break;
                case EntryType.fadeIn:
                    EntryFadeIn();
                    break;
                default:
                    break;
            }
        }

        void EntryFromBottom()
        {
            Image currentImage = null;
            if (leftSide)
                currentImage = charac.leftImage;
            else
                currentImage = charac.rightImage;

            if (currentImage == null)
                return;

            Vector2 initialPos = currentImage.rectTransform.anchoredPosition;
            currentImage.rectTransform.anchoredPosition = new Vector2(initialPos.x, -800);
            currentImage.enabled = true;
            Tweener anim = currentImage.rectTransform.DOLocalMoveY(initialPos.y, animDuration).SetUpdate(true);
            
            anim.OnComplete(delegate ()
            {
                OnCompleteEntry();
            });
        }

        void EntryFadeIn()
        {
            Image currentImage = null;
            if (leftSide)
                currentImage = charac.leftImage;
            else
                currentImage = charac.rightImage;

            if (currentImage == null)
                return;

            currentImage.SetAlpha(0);
            currentImage.enabled = true;
            Tweener anim = currentImage.DOFade(1, animDuration).SetUpdate(true);

            anim.OnComplete(delegate ()
            {
                OnCompleteEntry();
            });
        }

        void OnCompleteEntry()
        {
            if (leftSide)
                charac.leftName.SetActive(true);
            else
                charac.rightName.SetActive(true);

            if (charac.supposeToShakeCharacter)
            {
                if (charac.leftImage.enabled)
                    charac.SetLeftCharacterShake(charac.currentCharacterShakeIntensity, charac.currentCharacterShakeDuration);
                if (charac.rightImage.enabled)
                    charac.SetRightCharacterShaker(charac.currentCharacterShakeIntensity, charac.currentCharacterShakeDuration);
            }
        }
    }
}
