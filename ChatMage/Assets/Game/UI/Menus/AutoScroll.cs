using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class AutoScroll : MonoBehaviour, IScrollHandler, IDragHandler
{
    [SerializeField] Vector2 scrollingSpeed = new Vector2(0, -0.1f);
    [SerializeField] bool scrollOnStart = true;
    [ShowIf("scrollOnStart"), SerializeField] float startDelay = 0;
    [Space, SerializeField] bool stopOnScroll = true;
    [SerializeField] bool stopOnDrag = true;
    [Space, ReadOnlyInEditMode, SerializeField] bool _scrolling = false;

    ScrollRect scrollRect;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        if (scrollOnStart)
        {
            if (startDelay == 0)
                Scrolling = true;
            else
                this.DelayedCall(() => Scrolling = true, startDelay);
        }
    }

    public void StartScrolling() { Scrolling = true; }
    public void StopScrolling() { Scrolling = false; }
    public bool Scrolling { get { return _scrolling; } set { _scrolling = value; } }

    void Update()
    {
        if (Scrolling)
        {
            var position = scrollRect.normalizedPosition;
            position += scrollingSpeed * Time.deltaTime;
            position = position.Clamped(Vector2.zero, Vector2.one);
            scrollRect.normalizedPosition = position;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (stopOnScroll)
            Scrolling = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (stopOnDrag)
            Scrolling = false;
    }
}
