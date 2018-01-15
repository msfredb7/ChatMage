using System;

namespace CCC.Persistence
{
    public interface IPersistent
    {
        void Init(Action onComplete);
        bool InstantiateNewOnStart();
        UnityEngine.Object InstantiateNew();
    }
}