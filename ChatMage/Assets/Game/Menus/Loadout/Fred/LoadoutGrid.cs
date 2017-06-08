using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoadoutMenu
{
    public class LoadoutGrid : MonoBehaviour
    {
        public LoadoutElement firstElement;
        public RectTransform container;
        public event LoadoutElement.Event onElementClicked;

        [System.NonSerialized]
        List<LoadoutElement> elements = new List<LoadoutElement>();


        void Awake()
        {
            elements.Add(firstElement);

            //Ajoute le listener
            firstElement.onSelected += OnElementClicked;

            firstElement.gameObject.SetActive(false);
        }

        public void Fill(LoadoutEquipable[] equipables)
        {
            int eqCount = equipables == null ? 0 : equipables.Length;

            int index = 0;

            // Spawn / Update les elements
            for (index = 0; index < eqCount; index++)
            {
                if (elements.Count <= index)
                {
                    //On spawn un nouvel element
                    LoadoutElement newElement = Instantiate(firstElement.gameObject, container).GetComponent<LoadoutElement>();

                    //Ajoute le listener
                    newElement.onSelected += OnElementClicked;

                    //On fill un element existant
                    newElement.Fill(equipables[index]);

                    //Ajoute a la liste
                    elements.Add(newElement);
                }
                else
                {
                    //On fill un element existant
                    elements[index].Fill(equipables[index]);
                }
            }

            //Disable les elements restant
            for (; index < elements.Count; index++)
            {
                elements[index].gameObject.SetActive(false);
            }
        }

        void OnElementClicked(LoadoutElement element)
        {
            //On raise l'event. Devrais remonter au loadout ui. 
            if (onElementClicked != null)
                onElementClicked(element);
        }
    }
}