using CCC.Manager;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTutorial : BaseTutorial {

    public string positionLeftInputWaypointPrefabName;
    public string positionRightInputWaypointPrefabName;

    private GameObject leftInputWaypoint;
    private GameObject rightInputWaypoint;

    private GameObject currentleftInputWaypoint;
    private GameObject currentrightInputWaypoint;

    /// <summary>
    /// Utiliser pour l'initialisation des variables
    /// </summary>
    public override void Begin(GameObject canvas, LoadQueue queue)
    {
        base.Begin(canvas, queue);
        queue.AddUI(positionLeftInputWaypointPrefabName, (x) => leftInputWaypoint = x);
        queue.AddUI(positionRightInputWaypointPrefabName, (x) => rightInputWaypoint = x);
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
        // TODO
        currentleftInputWaypoint = Instantiate(leftInputWaypoint, currentCanvas.transform);
        currentleftInputWaypoint.transform.SetAsFirstSibling();
        currentleftInputWaypoint.GetComponent<Button>().onClick.AddListener(delegate () { Destroy(currentleftInputWaypoint); });
        currentleftInputWaypoint.GetComponent<Button>().interactable = false;
        FocusInput(currentleftInputWaypoint, true);
    }

    public void ShowRightInput()
    {
        // TODO
        currentrightInputWaypoint = Instantiate(rightInputWaypoint, currentCanvas.transform);
        currentrightInputWaypoint.transform.SetAsFirstSibling();
        currentrightInputWaypoint.GetComponent<Button>().onClick.AddListener(delegate () { Destroy(currentrightInputWaypoint); });
        currentrightInputWaypoint.GetComponent<Button>().interactable = false;
        FocusInput(currentrightInputWaypoint, true);
    }

    public void ShowVehicle()
    {
        Vector2 viewportPosition = Game.instance.gameCamera.cam.WorldToViewportPoint(Game.instance.Player.vehicle.transform.position);
        viewportPosition.Scale(new Vector2(1920, 1080));
        FocusSpotLight(viewportPosition, true, true);
        ShowInfo("Here you are ! This vehicle is the only direct thing you control. You'll use it to make your way throught. Good luck !",2);
    }

    public void ShowHp()
    {
        MoveSpotlight(Game.instance.ui.healthdisplay.hearthCountainer.transform.GetChild(1).GetComponent<RectTransform>().position,delegate() {
            Sequence sq = DOTween.Sequence().SetUpdate(true);
            sq.InsertCallback(2, delegate ()
            {
                DeFocusSpotLight(true, null);
            });
        });
    }

    public void ShowEnemy()
    {

    }

    public void ShowSmash()
    {

    }

    public void ShowButton()
    {

    }

    public void ShowItem()
    {

    }
}
