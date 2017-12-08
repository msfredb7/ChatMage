using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCinematic : MonoBehaviour {

	List<SceneAnimation> scenes;
	int currentScene;

	void Start() {
		currentScene = 0;

		for(var i = 1; i< transform.childCount - 1; i++) {
			scenes.Add(transform.GetChild(i).GetComponent<SceneAnimation>());
			scenes[i-1].animator.speed = 0;
			scenes[i-1].gameObject.SetActive(false);
		}

		scenes[0].gameObject.SetActive(true);
		scenes[0].StartSceneAnim();
	}

	public void NextScene() {
		scenes[currentScene].gameObject.SetActive(false);
		currentScene++;
		if (currentScene < scenes.Count) {
			scenes[currentScene].gameObject.SetActive(true);
			scenes[currentScene].StartSceneAnim();
		}
		else
			Debug.Log("EndIntro");
	}

	public void ClickToSkip() {
		scenes[currentScene].Skip();
	}
}
