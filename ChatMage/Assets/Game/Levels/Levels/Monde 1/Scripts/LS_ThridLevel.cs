using FullInspector;
using FullSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LS_ThridLevel : LevelScript
{
    [NonSerialized, NotSerialized]
    ArmyWallScript armyWall;


    [NonSerialized, NotSerialized]
    List<Unit> enemyBuffer = null;
    [NonSerialized, NotSerialized]
    List<Unit> _toKill = null;

    protected override void OnGameReady()
    {
        //Get army wall + disable collision
        armyWall = Game.instance.map.mapping.GetTaggedObject("army wall").GetComponent<ArmyWallScript>();
        armyWall.DisableCollision();

        //On l'approche du joueur 
        Game.instance.events.AddDelayedAction(() =>
        {
            armyWall.BringCloseToPlayer();
        },
        1.8f);
    }

    protected override void OnGameStarted()
    {
        //Re-enable la collision du army wall
        armyWall.EnableCollision();
    }

    public override void OnReceiveEvent(string message)
    {
        switch (message)
        {
            case "onIntersect": //Lorsque le joueur arrive a un barrage
                //On kill les units de la wave precedente
                KillToKill();

                //On marque la wave courrant comme etant 'a tuer la prochaine fois'
                _toKill = enemyBuffer;

                //On avance les units proche du joueur
                BringEnemiesToPlayer();

                //On clear le buffer
                ClearEnemyBuffer();
                break;

            case "boss":
                KillToKill();
                ClearEnemyBuffer();

                EnterBossRoom();
                break;
            default:
                break;
        }
    }

    public void AddToEnemyBuffer(Unit unit)
    {
        if (enemyBuffer == null)
            enemyBuffer = new List<Unit>();
        enemyBuffer.Add(unit);
        unit.onDeath += RemoveFromBuffer;
    }

    private void ClearEnemyBuffer()
    {
        if (enemyBuffer == null)
            return;

        foreach (Unit unit in enemyBuffer)
        {
            unit.onDeath -= RemoveFromBuffer;
        }
        enemyBuffer = null;
    }

    private void RemoveFromBuffer(Unit unit)
    {
        enemyBuffer.Remove(unit);
    }

    private void KillToKill()
    {
        if (_toKill == null)
            return;

        foreach (Unit unit in _toKill)
        {
            unit.ForceDie();
        }
        _toKill = null;
    }

    private void BringEnemiesToPlayer()
    {
        float minY = Game.instance.gameCamera.Bottom - 1;
        float minX = (GameCamera.DEFAULT_SCREEN_WIDTH / -2) + 1;
        float xLength = minX.Abs() * 2;
        float i = 0;

        foreach (Unit enemy in enemyBuffer)
        {
            float delta = minY - enemy.Position.y;
            if (delta > 0)
            {
                float x = ((7.5f * i) % xLength) + minX;
                float y = enemy.Position.y + delta * 0.65f;
                enemy.TeleportPosition(new Vector2(x, y));
                i++;
            }
        }
    }

    private void EnterBossRoom()
    {
        SpriteRenderer bossShadow = Game.instance.map.mapping.GetTaggedObject("boss room shadow").GetComponent<SpriteRenderer>();
        bossShadow.DOFade(0, 1.5f).OnComplete(() => bossShadow.enabled = false);
    }
}
