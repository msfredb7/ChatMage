using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashInstructions : MonoBehaviour
{
    [SerializeField] GameObject mobileVersion;
    [SerializeField] GameObject pcVersion;

    void Awake()
    {
        var isMobile = Application.isMobilePlatform;
        pcVersion.SetActive(!isMobile);
        mobileVersion.SetActive(isMobile);

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
