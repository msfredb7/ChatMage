using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadoutMenu
{
    [System.Serializable]
    public class Loadout
    {
        public LoadoutEquipable[] items;
        public LoadoutEquipable[] cars;
        public LoadoutEquipable[] smashes;

        [System.NonSerialized]
        public LoadoutEquipable chosenCar;
        [System.NonSerialized]
        public LoadoutEquipable chosenSmash;
        public List<LoadoutEquipable> chosenItems;
        public int itemSlots;

        public SimpleEvent onCarChange;
        public SimpleEvent onSmashChange;
        public SimpleEvent onItemChange;

        public Loadout(EquipablePreview[] cars, EquipablePreview[] smashes, EquipablePreview[] items, int itemSlots)
        {
            FillLists(cars, smashes, items);
            this.itemSlots = itemSlots;
        }

        public Loadout(EquipablePreview[] cars, EquipablePreview[] smashes, EquipablePreview[] items, int itemSlots, LoadoutResult previousResult)
        {
            FillLists(cars, smashes, items);
            ApplyResult(previousResult);
            this.itemSlots = itemSlots;
        }

        public LoadoutResult ProduceResult()
        {
            LoadoutResult result = new LoadoutResult();
            result.AddEquipable(chosenCar.preview.equipableAssetName, EquipableType.Car);
            result.AddEquipable(chosenSmash.preview.equipableAssetName, EquipableType.Smash);

            for (int i = 0; i < chosenItems.Count; i++)
            {
                result.AddEquipable(chosenItems[i].preview.equipableAssetName, EquipableType.Item);
            }

            return result;
        }

        private void FillLists(EquipablePreview[] cars, EquipablePreview[] smashes, EquipablePreview[] items)
        {
            //Items
            this.items = new LoadoutEquipable[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                this.items[i] = new LoadoutEquipable(items[i]);
            }

            //Cars
            this.cars = new LoadoutEquipable[cars.Length];
            for (int i = 0; i < cars.Length; i++)
            {
                this.cars[i] = new LoadoutEquipable(cars[i]);
            }

            //Smashes
            this.smashes = new LoadoutEquipable[smashes.Length];
            for (int i = 0; i < smashes.Length; i++)
            {
                this.smashes[i] = new LoadoutEquipable(smashes[i]);
            }
        }

        private void ApplyResult(LoadoutResult result)
        {
            //Car
            if (result.carOrder != null)
                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i].preview.equipableAssetName == result.carOrder.equipableName)
                    {
                        Equip(cars[i]);
                        break;
                    }
                }

            //Smash
            if (result.smashOrder != null)
                for (int i = 0; i < smashes.Length; i++)
                {
                    if (smashes[i].preview.equipableAssetName == result.smashOrder.equipableName)
                    {
                        Equip(smashes[i]);
                        break;
                    }
                }


            //Items  NOTE: Cette iteration est a l'envers des autres pour s'assurer de garder les items dans le bonne ordre.
            //Cette operation est plus longue, mais on a pas trop le choix.

            LinkedList<LoadoutEquipable> remainingItems = new LinkedList<LoadoutEquipable>(items);
            chosenItems = new List<LoadoutEquipable>(result.itemOrders.Count);

            for (int i = 0; i < result.itemOrders.Count; i++)
            {
                LinkedListNode<LoadoutEquipable> itemNode = remainingItems.First;

                while (itemNode != null)
                {
                    //S'agit-il de l'item cherché ?
                    if (itemNode.Value.preview.equipableAssetName == result.itemOrders[i].equipableName)
                    {
                        EquipItem(itemNode.Value);
                        remainingItems.Remove(itemNode);
                        break;
                    }
                    else
                    {
                        //Sinon, allons au prochain
                        itemNode = itemNode.Next;
                    }
                }
            }
        }

        public bool CanEquipMoreItems { get { return chosenItems.Count < itemSlots; } }

        /// <summary>
        /// Equipe le nouvel equipable. Retourne vrai si ça marche
        /// </summary>
        public bool Equip(LoadoutEquipable equipable)
        {
            if (!equipable.IsUnlocked)
                return false;

            switch (equipable.preview.type)
            {
                case EquipableType.Car:
                    EquipReplace(ref chosenCar, equipable);
                    if (onCarChange != null)
                        onCarChange();
                    return true;
                case EquipableType.Smash:
                    EquipReplace(ref chosenSmash, equipable);
                    if (onSmashChange != null)
                        onSmashChange();
                    return true;
                case EquipableType.Item:
                    if (!CanEquipMoreItems || chosenItems.Contains(equipable))
                        return false;
                    EquipItem(equipable);
                    if (onItemChange != null)
                        onItemChange();
                    return true;
                default:
                    throw new System.Exception("Lel da fuck");
            }
        }
        public void UnEquip(LoadoutEquipable equipable)
        {
            if (!equipable.IsEquipped)
                return;

            switch (equipable.preview.type)
            {
                case EquipableType.Car:
                    UnequipSingle(ref chosenCar);
                    if (onCarChange != null)
                        onCarChange();
                    break;
                case EquipableType.Smash:
                    UnequipSingle(ref chosenSmash);
                    if (onSmashChange != null)
                        onSmashChange();
                    break;
                case EquipableType.Item:
                    UnequipItem(equipable);
                    if (onItemChange != null)
                        onItemChange();
                    break;
                default:
                    throw new System.Exception("Lel da fuck");
            }
        }

        #region private Equipe/Unequip

        //Utiliser pour unequip des smash/vehicle
        private void UnequipSingle(ref LoadoutEquipable container)
        {
            container.OnUnequip();
            container = null;

        }
        //Utiliser pour unequip des items
        private void UnequipItem(LoadoutEquipable item)
        {
            item.OnUnequip();
            chosenItems.Remove(item);
        }

        //Utiliser pour equip des smash/vehicle
        private void EquipReplace(ref LoadoutEquipable container, LoadoutEquipable newEquipable)
        {
            if (newEquipable == container)
                return;

            if (container != null)
                container.OnUnequip();

            container = newEquipable;
            container.OnEquip();
        }

        //Utiliser pour equip des items
        private void EquipItem(LoadoutEquipable item)
        {
            item.OnEquip();
            chosenItems.Add(item);
        }
        #endregion
    }

}