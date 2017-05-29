using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CCC.Utility
{
    /// <summary>
    /// Semblable au type 'bool'. L'élément vaut 'true' par défaut. S'il y a une ou plusieurs clé d'inséré, il vaut 'false'.
    /// </summary>
    public class Locker
    {
        public UnityEvent onChange = new UnityEvent();
        private List<string> keys = new List<string>();
        public Locker() { }

        /// <summary>
        /// Enlève la première instance d'une clé de ce nom
        /// </summary>
        public void Unlock(string key)
        {
            keys.Remove(key);
            onChange.Invoke();
        }


        /// <summary>
        /// Vérifie si la clé existe
        /// </summary>
        public bool HasKey(string key)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if(keys[i] == key)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Enlève tous les instances de clé de ce nom
        /// </summary>
        public void UnlockAll(string key)
        {
            keys.RemoveAll(x => x.Equals(key));
            onChange.Invoke();
        }

        /// <summary>
        /// Ajoute une clé de ce nom
        /// </summary>
        public void Lock(string key)
        {
            keys.Add(key);
            onChange.Invoke();
        }

        /// <summary>
        /// Ajoute une clé de ce nom, si elle ne s'y trouve pas déjà
        /// </summary>
        public void LockUnique(string key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
                onChange.Invoke();
            }
        }

        /// <summary>
        /// Tous les clé ont-elles été enlevées ? Faux si une ou plusieur clé sont encore présente
        /// </summary>
        public bool IsUnlocked()
        {
            return keys.Count == 0;
        }

        public override string ToString()
        {
            return this;
        }

        public static implicit operator bool (Locker locker)
        {
            return locker.IsUnlocked();
        }
        public static implicit operator string (Locker locker)
        {
            return locker.IsUnlocked().ToString();
        }
    }
}