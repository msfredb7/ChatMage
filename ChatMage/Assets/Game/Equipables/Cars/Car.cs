using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public abstract class Car : Equipable
{
    public PlayerController playerPrefab;
    //public Sprite carSprite;
    //public float spriteSize = 1;
    //public Vector2 spriteLocalPos = Vector2.zero;

    //public override void Init(PlayerController player)
    //{
    //    base.Init(player);

    //    if (carSprite != null)
    //    {
    //        PlayerCarTriggers playerCar = player.playerCarTriggers;
    //        SpriteRenderer renderer = playerCar.carSpriteRenderer;
    //        renderer.sprite = carSprite;
    //        renderer.transform.localPosition = spriteLocalPos;
    //        renderer.transform.localScale *= spriteSize;
    //    }
    //}

    public abstract void OnInputUpdate(float horizontalInput);
}
