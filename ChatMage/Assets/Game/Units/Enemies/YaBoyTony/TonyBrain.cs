using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyBrain : EnemyBrain<TonyVehicle>
{
    public float roamTime;

    private float roamingRemains = 5;
    private bool isSnapping = false;

    protected override void Start()
    {
        base.Start();

        vehicle.onTeleportPosition += Vehicle_onTeleportPosition;
    }

    private void Vehicle_onTeleportPosition(Unit unit, Vector2 delta)
    {
        if (!isSnapping && roamingRemains <= 0)
            roamingRemains = roamTime;
    }

    protected override void UpdateWithTarget()
    {
        //Tony n'est pas suppos� avoir de target
        UpdateWithoutTarget();
    }

    protected override void UpdateWithoutTarget()
    {
        if(roamingRemains > 0)
        {
            roamingRemains -= vehicle.DeltaTime();

            if(roamingRemains <= 0)
            {
                SetBehavior(null);
                vehicle.GotoPosition(GetNewZone(), OnArriveToZone);
            }
            else
            {
                if (CanGoTo<WanderBehavior>())
                {
                    SetBehavior(new WanderBehavior(vehicle));
                }
            }
        }
    }

    Vector2 GetNewZone()
    {
        GameCamera cam = Game.instance.gameCamera;
        Vector2 screenSize = cam.ScreenSize;
        float hfScreenX = screenSize.x / 2;
        float hfScreenY = screenSize.y / 2;
        float x = Random.Range(-hfScreenX, hfScreenX);
        float y = Random.Range(-hfScreenY, hfScreenY);

        Vector2 v = new Vector2(x * 0.75f, y * 0.75f);
        v = cam.AdjustVector(v);

        Vector2 pos = v + cam.Center;
        return Game.instance.map.VerifyPosition(pos, vehicle.animator.ZoneWidth);
    }

    private void OnArriveToZone()
    {
        SetBehavior(new TonyTakeSnapBehavior(vehicle, OnSnapComplete));
        isSnapping = true;
    }

    private void OnSnapComplete()
    {
        isSnapping = false;
        roamingRemains = roamTime;
    }
}
