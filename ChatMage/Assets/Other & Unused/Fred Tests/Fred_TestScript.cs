using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;

public class Fred_TestScript : BaseBehavior
{
	public LaserSword sword;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sword.OpenSword(null);
        }
		if (Input.GetKeyDown(KeyCode.Y))
        {
            sword.CloseSword(null);
        }
    }
    public void TestOne()
    {

    }
    public void TestTwo(float a)
    {

    }
    public void TestThree(AudioPlayable e)
    {

    }
    public void TestFour(GameObject e)
    {

    }
    public void TestFive(AudioClip e)
    {

    }
}