using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpScript : WindowAnimation {

    public Text text;
    public GameObject cancel;
    public GameObject confirm;
    public GameObject ok;
    public Camera cam;

    public void Start()
    {
        if (Camera.main == null)
            cam.gameObject.SetActive(true);
        MasterManager.Sync();
    }

    public void ChangeToOKPopUp()
    {
        cancel.SetActive(false);
        confirm.SetActive(false);
    }

    public void ChangeToChoicePopUp()
    {
        ok.SetActive(false);
    }

    public void ChangeToPopUp()
    {
        ok.SetActive(false);
        cancel.SetActive(false);
        confirm.SetActive(false);
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
