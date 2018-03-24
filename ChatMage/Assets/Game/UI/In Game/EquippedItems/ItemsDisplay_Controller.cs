using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.DesignPattern;

public class ItemsDisplay_Controller : Pool<ItemsDiplay_Ball>
{
    [SerializeField] private ItemsDiplay_Ball ballPrefab;
    [SerializeField] private ItemsDisplay_FlyingIntro introHandler;
    [SerializeField] private ItemsDisplay_Rack rack;
    [SerializeField] private RectTransform ballContainer;
    [SerializeField] private RectTransform destroyedBallContainer;

    public List<ItemsDiplay_Ball> balls = new List<ItemsDiplay_Ball>();

    private PlayerController player;

    public void Init(PlayerController player)
    {
        this.player = player;
    }

    protected override ItemsDiplay_Ball NewItem()
    {
        var newBall = ballPrefab.DuplicateGO(ballContainer);
        newBall.GetGravityComponent().rack = (RectTransform)rack.transform;

        return newBall;
    }

    public Image AddItem(Sprite itemSprite, bool skipIntro = false)
    {
        return AddItem(itemSprite, new Vector3(Screen.width / 2, 2 * Screen.height / 3), skipIntro);
    }
    public Image AddItem(Sprite itemSprite, Vector3 spawnPosition)
    {
        return AddItem(itemSprite, spawnPosition, false);
    }
    private Image AddItem(Sprite itemSprite, Vector3 spawnPosition, bool skipIntro = false)
    {
        var newBall = GetFromPool();
        newBall.Tr.SetParent(destroyedBallContainer);
        balls.Add(newBall);

        if (skipIntro)
        {
            PlaceBallIntoRack(newBall);
            rack.MoveDown();
        }
        else
        {
            //bool useTopSpawn = true;
            //if (player != null)
            //{
            //    //Si le joueur est dans la portion inférieur de l'écran OU qu'il s'y dirige, on utilise le spawn du haut
            //    var yDelta = player.vehicle.Position.y - Game.Instance.gameCamera.Height;
            //    var ySpeed = player.vehicle.Speed.y;
            //    useTopSpawn = (yDelta + ySpeed * 0.5f) < 0;
            //}
            introHandler.HandleBallIntro(newBall, spawnPosition, !rack.CanGoDownFurther(), rack.MoveDown, () => PlaceBallIntoRack(newBall));
        }


        var image = newBall.GetItemImage();
        image.enabled = itemSprite != null;
        image.sprite = itemSprite;
        return image;
    }

    public void RemoveItem(Image imageReference)
    {
        int ballIndex = FindBall(imageReference);
        if (ballIndex < 0)
            return;

        rack.MoveUp();
        balls[ballIndex].Tr.SetParent(destroyedBallContainer);
        balls[ballIndex].BreakAnimation();

        //Interrupt intro if necessary
        introHandler.Interrupt(balls[ballIndex]);

        //Update la parenté de la ball au dessus
        if (ballIndex < balls.Count - 1)
        {
            ItemsDisplay_Gravity gravityComponent = null;
            int underBallIndex = ballIndex - 1;
            if (underBallIndex >= 0)
                gravityComponent = balls[underBallIndex].GetGravityComponent();
            balls[ballIndex + 1].GetGravityComponent().nextGravityComponent = gravityComponent;
        }

        //Enlève de la liste
        balls.RemoveAt(ballIndex);
    }

    private void PlaceBallIntoRack(ItemsDiplay_Ball ball)
    {
        int ballIndex = balls.IndexOf(ball);
        if (ballIndex > 0)
            ball.GetGravityComponent().nextGravityComponent = balls[ballIndex - 1].GetGravityComponent();
        ball.GetGravityComponent().enabled = true;
        ball.Tr.SetParent(ballContainer);
    }

    private int FindBall(Image imageReference)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetItemImage() == imageReference)
                return i;
        }
        return -1;
    }
}
