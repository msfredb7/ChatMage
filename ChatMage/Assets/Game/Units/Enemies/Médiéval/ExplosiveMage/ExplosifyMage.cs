using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosifyMage : MonoBehaviour
{
    public ExplosiveMageProjectile projectilePrefab;

    public void ShootAtTarget(Unit unit, Vector2 emissionPosition)
    {
        projectilePrefab.DuplicateGO(emissionPosition, Quaternion.identity).GoTo(unit.Position);
    }
}
