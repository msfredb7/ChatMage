using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialoguing
{
    public class Characters : MonoBehaviour
    {
        public Image leftImage;
        public Image rightImage;

        // Special Entry
        [HideInInspector]
        public bool specialEntryMode = false;
        [HideInInspector]
        public bool supposeToShakeCharacter = false;

        // Name
        public GameObject leftName;
        public GameObject rightName;

        // Shaking
        [HideInInspector]
        public VectorShaker leftVectorShaker;
        [HideInInspector]
        public VectorShaker rightVectorShaker;
        public float currentCharacterShakeIntensity = 50;
        public float currentCharacterShakeDuration = 0.25f;

        private Vector2 stdLeftOffset;
        private Vector2 stdRightOffset;
        private bool leftOffsetSet = false;
        private bool rightOffsetSet = false;

        void Awake()
        {
            stdLeftOffset = leftImage.rectTransform.anchoredPosition;
            stdRightOffset = rightImage.rectTransform.anchoredPosition;

            // Init Shake
            specialEntryMode = false;
            supposeToShakeCharacter = false;

            // Init Shaking
            leftVectorShaker = leftImage.GetComponent<VectorShaker>();
            rightVectorShaker = rightImage.GetComponent<VectorShaker>();

            HideBoth();
        }

        public void SetLeftImage(Sprite leftSprite)
        {
            if (leftOffsetSet)
                ResetLeftOffset();

            if (!specialEntryMode)
                leftImage.enabled = true;
            leftImage.sprite = leftSprite;

            rightImage.enabled = false;
        }
        public void SetLeftImage(Sprite leftSprite, Vector2 offset)
        {
            SetLeftImage(leftSprite);
            SetLeftOffset(offset);
        }

        public void SetRightImage(Sprite rightSprite)
        {
            if (rightOffsetSet)
                ResetRightOffset();

            if (!specialEntryMode)
                rightImage.enabled = true;
            rightImage.sprite = rightSprite;

            leftImage.enabled = false;
        }
        public void SetRightImage(Sprite rightSprite, Vector2 offset)
        {
            SetRightImage(rightSprite);
            SetRightOffset(offset);
        }

        public void SetBothImage(Sprite leftSprite, Sprite rightSprite)
        {
            if (rightOffsetSet)
                ResetRightOffset();
            if (leftOffsetSet)
                ResetLeftOffset();

            leftImage.enabled = true;
            leftImage.sprite = leftSprite;
            rightImage.enabled = true;
            rightImage.sprite = rightSprite;
        }
        public void SetBothImage(Sprite leftSprite, Vector2 leftOffset, Sprite rightSprite, Vector2 rightOffset)
        {
            SetBothImage(leftSprite, rightSprite);
            SetRightOffset(rightOffset);
            SetLeftOffset(leftOffset);
        }

        public void HideBoth()
        {
            leftImage.enabled = false;
            rightImage.enabled = false;
        }

        public void SetLeftOffset(Vector2 offset)
        {
            leftOffsetSet = true;
            leftImage.rectTransform.anchoredPosition = stdLeftOffset + offset;
        }

        public void SetRightOffset(Vector2 offset)
        {
            rightOffsetSet = true;
            rightImage.rectTransform.anchoredPosition = stdRightOffset + offset;
        }

        public void ResetLeftOffset()
        {
            leftOffsetSet = false;
            leftImage.rectTransform.anchoredPosition = stdLeftOffset;
        }

        public void ResetRightOffset()
        {
            rightOffsetSet = false;
            rightImage.rectTransform.anchoredPosition = stdRightOffset;
        }

        public void SetLeftCharacterShake(float strength, float duration)
        {
            leftVectorShaker.Shake(strength, duration);
        }

        public void SetRightCharacterShaker(float strength, float duration)
        {
            rightVectorShaker.Shake(strength, duration);
        }

        public void SetLeftText(string text)
        {
            DisableRightName();
            ActivateLeftName(text);
        }

        public void SetRightText(string text)
        {
            DisableLeftName();
            ActivateRightName(text);
        }

        public void DisableRightName()
        {
            Image myImage = rightImage.GetComponentInChildren<NameSetText>().GetComponent<Image>();
            myImage.color = myImage.color.ChangedAlpha(0);
            rightImage.GetComponentInChildren<NameSetText>().SetText("");
        }

        public void ActivateRightName(string text)
        {
            Image myImage = rightImage.GetComponentInChildren<NameSetText>().GetComponent<Image>();
            myImage.color = myImage.color.ChangedAlpha(1);
            rightImage.GetComponentInChildren<NameSetText>().SetText(text);
            if (specialEntryMode)
                myImage.gameObject.SetActive(false);
        }

        public void DisableLeftName()
        {
            Image myImage = leftImage.GetComponentInChildren<NameSetText>().GetComponent<Image>();
            myImage.color = myImage.color.ChangedAlpha(0);
            leftImage.GetComponentInChildren<NameSetText>().SetText("");
        }

        public void ActivateLeftName(string text)
        {
            Image myImage = leftImage.GetComponentInChildren<NameSetText>().GetComponent<Image>();
            myImage.color = myImage.color.ChangedAlpha(1);
            leftImage.GetComponentInChildren<NameSetText>().SetText(text);
            if (specialEntryMode)
                myImage.gameObject.SetActive(false);
        }
    }
}