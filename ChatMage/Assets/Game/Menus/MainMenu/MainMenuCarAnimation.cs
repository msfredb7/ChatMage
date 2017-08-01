﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuCarAnimation : MonoBehaviour {

    public GameObject rightWaypoint;
    public float speed;

	public void Init ()
    {
        transform.DOLocalMoveX(rightWaypoint.transform.localPosition.x, speed).SetLoops(-1, LoopType.Restart);
    }
}
