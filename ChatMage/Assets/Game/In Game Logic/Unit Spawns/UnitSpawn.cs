using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSpawn : MonoBehaviour
{
    public bool gizmosAlwaysVisible = true;
    public enum PositionType { World, RelativeToPlayer, RelativeToCamera }
    public PositionType posType;
    public string[] tags;
    public event SimpleEvent onCancelSpawning;
    public event Unit.Unit_Event onUnitSpawned;

    protected InGameEvents events;
    protected float cancelTime;
    private bool canSpawn = true;

    #region Gizmo
    public virtual void DrawGizmos()
    {
        //Color
        Gizmos.color = new Color(0, 1, 0.5f, 0.75f);
        //Matrix GET
        Matrix4x4 stdMatrix = Gizmos.matrix;
        //Matrix SET
        Gizmos.matrix = transform.localToWorldMatrix;
        //Cube
        Gizmos.DrawCube(Vector3.right * 0.5f, new Vector3(1, 0.2f, 0.2f));
        //Matrix reset
        Gizmos.matrix = stdMatrix;
    }

    void OnDrawGizmosSelected()
    {
        if (!gizmosAlwaysVisible)
            DrawGizmos();
    }

    void OnDrawGizmos()
    {
        if (gizmosAlwaysVisible)
            DrawGizmos();
    }
    #endregion

    public void Init(InGameEvents events)
    {
        this.events = events;
    }

    protected virtual void OnUnitSpawned(Unit unit)
    {
        //Check pour des porte, des truc du genre
        if (onUnitSpawned != null)
            onUnitSpawned(unit);
    }

    public abstract Vector2 DefaultSpawnPosition();
    public abstract float DefaultSpawnRotation();

    protected T SpawnUnit<T>(T prefab, Vector2 position, float rotation) where T : Unit
    {
        if (!canSpawn)
            return null;

        T unit = Game.instance.SpawnUnit(prefab, position);
        unit.Rotation = rotation;

        OnUnitSpawned(unit);

        return unit;
    }

    public virtual T SpawnUnit<T>(T prefab) where T : Unit
    {
        T unit = SpawnUnit(prefab, DefaultSpawnPosition(), DefaultSpawnRotation());
        return unit;
    }
    public void SpawnUnit<T>(T prefab, float delay) where T : Unit
    {
        float time = events.GameTime;
        if (delay <= 0)
            SpawnUnit(prefab);
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    SpawnUnit(prefab);
            }, delay);
    }
    public void SpawnUnit<T>(T prefab, float delay, Action<Unit> callback) where T : Unit
    {
        float time = events.GameTime;
        if (delay <= 0)
            callback(SpawnUnit(prefab));
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    callback(SpawnUnit(prefab));
            }, delay);
    }

    public virtual void SpawnUnits(Unit[] units, float interval)
    {
        for (int i = 0; i < units.Length; i++)
        {
            float delay = i * interval;
            SpawnUnit(units[i], delay);
        }
    }
    public virtual void SpawnUnits(List<Unit> units, float interval)
    {
        for (int i = 0; i < units.Count; i++)
        {
            float delay = i * interval;
            SpawnUnit(units[i], delay);
        }
    }
    public virtual void SpawnUnits(Unit[] units, float interval, Action<Unit> callback)
    {
        for (int i = 0; i < units.Length; i++)
        {
            float delay = i * interval;
            SpawnUnit(units[i], delay, callback);
        }
    }
    public virtual void SpawnUnits(List<Unit> units, float interval, Action<Unit> callback)
    {
        for (int i = 0; i < units.Count; i++)
        {
            float delay = i * interval;
            SpawnUnit(units[i], delay, callback);
        }
    }

    public void SpawnUnits(Unit[] units, float interval, float delay)
    {
        float time = events.GameTime;
        if (delay <= 0)
            SpawnUnits(units, interval);
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    SpawnUnits(units, interval);
            }, delay);
    }
    public void SpawnUnits(List<Unit> units, float interval, float delay)
    {
        float time = events.GameTime;
        if (delay <= 0)
            SpawnUnits(units, interval);
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    SpawnUnits(units, interval);
            }, delay);
    }
    public void SpawnUnits(Unit[] units, float interval, float delay, Action<Unit> callback)
    {
        float time = events.GameTime;
        if (delay <= 0)
            SpawnUnits(units, interval, callback);
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    SpawnUnits(units, interval, callback);
            }, delay);
    }
    public void SpawnUnits(List<Unit> units, float interval, float delay, Action<Unit> callback)
    {
        float time = events.GameTime;
        if (delay <= 0)
            SpawnUnits(units, interval, callback);
        else
            events.AddDelayedAction(delegate ()
            {
                if (time > cancelTime)
                    SpawnUnits(units, interval, callback);
            }, delay);
    }

    public bool CanSpawn { get { return canSpawn; } }

    public void EnableSpawning()
    {
        canSpawn = true;
    }

    public void CancelSpawning()
    {
        canSpawn = false;
        cancelTime = events.GameTime;
        if (onCancelSpawning != null)
            onCancelSpawning();
    }
}
