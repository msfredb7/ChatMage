using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;

public class DrawRoad : MonoBehaviour {

	public float timeBetweenDots = 0.1f;

	public Transform roadContainer;
	public Transform nextLevel;

	int currentDot;
	int amountOfDots;

	public void StartRoad() {
		if (roadContainer == null)
			return;

		currentDot = 0;
		amountOfDots = roadContainer.childCount;

		DelayManager.LocalCallTo(MakeRoad, timeBetweenDots, this);
	}

	void MakeRoad() {
		roadContainer.GetChild(currentDot).gameObject.SetActive(true);
		currentDot++;
		if(currentDot < amountOfDots)
			DelayManager.LocalCallTo(MakeRoad, timeBetweenDots, this);
		else
			DelayManager.LocalCallTo(delegate () { nextLevel.gameObject.SetActive(true); }, timeBetweenDots, this);
	}

	public void ShowAllRoad() {
		if (roadContainer == null)
			return;

		for(int i = 0; i < roadContainer.childCount; i++) {
			roadContainer.GetChild(i).gameObject.SetActive(true);
		}
	}
}
