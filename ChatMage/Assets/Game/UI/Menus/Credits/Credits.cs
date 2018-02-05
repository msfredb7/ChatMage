using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : WindowAnimation {

    public Button exit;

	void Start ()
    {
        exit.onClick.AddListener(delegate ()
        {
            Close();
        });
    }
}
