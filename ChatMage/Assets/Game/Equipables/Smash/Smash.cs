using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System;

public abstract class Smash : Equipable {
    public abstract void OnSmash(Action onComplete);
}
