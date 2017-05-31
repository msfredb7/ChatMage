using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameEvents : MonoBehaviour
{
    LevelScript currentLevel;
    float timer = 0;

    private class DelayedAction
    {
        public float at;
        public Action action;
    }

    LinkedList<DelayedAction> delayedActions = new LinkedList<DelayedAction>();

    public void Init(LevelScript currentLevel)
    {
        this.currentLevel = currentLevel;
    }

    void Update()
    {
        timer += Time.deltaTime * Game.instance.worldTimeScale;
        CheckActionList();
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

    void CheckActionList()
    {
        while (delayedActions.First != null)
        {
            LinkedListNode<DelayedAction> node = delayedActions.First;

            if (node.Value.at <= timer)
            {
                node.Value.action();
                delayedActions.RemoveFirst();
            }
            else
                break;
        }
    }

    public void AddDelayedAction(Action action, float delay)
    {
        DelayedAction da = new DelayedAction() { at = delay + timer, action = action };
        LinkedListNode<DelayedAction> node = delayedActions.First;

        if (node == null)
        {
            delayedActions.AddFirst(da);
        }
        else
            while (true)
            {
                if (node.Value.at > da.at)
                {
                    delayedActions.AddBefore(node, da);
                    break;
                }
                if (node.Next == null)
                {
                    delayedActions.AddAfter(node, da);
                    break;
                }
                node = node.Next;
            }
    }

    public void SetPlayerOnSpawn(float lookAngle)
    {
        Game.instance.Player.vehicle.TeleportPosition(Game.instance.map.mapping.GetRandomSpawnPoint(Waypoint.WaypointType.PlayerSpawn).transform.position);
        Game.instance.Player.vehicle.TeleportDirection(lookAngle);
        LockPlayer();
    }

    // Dialogue (A faire)
    public void Dialogue()
    {
        // TODO : Faire une classe Discussion qui determine le dialogue (qui parle, il dit quoi et quand)
    }

    // Boss (A faire)
    public void BossBattle()
    {
        // TODO : Enum du type de boss ??
    }

    // Cinematic (A faire)
    public void CienamticEvent()
    {
        // TODO : On doit loader une scene puis revenir ou on etait ???
    }

    public void PauseGame(float timeStart, float timeEnd)
    {
        AddDelayedAction(delegate () { Time.timeScale = 0; }, timeStart);
        AddDelayedAction(delegate () { Time.timeScale = 1; }, timeEnd);
    }

    public GameObject ShowUI(GameObject prefab)
    {
        return Instantiate(prefab, Game.instance.ui.gameObject.transform);
    }

    public GameObject ShowUIAtLocation(GameObject prefab, Vector2 position)
    {
        GameObject newUI = Instantiate(prefab, Game.instance.ui.gameObject.transform);
        newUI.transform.position = position;
        return newUI;
    }

    #region Lock Player

    public void LockPlayer()
    {
        Game.instance.Player.vehicle.canMove.Lock("ige");
        Game.instance.Player.playerStats.canTurn.Lock("ige");
    }

    public void UnLockPlayer()
    {
        Game.instance.Player.vehicle.canMove.Unlock("ige");
        Game.instance.Player.playerStats.canTurn.Unlock("ige");
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
            SpawnEntity(unit, newTime, locationType, false);
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
            SpawnEntity(unit, newTime, locationType, true);
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
            SpawnEntity(unit, newTime, Waypoint.WaypointType.enemySpawn, false);
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
            SpawnEntity(unit, newTime, Waypoint.WaypointType.enemySpawn, true);
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
            AddDelayedAction(delegate ()
            {
                Unit newUnit = Game.instance.spawner.SpawnUnitAtRandomLocation(unit, locationType);
                if (function != null)
                    function(newUnit);
            }, time);
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
                    AddDelayedAction(delegate ()
                    {
                        Game.instance.spawner.SpawnUnitAtRandomMultipleDefinedLocation(unit, waypoints, function);
                    }, time);
                }
                else // Mais qu'on veut pas que ce soit random
                {
                    // On fait spawn l'entity a un endroit defini selon un ordonnancement de position
                    AddDelayedAction(delegate ()
                    {
                        Game.instance.spawner.SpawnUnitAtMultipleDefinedLocation(unit, waypoints, function);
                    }, time);
                }
            }
            else // Si on a defini un waypoint
            {
                AddDelayedAction(delegate ()
                {
                    Unit newUnit = Game.instance.spawner.SpawnUnitAtLocation(unit, waypoints[0]);
                    if (function != null)
                        function(newUnit);
                }, time);
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
        AddDelayedAction(Win, time);
    }

    public void LoseIn(float time)
    {
        AddDelayedAction(Lose, time);
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
        LockPlayer();

        Game.instance.Player.playerStats.damagable = false;

        // Si on a gagner
        if (result)
            ShowUI(uiPrefab).GetComponent<GameResultUI>().UpdateResult(true, currentLevel);
        else // Si on a perdu
            ShowUI(uiPrefab).GetComponent<GameResultUI>().UpdateResult(false, currentLevel);
    }

    #endregion
}
