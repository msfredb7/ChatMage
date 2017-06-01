using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlayer : MonoBehaviour
{
    public List<Road> roads = new List<Road>();
    public Transform teleportingContainer;
    public bool changeRoadOnTeleport;

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
                if (changeRoadOnTeleport)
                    SetRoad(++currentRoadIndex % roads.Count);
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
            TeleportTo(currentRoad.TopHeight, currentRoad.GetTeleportingBottom());
        }
        else if (currentRoad.IsTargetUnderRoad(playerHeight))
        {
            //Teleport from bottom to top !
            TeleportTo(currentRoad.BottomHeight, currentRoad.GetTeleportingTop());
        }
    }

    void FinishTeleport()
    {
        Game.instance.unitsContainer.gameObject.SetActive(true);

        //Save rigidbods data
        for (int i = 0; i < Game.instance.unitsContainer.childCount; i++)
        {
            Unit unit = Game.instance.unitsContainer.GetChild(i).GetComponent<Unit>();
            if (unit != null)
                unit.LoadRigidbody();
        }
    }

    void TeleportTo(float from, float to)
    {
        teleportingContainer.position = new Vector3(0, from, 0);
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

        //Move teleporter
        teleportingContainer.position = new Vector3(0, to, 0);

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
        if (currentRoad == roads[index])
            return;

        //Disable old road
        if (currentRoad != null)
            currentRoad.gameObject.SetActive(false);

        //Enable new road
        currentRoad = roads[index];
        currentRoad.gameObject.SetActive(true);
        currentRoadIndex = index;
    }
}
