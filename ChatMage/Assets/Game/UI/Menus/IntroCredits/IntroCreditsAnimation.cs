
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroCreditsAnimation : MonoBehaviour
{

    public Image backgroundSprite;
    public Image logoSprite;
    public Image logo2Sprite;
    public float fadeDuration = 1f;
    public float startDelay = 2f;
    public AudioClip logoSound;

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(delegate ()
        {
            DefaultAudioSources.PlayMusic(logoSound, false, 0.1f); // Son de l'apparition du logo
            // Après un délai initial
            this.DelayedCall(delegate ()
            {
                Sequence animation = DOTween.Sequence();
                animation.Append(backgroundSprite.DOFade(1, fadeDuration));
                animation.Append(logo2Sprite.DOFade(1, fadeDuration)); // On fait fade in le logo
                animation.Append(logo2Sprite.DOFade(0, fadeDuration)); // On fait fade out le logo
                animation.Append(backgroundSprite.DOFade(0, fadeDuration)); // On fait fade out le background
                animation.OnComplete(delegate () { Scenes.Load("MainMenu"); });
            }, startDelay);
        });
    }

    void Update()
    {
        if (Input.anyKey)
            Scenes.Load("MainMenu");
    }
}
