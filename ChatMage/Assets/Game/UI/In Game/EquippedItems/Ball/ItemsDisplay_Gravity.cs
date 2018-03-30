using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDisplay_Gravity : MonoBehaviour
{
    public ItemsDisplay_Gravity nextGravityComponent;
    public RectTransform rack;

    [SerializeField] private FloatReference verticalAcceleration;
    [SerializeField] private FloatReference velocityKill;
    [SerializeField] private FloatReference bounce;
    [SerializeField] private float maxVerticalVel = 500;
    [SerializeField] private bool selfUpdate = false;
    [SerializeField] AudioPlayable hitSFX;

    private RectTransform tr;
    private float currentSpeed = 0;

    private bool allowedToSFX;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        allowedToSFX = true;
    }

    void Update()
    {
        if (selfUpdate)
            UpdatePosition(Time.unscaledDeltaTime);
    }

    public void UpdatePosition(float deltaTime)
    {
        if (!enabled)
            return;

        var minHeight = GetTargetBottomHeight();
        var bottomHeight = tr.anchoredPosition.y - tr.sizeDelta.y / 2;

        if (bottomHeight > minHeight)
        {
            //var deltaTime = Time.unscaledDeltaTime;
            currentSpeed += verticalAcceleration * deltaTime;
            bottomHeight += currentSpeed * deltaTime;

            if (bottomHeight <= minHeight)
            {
                if (allowedToSFX)
                {
                    allowedToSFX = false;
                    if (hitSFX != null)
                        DefaultAudioSources.PlayStaticSFX(hitSFX);
                }

                if (currentSpeed.Abs() > velocityKill && bounce > 0)
                {
                    //Bounce !
                    bottomHeight = (minHeight - bottomHeight) + minHeight;
                    currentSpeed = (-currentSpeed * bounce).Capped(maxVerticalVel);
                }
                else
                {
                    bottomHeight = minHeight;
                    currentSpeed = 0;
                }
            }
        }
        else
        {
            currentSpeed = Mathf.Max(currentSpeed, 0);
            bottomHeight = minHeight;
        }

        tr.anchoredPosition = new Vector2(0, bottomHeight + tr.sizeDelta.y / 2);
    }

    public float GetTopHeight() { return tr.anchoredPosition.y + tr.sizeDelta.y / 2; }

    private float GetTargetBottomHeight()
    {
        return nextGravityComponent == null ? -rack.sizeDelta.y : nextGravityComponent.GetTopHeight();
    }
}
