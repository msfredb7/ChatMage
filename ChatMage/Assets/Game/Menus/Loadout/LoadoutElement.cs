using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LoadoutMenu
{
    public class LoadoutElement : MonoBehaviour
    {
        //C'est ici qu'on devrais mettre des animation du style:
        //  - Il est highlight parce qu'il est selectionné
        //  - Il a une border shiny parce qu'il est equipped
        //  - Il est grisé parce qu'il n'est pas unlocked
        //  - etc.

        [Header("Linking")]
        public Text text;
        public Button button;
        public Image icon;

        [Header("Visuel Temporaire")]
        public GameObject equippedVisuals;
        public Color normalColor;
        public Color selectedColor;
        public float lockedAlpha;


        public LoadoutEquipable Equipable { get { return equipable; } }
        public delegate void Event(LoadoutElement element);
        public event Event onSelected;


        private LoadoutEquipable equipable;
        private float currentAlpha;

        void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            if (onSelected != null)
                onSelected(this);

            VisuallySelected = true;
        }

        public bool VisuallySelected
        {
            set
            {
                button.image.color = (value ? selectedColor : normalColor) * new Vector4(1, 1, 1, currentAlpha);
            }
        }

        public bool VisuallyLocked
        {
            set
            {
                currentAlpha = value ? lockedAlpha : 1;
                icon.color = value ? Color.black : Color.white;
                text.enabled = !value;
                button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, currentAlpha);
            }
        }

        public bool VisuallyEquipped
        {
            set
            {
                equippedVisuals.SetActive(value);
            }
        }

        public void Fill(LoadoutEquipable equipable)
        {
            //Remove previous listeners
            if (this.equipable != null)
            {
                equipable.onEquip -= OnEquipped;
                equipable.onUnequip -= OnUnequipped;
            }

            this.equipable = equipable;


            //Add new listeners
            equipable.onEquip += OnEquipped;
            equipable.onUnequip += OnUnequipped;

            text.text = equipable.preview.displayName;
            
            if (icon.enabled = equipable.preview.icon != null)
                icon.sprite = equipable.preview.icon;

            VisuallyLocked = !equipable.IsUnlocked;
            VisuallySelected = false;

            VisuallyEquipped = equipable.IsEquipped;

            gameObject.SetActive(true);
        }

        private void OnEquipped(LoadoutEquipable equipable)
        {
            VisuallyEquipped = true;
        }

        private void OnUnequipped(LoadoutEquipable equipable)
        {
            VisuallyEquipped = false;
        }
    }

}
