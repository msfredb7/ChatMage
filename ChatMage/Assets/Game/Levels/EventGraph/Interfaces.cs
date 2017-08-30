using System;
using UnityEngine;

namespace GameEvents
{
    public interface IObjEvent
    {
        UnityEngine.Object AsObject();
    }
    public interface IBaseEvent : IObjEvent
    {

    }
    public interface IEvent : IBaseEvent
    {
        void Trigger();
    }
    public interface IEvent<T> : IBaseEvent
    {
        void Trigger(T a);
    }
    public interface INodedEvent : IObjEvent
    {
        EventGraph Graph { get; set; }
        Rect WindowRect { get; set; }
        void ResetWindowRectPos();
        void ResetWindowRectSize();
        Color GUIColor();
        string name { get; set; }
        bool CanBeManuallyDestroyed();
        string NodeLabel();
        string TypeLabel();
        void GetAdditionalMoments(out BaseMoment[] moments, out string[] names);
        bool LinkToGraph();
        void MoveToPos(Vector2 position);
        void OnGameReady();
    }
}