using CCC.Manager;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroCreditsAnimation : MonoBehaviour {

    public Image backgroundSprite;
    public Image logoSprite;
    public Image logo2Sprite;
    public float fadeDuration = 1f;
    public float animationDuration = 3f;
    public float startDelay = 2f;
    public AudioClip introSong;
    public AudioClip themeSong;

	void Start ()
    {
        MasterManager.Sync(delegate() {
            SoundManager.PlayMusic(introSong, true, 0.1f, true);
            // Après un délai
            DelayManager.LocalCallTo(delegate ()
            {
                // On fait fade in le background
                backgroundSprite.DOFade(1, fadeDuration).OnComplete(delegate ()
                {
                    // On fait fade in le logo 1
                    logo2Sprite.DOFade(1, fadeDuration).OnComplete(delegate() {
                        // Apres un delai 
                        DelayManager.LocalCallTo(delegate ()
                        {
                            // on fade out le logo 1
                            logo2Sprite.DOFade(0, fadeDuration).OnComplete(delegate ()
                            {
                                // Apres un cours delai 
                                DelayManager.LocalCallTo(delegate ()
                                {
                                    // on fade in le logo 2
                                    logoSprite.DOFade(1, fadeDuration).OnComplete(delegate ()
                                    {
                                        // Apres un cours delai
                                        DelayManager.LocalCallTo(delegate ()
                                        {
                                            // On fade out le logo 2
                                            logoSprite.DOFade(0, fadeDuration).OnComplete(delegate ()
                                            {
                                                // Apres on fade out le background
                                                backgroundSprite.DOFade(0, fadeDuration).OnComplete(delegate ()
                                                {
                                                    // On load le jeu
                                                    SoundManager.PlayMusic(themeSong, false, 1f, false);
                                                    Scenes.Load("MainMenu");
                                                });
                                            });
                                        }, animationDuration, this);
                                    });
                                }, 1, this);
                            });
                        }, animationDuration, this);
                    });
                });
            }, startDelay, this);
        });
    }

    void Update()
    {
        // Vraiment étonnant que ceci donne un behavior parfait. (on est sur de voir le logo lonely boy mais on peut skip rapide)
        if (Input.anyKey)
        {
            if(logo2Sprite.color.a == 1)
                logo2Sprite.DOFade(0, fadeDuration).OnComplete(delegate() {
                    if (backgroundSprite.color.a == 1)
                        backgroundSprite.DOFade(0, fadeDuration).OnComplete(delegate () {
                            // On load le jeu
                            SoundManager.PlayMusic(themeSong, false, 1f, false);
                            Scenes.Load("MainMenu");
                        });
                });
            if(logoSprite.color.a == 1)
                logoSprite.DOFade(0, fadeDuration).OnComplete(delegate() {
                    if (backgroundSprite.color.a == 1)
                        backgroundSprite.DOFade(0, fadeDuration).OnComplete(delegate () {
                            // On load le jeu
                            SoundManager.PlayMusic(themeSong, false, 1f, false);
                            Scenes.Load("MainMenu");
                        });
                });
        }
    }
}
