using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDisplay_Rack : MonoBehaviour
{
    [SerializeField] private float moveInterval = 128;
    [SerializeField] private Ease moveUpEase = Ease.InOutSine;
    [SerializeField] private float moveUpDuration = 0.35f;
    [SerializeField] private Ease moveDownEase = Ease.OutElastic;
    [SerializeField] private float moveDownDuration = 0.35f;
    [SerializeField] private int minFloors = 1;
    [SerializeField] private int maxFloors = 10;

    private int currentFloor = -1;
    private Tween currentAnimation;
    private RectTransform tr;

    void Awake()
    {
        tr = (RectTransform)transform;

        Canvas.ForceUpdateCanvases();
        currentFloor = -1;
        SetHeight(minFloors, false);
    }

    public void SetHeight(int floor) { SetHeight(floor, true); }
    public void SetHeight(int floor, bool animated)
    {
        // Stop previous animation
        StopAnimating();

        // Clamp floor
        var visualFloor = floor.Clamped(minFloors, maxFloors);

        if (currentFloor == visualFloor)
        {
            // Apply to current floor
            currentFloor = floor;
            return;
        }

        // Apply to current floor
        currentFloor = floor;


        // Get target size
        var targetSize = GetSizeDeltaAt(visualFloor);

        // Move!
        if (animated)
        {
            // Animated
            var currentSize = tr.sizeDelta;

            //Which direction ?
            if (targetSize.y > currentSize.y)
            {
                //Going down
                tr.DOSizeDelta(targetSize, moveDownDuration).SetEase(moveDownEase);
            }
            else
            {
                //Going up
                tr.DOSizeDelta(targetSize, moveUpDuration).SetEase(moveUpEase);
            }
        }
        else
        {
            // Instant (no animation)
            tr.sizeDelta = targetSize;
        }

    }

    public void MoveDown() { MoveDown(true); }
    public void MoveDown(bool animated)
    {
        SetHeight(currentFloor + 1, animated);
    }

    public int GetCurrentFloor() { return currentFloor; }
    public int GetCurrentVisualFloor() { return currentFloor.Clamped(minFloors, maxFloors); }
    public bool CanGoDownFurther() { return currentFloor < maxFloors; }


    public void MoveUp() { MoveUp(true); }
    public void MoveUp(bool animated)
    {
        SetHeight(currentFloor - 1, animated);
    }

    private float GetHeightAt(int floor)
    {
        return moveInterval * floor;
    }
    private Vector2 GetSizeDeltaAt(int floor)
    {
        return new Vector2(tr.sizeDelta.x, GetHeightAt(floor));
    }

    void StopAnimating()
    {
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
            currentAnimation = null;
        }
    }

}
