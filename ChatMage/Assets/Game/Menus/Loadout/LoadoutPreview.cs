using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPreview : MonoBehaviour
{
    public Sprite imageUnkown;
    public GameObject panel;
    public Image imagePreview;
    public Text title;
    public Text description;
    public GameObject effectTitle;
    public Text effect;
    public GameObject smashIcon;
    public GameObject specialInputIcon;
    private string specialInputTooltip;

    public void DisplayPreview(EquipablePreview preview)
    {
        Enabled();

        title.text = preview.displayName;
        description.text = preview.description;
        if (preview.unlocked)
        {
            imagePreview.sprite = preview.largeIcon;
            if (!effectTitle.gameObject.activeSelf)
                effectTitle.gameObject.SetActive(true);
            if (!effect.gameObject.activeSelf)
                effect.gameObject.SetActive(true);
            effect.text = preview.effects;
            if (preview.affectSmash)
                smashIcon.SetActive(true);
            else
                smashIcon.SetActive(false);
            if (preview.specialInput)
            {
                specialInputIcon.SetActive(true);
                specialInputTooltip = preview.specialInputTooltipText;
                // TODO: Tooltip a terminer au besoin
            } else
            {
                specialInputIcon.SetActive(false);
            }
        }else
        {
            imagePreview.sprite = imageUnkown;
            effect.gameObject.SetActive(false);
            effectTitle.gameObject.SetActive(false);
            specialInputIcon.SetActive(false);
            smashIcon.SetActive(false);
        }
        // A remettre
        //SoundManager.Play(preview.selectSound);
    }

    public void Disable()
    {
        imagePreview.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        effectTitle.gameObject.SetActive(false);
        effect.gameObject.SetActive(false);
        smashIcon.gameObject.SetActive(false);
        specialInputIcon.gameObject.SetActive(false);
    }

    private void Enabled()
    {
        imagePreview.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        effectTitle.gameObject.SetActive(true);
        effect.gameObject.SetActive(true);
    }
}
