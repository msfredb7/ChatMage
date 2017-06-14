using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlayer : MonoBehaviour
{
    public List<Road> roads = new List<Road>();
    public Transform teleportingContainer;
    public bool changeRoadOnTeleport;

    public event SimpleEvent onStartTeleport;
    public event SimpleEvent onCompleteTeleport;

    private Road currentRoad;
    private int currentRoadIndex;
    private Transform playerT;
    private bool justTeleported = false;

    public void Init(Transform player)
    {
        playerT = player;
        if (currentRoad == null)
            SetRoad(0);
    }

    void FixedUpdate()
    {
        if (playerT == null)
            return;

        if (!Game.instance.unitsContainer.gameObject.activeSelf)
        {
            if (justTeleported)
            {
                justTeleported = false;
            }
            else
            {
                FinishTeleport();
            }
        }
    }

    void Update()
    {
        if (playerT == null)
            return;

        float playerHeight = playerT.position.y;

        if (currentRoad.IsTargetAboveRoad(playerHeight))
        {
            //Teleport from top to bottom !
            Teleport(false);
        }
        else if (currentRoad.IsTargetUnderRoad(playerHeight))
        {
            //Teleport from bottom to top !
            Teleport(true);
        }
    }

    void FinishTeleport()
    {
        Game.instance.unitsContainer.gameObject.SetActive(true);

        //Load rigidbods data
        for (int i = 0; i < Game.instance.unitsContainer.childCount; i++)
        {
            Unit unit = Game.instance.unitsContainer.GetChild(i).GetComponent<Unit>();
            if (unit != null)
                unit.LoadRigidbody();
        }

        Game.instance.gameCamera.OnCompleteTeleport();
        currentRoad.OnCompleteTeleport();

        if (onCompleteTeleport != null)
            onCompleteTeleport();
    }

    void Teleport(bool fromBottomToTop)
    {
        if (onStartTeleport != null)
            onStartTeleport();

        currentRoad.OnStartTeleport();

        teleportingContainer.position = 
            new Vector3(
                0,
                fromBottomToTop ? currentRoad.BottomHeight : currentRoad.TopHeight,
                0);
        teleportingContainer.gameObject.SetActive(false);

        Transform unitContainer = Game.instance.unitsContainer;

        //Save rigidbods data
        for (int i = 0; i < unitContainer.childCount; i++)
        {
            Unit unit = unitContainer.GetChild(i).GetComponent<Unit>();
            if (unit != null)
                unit.SaveRigidbody();
        }

        unitContainer.gameObject.SetActive(false);

        //Set in teleporter
        for (int i = 0; i < unitContainer.childCount; i++)
        {
            unitContainer.GetChild(i).SetParent(teleportingContainer, true);
            i--;
        }

        //Change Road ?
        if (changeRoadOnTeleport)
            SetRoad(fromBottomToTop ? --currentRoadIndex : ++currentRoadIndex);

        float lastY = teleportingContainer.position.y;

        //Move teleporter
        teleportingContainer.position = 
            new Vector3(
                0,
                fromBottomToTop ? currentRoad.GetTeleportingTop() : currentRoad.GetTeleportingBottom(),
                0);

                //Notify camera
        Game.instance.gameCamera.OnTeleport(teleportingContainer.position.y - lastY);

        //Get out of teleporter
        for (int i = 0; i < teleportingContainer.childCount; i++)
        {
            teleportingContainer.GetChild(i).SetParent(Game.instance.unitsContainer, true);
            i--;
        }

        teleportingContainer.gameObject.SetActive(true);
        justTeleported = true;
    }

    public void SetRoad(int index)
    {
        while (index > 0)
            index -= roads.Count;
        while (index < 0)
            index += roads.Count;

        if (currentRoad == roads[index])
            return;

        //Disable old road
        if (currentRoad != null)
            currentRoad.gameObject.SetActive(false);

        //Enable new road
        currentRoad = roads[index];
        currentRoad.Init(Game.instance.gameCamera, this);
        currentRoad.gameObject.SetActive(true);
        currentRoadIndex = index;
    }
}
