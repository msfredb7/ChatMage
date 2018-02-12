using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewItemNotification : WindowAnimation {

    [Header("Notification")]
    public Image itemIcon;
    public Text itemName;
    public Text itemDescription;

    public float duration = 5f;
	
	public void DisplayNewItem(EquipablePreview itemPreview)
    {
        itemIcon.sprite = itemPreview.icon;
        itemName.text = itemPreview.displayName;
        itemDescription.text = itemPreview.effects;
        Open(delegate() {
            this.DelayedCall(delegate ()
            {
                Close();
            }, duration);
        });
    }
}
