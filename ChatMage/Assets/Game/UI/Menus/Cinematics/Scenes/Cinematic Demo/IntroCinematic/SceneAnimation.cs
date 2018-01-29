using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SceneAnimation : MonoBehaviour {

	public Image sceneImage;
	public Text sceneText;
	[TextArea]
	public string text;
	public float timeBetweenLetters;
	public Animator animator;
	public IntroCinematic introControl;

	int currentLetter;
	bool skipAnim;

	Tweener fadeAnim;

	public void StartSceneAnim() {
		animator.speed = 0;
		skipAnim = false;
		fadeAnim = sceneImage.DOFade(1, 1).OnComplete(delegate(){ if (!skipAnim) PlayImageAnim(); });
	}

	void PlayImageAnim() {
		if (!skipAnim) {
			currentLetter = 0;
			animator.speed = 1 / Mathf.Max(1, 1.8f * text.Length * timeBetweenLetters);
			AddLetter();
		}
		
	}

	void AddLetter() {
		if (!skipAnim) {
			sceneText.text += text[currentLetter];
			currentLetter++;
			if (currentLetter < text.Length)
				this.DelayedCall(AddLetter, timeBetweenLetters);
		}
		
	}

	public void FinishAnim() {
		animator.speed = 1;
		if (!skipAnim) {
			sceneText.DOFade(0, 1);
			sceneImage.DOFade(0, 1).OnComplete(introControl.NextScene);
		}
		
	}

	public void Skip() {
		if (skipAnim) {
			introControl.NextScene();
		}
		else {
			skipAnim = true;
			fadeAnim.Complete();
			animator.speed = 100;
			sceneText.text = text;
		}
		
	}
}
