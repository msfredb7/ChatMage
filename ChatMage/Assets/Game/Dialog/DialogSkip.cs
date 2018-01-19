using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialoguing
{
    public static class DialogSkip
    {
        private const string PERMANENT_SKIP = "ps";

        public static List<int> temporarySkipList = new List<int>();

        public static void ClearTemporarySkipList()
        {
            temporarySkipList.Clear();
        }
        public static void AddToTemporarySkipList(Dialog dialog)
        {
            if (temporarySkipList == null)
                temporarySkipList = new List<int>();

            if (!IsInTemporarySkip(dialog))
            {
                temporarySkipList.Add(dialog.ID);
            }
        }
        public static bool IsInTemporarySkip(Dialog dialog)
        {
            return temporarySkipList.Contains(dialog.ID);
        }


        #region Permanent Skip
        private static string PermanentSkipKey(Dialog dialog)
        {
            return PERMANENT_SKIP + dialog.IDToString();
        }
        public static void AddToPermanentSkipList(Dialog dialog)
        {
            DataSaver.instance.SetBool(DataSaver.Type.Dialog, PermanentSkipKey(dialog), true);
        }
        public static bool IsInPermanentSkip(Dialog dialog)
        {
            bool value = DataSaver.instance.GetBool(DataSaver.Type.Dialog, PermanentSkipKey(dialog), false);
            return value;
        }
        #endregion

        #region Save
        public static void SavePermanentSkipList()
        {
            DataSaver.instance.SaveData(DataSaver.Type.Dialog);
        }
        public static void SavePermanentSkipListAsync()
        {
            DataSaver.instance.SaveDataAsync(DataSaver.Type.Dialog, null);
        }
        #endregion
    }
}