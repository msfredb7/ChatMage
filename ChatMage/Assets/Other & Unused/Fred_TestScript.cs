using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;

public class Fred_TestScript : BaseBehavior
{
    public A variable;
    public bool useCube;

    [InspectorShowIf("useCube")]
    public GameObject cube;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            cube.transform.DOMoveY(3, 2).SetEase(Ease.OutBounce).SetDelay(1).SetLoops(-1, LoopType.Yoyo);
        }
    }

    [InspectorButton]
    void SomeMethod()
    {

    }
}

[System.Serializable]
public abstract class A
{

}

[System.Serializable]
public class B : A
{
    public int health;
    public A a;
}

[System.Serializable]
public class C : A
{

}