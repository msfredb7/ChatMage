using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameEvents : MonoBehaviour
{
    LevelScript currentLevel;

    public void Init(LevelScript currentLevel)
    {
        this.currentLevel = currentLevel;
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Dialogue (A faire)
    public void Dialogue()
    {
        // TODO : Faire une classe Discussion qui determine le dialogue (qui parle, il dit quoi et quand)
    }

    // Boss
    public void BossBattle()
    {
        // TODO : Enum du type de boss ??
    }

    // Cienamtic
    public void CienamticEvent()
    {
        // TODO : On doit loader une scene puis revenir ou on etait ???
    }

    public void PauseGame(float timeStart, float timeEnd)
    {
        DelayManager.LocalCallTo(delegate () { Time.timeScale = 0; }, timeStart, this);
        DelayManager.LocalCallTo(delegate () { Time.timeScale = 1; }, timeEnd, this);
    }

    public GameObject ShowUI(GameObject prefab)
    {
        return Instantiate(prefab, Game.instance.ui.gameObject.transform);
    }

    #region Lock Player

    public void LockPlayer()
    {
        Vector2 bounds = Game.instance.WorldBounds;
        Vector3 startPos = new Vector3(bounds.x / 2, bounds.y / 3);
        LockPlayer(startPos);
    }

    public void LockPlayer(Vector3 position)
    {
        Vector3 startPos = position;
        Game.instance.Player.transform.position = startPos;
        Game.instance.Player.vehicle.canMove.Lock("intro");
        Game.instance.Player.playerStats.canTurn.Lock("intro");
        Game.instance.Player.vehicle.targetDirection = 90;
    }

    public void UnLockPlayer()
    {
        Game.instance.Player.vehicle.canMove.Unlock("intro");
        Game.instance.Player.playerStats.canTurn.Unlock("intro");
    }

#endregion



    #region Spawn Units

    /// <summary>
    /// WAVE - Spawn des entity d'un certain type a un temp precis a des endroits selon un type
    /// </summary>
    public void SpawnEntityFixedTime(Unit unit, float time, Waypoint.WaypointType locationType, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnEntity(unit, time, locationType, false);
        }
    }

    /// <summary>
    /// WAVE - Spawn des entity d'un certain type a un temp precis a des endroits randoms selon un type
    /// </summary>
    public void SpawnEntityFixedTimeRandom(Unit unit, float time, Waypoint.WaypointType locationType, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnEntity(unit, time, locationType, true);
        }
    }

    /// <summary>
    /// WAVE - Spawn des entity d'un certain type a un temp precis a des endroits predefinit selon un type
    /// </summary>
    public void SpawnEntityFixedTimeNotRandom(Unit unit, float time, List<Waypoint> waypoints, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnEntity(unit, time, Waypoint.WaypointType.enemySpawn, false);
        }
    }

    /// <summary>
    /// WAVE - Spawn des entity d'un certain type a un temp precis a des endroits predefinit selon un type
    /// </summary>
    public void SpawnEntityFixedTimeWithLocationRandom(Unit unit, float time, List<Waypoint> waypoints, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnEntity(unit, time, Waypoint.WaypointType.enemySpawn, true);
        }
    }

    /// <summary>
    /// WAVE - Spawn des entity d'un certain type a un temp precis de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntityFixedTime(Unit unit, float time, Waypoint.WaypointType locationType, int amount = 1, bool random = true, Action<Unit> function = null, List<Waypoint> waypoints = null)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnEntity(unit, time, locationType, random, function, waypoints);
        }
    }

    /// <summary>
    /// FILE - Spawn des entity d'un certain type au fil du temps de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntitySpreadTime(Unit unit, float time, Waypoint.WaypointType locationType, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            float newTime = (time / amount) * i;
            SpawnEntity(unit, time, locationType, false);
        }
    }

    /// <summary>
    /// FILE - Spawn des entity d'un certain type au fil du temps de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntitySpreadTimeRandom(Unit unit, float time, Waypoint.WaypointType locationType, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            float newTime = (time / amount) * i;
            SpawnEntity(unit, time, locationType, true);
        }
    }

    /// <summary>
    /// FILE - Spawn des entity d'un certain type au fil du temps de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntitySpreadTimeNotRandom(Unit unit, float time, List<Waypoint> waypoints, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            float newTime = (time / amount) * i;
            SpawnEntity(unit, time, Waypoint.WaypointType.enemySpawn, false);
        }
    }

    /// <summary>
    /// FILE - Spawn des entity d'un certain type au fil du temps de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntitySpreadTimeWithLocationRandom(Unit unit, float time, List<Waypoint> waypoints, int amount)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            float newTime = (time / amount) * i;
            SpawnEntity(unit, time, Waypoint.WaypointType.enemySpawn, true);
        }
    }

    /// <summary>
    /// FILE - Spawn des entity d'un certain type au fil du temps de maniere random (ou pas) a des endroits predefinit (ou pas) 
    /// </summary>
    public void SpawnEntitySpreadTime(Unit unit, float time, Waypoint.WaypointType locationType, int amount = 1, bool random = true, Action<Unit> function = null, List<Waypoint> waypoints = null)
    {
        if (currentLevel.isOver) return;

        for (int i = 0; i < amount; i++)
        {
            float newTime = (time / amount) * i;
            SpawnEntity(unit, newTime, locationType, random, function, waypoints);
        }
    }

    private void SpawnEntity(Unit unit, float time, Waypoint.WaypointType locationType, bool random = true, Action<Unit> function = null, List<Waypoint> waypoints = null)
    {
        // Si on a defini aucun waypoints
        if (waypoints == null)
        {
            // On fait spawn les entity a des endroits random 
            DelayManager.LocalCallTo(delegate ()
            {
                Unit newUnit = Game.instance.spawner.SpawnUnitAtRandomLocation(unit, locationType);
                if (function != null)
                    function(newUnit);
            }, time, this);
        }
        else // Si on a defini des waypoints
        {
            // Si on a defini plus d'un waypoint
            if (waypoints.Count > 1)
            {
                // Mais qu'on veut que ce soit random
                if (random)
                {
                    // On spawn l'entity a un endroit random parmis des position defini
                    DelayManager.LocalCallTo(delegate ()
                    {
                         Game.instance.spawner.SpawnUnitAtRandomMultipleDefinedLocation(unit, waypoints, function);
                    }, time, this);
                }
                else // Mais qu'on veut pas que ce soit random
                {
                    // On fait spawn l'entity a un endroit defini selon un ordonnancement de position
                    DelayManager.LocalCallTo(delegate ()
                    {
                        Game.instance.spawner.SpawnUnitAtMultipleDefinedLocation(unit, waypoints, function);
                    }, time, this);
                }
            }
            else // Si on a defini un waypoint
            {
                DelayManager.LocalCallTo(delegate ()
                {
                    Unit newUnit = Game.instance.spawner.SpawnUnitAtLocation(unit, waypoints[0]);
                    if (function != null)
                        function(newUnit);
                }, time, this);
            }
        }
    }

#endregion



    #region Game Ending

    // Box in center
    public void Reward()
    {
        // TODO : A determiner
    }

    public void WinIn(float time)
    {
        DelayManager.LocalCallTo(Win, time, this);
    }

    public void LoseIn(float time)
    {
        DelayManager.LocalCallTo(Lose, time, this);
    }

    protected void Win()
    {
        currentLevel.hasWon = true;
        currentLevel.End();
    }

    protected void Lose()
    {
        currentLevel.hasWon = false;
        currentLevel.End();
    }

    // Outro
    public void Outro(bool result, GameObject uiPrefab)
    {
        LockPlayer(Game.instance.Player.vehicle.transform.position);
        Game.instance.Player.playerStats.damagable = false;
        // Si on a gagner
        if (result)
            ShowUI(uiPrefab).GetComponent<GameResultUI>().UpdateResult(true, currentLevel);
        else // Si on a perdu
            ShowUI(uiPrefab).GetComponent<GameResultUI>().UpdateResult(false, currentLevel);
    }

#endregion

}
