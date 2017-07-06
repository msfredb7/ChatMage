using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    [RequireComponent(typeof(PointerListener), typeof(Image))]
    public class ProxyButton : MonoBehaviour
    {
        Image image;
        Action currentAction;

        void Awake()
        {
            image = GetComponent<Image>();
            GetComponent<PointerListener>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (currentAction != null)
                currentAction();

            image.enabled = false;
        }

        public void SetBig()
        {
            image.rectTransform.sizeDelta = Vector2.one * 450;
        }

        public void SetSmall()
        {
            image.rectTransform.sizeDelta = Vector2.one * 250;
        }

        public void SetSizeDelta(Vector2 sizeDelta)
        {
            image.rectTransform.sizeDelta = sizeDelta;
        }

        public void Proxy(Vector2 position, Action onClick)
        {
            currentAction = onClick;
            transform.position = position;
            image.enabled = true;
        }

        public void Proxy(Button button)
        {
            Proxy(button.transform.position, delegate ()
            {
                button.onClick.Invoke();
            });
        }

        public void Proxy(Button button, Action onClick)
        {
            Proxy(button.transform.position, delegate ()
            {
                button.onClick.Invoke();
                if (onClick != null)
                    onClick();
            });
        }
    }
}
