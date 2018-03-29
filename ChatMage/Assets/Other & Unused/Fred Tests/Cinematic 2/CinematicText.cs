using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;

public class CinematicText : MonoBehaviour
{
    public bool additive = false;
    public bool clearTextOnDisable = true;
    public Text textComponent;
    public Text buffer;
    public AudioSource audioComponent;

    [TextArea]
    public string text;
    public float writingSpeed = 1;

    private static int activeCount = 0;

    private Tween myAnim;

    void OnEnable()
    {
        activeCount++;

        textComponent.DOKill();
        audioComponent.enabled = true;
        var startText = additive ? textComponent.text : "";
        //textComponent.text = startText;
        buffer.text = "";

        //DOGetter<Text> getter = null;
        //if (additive)
        //    getter = () => textComponent.text.Replace(startText, "");
        //else
        //    getter = () => textComponent.text;

        //DOSetter<Text> setter = null;
        //if (additive)
        //    setter = (s) => textComponent.text = startText + s;
        //else
        //    setter = (s) => textComponent.text = s;

        TweenCallback apply = () => textComponent.text = startText + buffer.text;
        //if (additive)
        //    apply = () => textComponent.text = startText + buffer.text;
        //else
        //    apply = () => textComponent.text = buffer.text;

        //myAnim = DOTween.To(getter, setter, text, text.Length * 0.022f / writingSpeed)
        //    .OnComplete(() => audioComponent.enabled = false);

        myAnim = buffer.DOText(text, text.Length * 0.022f / writingSpeed)
            .OnUpdate(apply)
            .OnComplete(() => audioComponent.enabled = false);
    }

    void OnDisable()
    {
        activeCount--;

        if (activeCount == 0)
        {
            audioComponent.enabled = false;
            if (clearTextOnDisable)
                textComponent.text = "";
        }
        myAnim.Kill();
    }
}
