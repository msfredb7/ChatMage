using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float downSize = 0.93f;
    public float upSize = 1f;
    [Range(0, 10)]
    public float pressSpeed = 4;
    [Range(0, 10)]
    public float releaseSpeed = 0.175f;
    public Ease pressEase = Ease.InSine;
    public Ease releaseEase = Ease.OutElastic;
    public bool independentUpdate = false;

    private RectTransform tr;
    private bool clickSequence = false;
    private Tween currentTween = null;

    void Awake()
    {
        if (GetComponent<RectTransform>() == null)
        {
            Debug.LogError("Cannot put UI_ClickAnimation on a non-UI gameObject");
            enabled = false;
            return;
        }

        tr = GetComponent<RectTransform>();
    }

    public void ManualClickAnim()
    {
        clickSequence = true;
        DownAnim().OnComplete(delegate ()
        {
            clickSequence = false;
            UpAnim();
        });
    }

    private Tween DownAnim()
    {
        currentTween.Kill();
        return currentTween = tr.DOScale(Vector3.one * downSize, 0.1f / (pressSpeed + 0.001f)).SetEase(pressEase).SetUpdate(independentUpdate);
    }

    private Tween UpAnim()
    {
        currentTween.Kill();
        return currentTween = tr.DOScale(Vector3.one * upSize, 0.1f / (releaseSpeed + 0.001f)).SetEase(releaseEase).SetUpdate(independentUpdate);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickSequence = true;
        DownAnim();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickSequence)
        {
            clickSequence = false;
            UpAnim();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (clickSequence)
        {
            clickSequence = false;
            UpAnim();
        }
    }
}