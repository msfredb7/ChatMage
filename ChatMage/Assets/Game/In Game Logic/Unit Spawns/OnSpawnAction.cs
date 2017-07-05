using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class OnSpawnAction : MonoBehaviour
{
    public UnitSpawn attachedSpawn;

    protected virtual void Awake()
    {
        if (attachedSpawn == null && GetComponent<UnitSpawn>() != null)
            attachedSpawn = GetComponent<UnitSpawn>();
    }

    protected virtual void Start()
    {
        if (!Application.isPlaying)
            return;

        attachedSpawn.onUnitSpawned += AttachedSpawn_onUnitSpawned;
    }

    protected abstract void AttachedSpawn_onUnitSpawned(Unit unit);
}
