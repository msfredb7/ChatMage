using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialInfo : MonoBehaviour {

    public float coolDown;

    public abstract void DisplayInfo(string text);

    public abstract void OnEnd(); 
}
