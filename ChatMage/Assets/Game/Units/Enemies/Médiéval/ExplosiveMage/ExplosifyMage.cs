using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosifyMage : MonoBehaviour
{
    public ExplosiveMageProjectile projectilePrefab;
    public float lookAheadMultiplier;
    public bool clampTargetToAIArea;

    public void ShootAtTarget(Unit unit, Vector2 emissionPosition)
    {
        Vector2 targetPos = unit.Position;
        if (unit is Vehicle)
        {
            targetPos = PredictAim(unit as Vehicle, emissionPosition);
        }

        ShootAtPosition(targetPos, emissionPosition);
    }

    public void ShootAtPosition(Vector2 position, Vector2 emissionPosition)
    {
        if (clampTargetToAIArea)
            position = Game.Instance.aiArea.ClampToArea(position);

        projectilePrefab.DuplicateGO(emissionPosition, Quaternion.identity).GoTo(position);
    }

    public Vector2 PredictAim(Vehicle enemy, Vector2 emissionPosition)
    {
        var mySpeed = projectilePrefab.trail.GetComponent<Traveler>().TravelSpeed;

        float timeRequired = (enemy.Position - emissionPosition).magnitude / mySpeed;
        float totalLookAhead = timeRequired * lookAheadMultiplier;
        return enemy.Position + enemy.Speed * totalLookAhead;
    }
}
