using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTutorial : BaseTutorial {

    public string positionLeftInputWaypointPrefabName;

    private GameObject leftInputWaypoint;

    private GameObject currentleftInputWaypoint;

    protected override void Start()
    {
        base.Start();
        LoadQueue queue = new LoadQueue(null);
        queue.AddUI(positionLeftInputWaypointPrefabName, (x) => leftInputWaypoint = x);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void End()
    {
        base.End();
    }

    public void ShowLeftInput()
    {
        currentleftInputWaypoint = Instantiate(leftInputWaypoint, currentCanvas.transform);
        currentleftInputWaypoint.GetComponent<Button>().onClick.AddListener(delegate () { Destroy(currentleftInputWaypoint); });
        FocusInput(currentleftInputWaypoint, true);
    }
}
