
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutElementInspector : MonoBehaviour
{
    private enum ButtonType { None = -1, Equip = 0, Unequip = 1, Shop = 2, BuySlots = 3 }

    [Header("Basic info")]
    public Text title;
    public Text description;
    public Image imagePreview;

    [Header("Effect")]
    //public GameObject effectTitle;
    public Text effect;

    [Header("3 buttons")]
    public Button equipButton;
    public Button unequipButton;
    public Button shopButton;
    public Button buySlotsButton;

    //Leur events respectifs
    public event LoadoutEquipable.Event onEquipClicked;
    public event LoadoutEquipable.Event onUnequipClicked;
    public event LoadoutEquipable.Event onShopClicked;
    public event LoadoutEquipable.Event onBuySlotsClicked;

    [Header("Other")]
    public Sprite lockedSprite;

    private LoadoutEquipable currentEquipable = null;

    void Awake()
    {
        equipButton.onClick.AddListener(OnEquipClick);
        unequipButton.onClick.AddListener(OnUnequipClick);
        shopButton.onClick.AddListener(OnShopClick);
        buySlotsButton.onClick.AddListener(OnBuySlotsClick);
    }

    /// <summary>
    /// Passer 'null' en paramettre pour cacher l'inspecteur
    /// </summary>
    public void Fill(LoadoutEquipable equipable)
    {
        //Ne pas redisplay pour rien
        if (currentEquipable == equipable && currentEquipable != null)
            return;

        currentEquipable = equipable;

        //Basic info trio
        imagePreview.gameObject.SetActive(equipable != null);
        title.gameObject.SetActive(equipable != null);

        //Effect text
        effect.gameObject.SetActive(equipable != null);

        if (equipable == null)
        {
            SetButton(ButtonType.None);
        }
        else
        {
            //Basic info
            imagePreview.sprite = equipable.preview.icon;

            if (equipable.IsUnlocked)
            {
                //Unlocked !

                //Text
                title.text = equipable.preview.displayName;
                effect.text = equipable.preview.effects;

                //Sprite
                imagePreview.color = Color.white;

                //Button
                if (equipable.IsEquipped)
                    SetButton(ButtonType.Unequip);
                else
                    SetButton(ButtonType.Equip);
            }
            else
            {
                //Locked !
                title.text = "??????????";
                effect.text = "???????????????????????????????????????????????";

                //Sprite
                imagePreview.color = Color.black;

                //Button
                //Temporaire. Lorsqu'on veut l'enlever, simplement delete les lignes 101, 103 et 104
                //  ET EquipablePreview.cs ligne 12
                if (equipable.preview.canBeCheatUnlocked)
                    SetButton(ButtonType.Shop);
                else
                    SetButton(ButtonType.None);
            }

            // A remettre
            //SoundManager.Play(preview.selectSound);
        }
    }

    private void SetButton(ButtonType type)
    {
        buySlotsButton.gameObject.SetActive(type == ButtonType.BuySlots);
        equipButton.gameObject.SetActive(type == ButtonType.Equip);
        unequipButton.gameObject.SetActive(type == ButtonType.Unequip);
        shopButton.gameObject.SetActive(type == ButtonType.Shop);
    }

    public void ShowBuySlotsButton()
    {
        SetButton(ButtonType.BuySlots);
    }

    #region Click Events
    private void OnEquipClick()
    {
        ClickCheckThrow();

        if (onEquipClicked != null)
            onEquipClicked(currentEquipable);
    }
    private void OnUnequipClick()
    {
        ClickCheckThrow();

        if (onUnequipClicked != null)
            onUnequipClicked(currentEquipable);
    }
    private void OnShopClick()
    {
        ClickCheckThrow();

        if (onShopClicked != null)
            onShopClicked(currentEquipable);
    }
    private void OnBuySlotsClick()
    {
        ClickCheckThrow();

        if (onBuySlotsClicked != null)
            onBuySlotsClicked(currentEquipable);
    }

    private void ClickCheckThrow()
    {
        if (currentEquipable == null)
            throw new System.Exception("Ne devrais pas pouvoir clicker sur se bouton lorsque le 'currentEquipable' est null");
    }
    #endregion
}
