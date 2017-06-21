using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuCarAnimation : MonoBehaviour {

    public GameObject rightWaypoint;
    public GameObject leftWaypoint;
    public float speed;

	void Start ()
    {
        DoAnimation(rightWaypoint);
	}
    
    void DoAnimation(GameObject waypoint)
    {
        Tweener animation = transform.DOLocalMoveX(waypoint.transform.localPosition.x, speed);
        animation.OnComplete(delegate () {
            transform.DOMoveX(leftWaypoint.transform.position.x, 0);
            DoAnimation(rightWaypoint);
        });
    }
}
