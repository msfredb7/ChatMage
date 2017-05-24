using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

    public bool complete;
    public Coroutine task;

    public Task(Coroutine task)
    {
        this.task = task;
        complete = false;
    }
}
