using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Utility
{
    public interface ILottery
    {
        float Weight();
    }

    // Structure de données permettant de choisir un élément ayant une chance d'être pigé
    // parmis un lot d'éléments ayant également leur propre chance d'être pigé (weight)
    public class Lottery
    {
        public Lottery() { }
        public Lottery(ILottery[] items)
        {
            list.AddRange(items);
        }
        private class LotteryItem
        {
            // Constructeur d'un élément qui va faire parti du lot
            public LotteryItem(object obj, float weight)
            {
                this.obj = obj;
                this.weight = weight;
            }
            public object obj = null;
            public float weight = 1;
        }

        ArrayList list = new ArrayList(); // Liste des objets du lot

        public void Add(ILottery item)
        {
            Add(item, item.Weight());
        }

        /// <summary>
        /// Ajout d'un objet dans le lot en fonction de sa chance d'être sélectionné
        /// </summary>
        /// <param name="item"></param>
        /// <param name="weight"></param>
        public void Add(object item, float weight)
        {
            list.Add(new LotteryItem(item, weight));
        }

        /// <summary>
        /// Nombre d'éléments dans le lot
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Sélection d'un élément de facon aléatoire en fonction de leurs chance d'être pigé
        /// </summary>
        /// <returns></returns>
        public object Pick()
        {
            if (list.Count <= 0)
            {
                Debug.LogError("No lottery item to pick from. Add some before picking.");
                return null;
            }

            float totalWeight = 0;
            foreach (ILottery item in list)
            {
                totalWeight += item.Weight();
            }

            float ticket = Random.Range(0, totalWeight);
            float currentWeight = 0;
            foreach (ILottery item in list)
            {
                currentWeight += item.Weight();
                if (ticket < currentWeight)
                {
                    if (item is LotteryItem)
                        return (item as LotteryItem).obj;          //Devrais toujours return ici
                    else
                        return item;
                }
            }

            Debug.LogError("Error in lotery.");
            return null;
        }
    }
}
