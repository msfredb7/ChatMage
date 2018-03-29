using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnBack);
    }

    void OnBack()
    {
        LoadingScreen.TransitionTo(MainMenu.SCENENAME, null);
    }
}
