using System;
using UnityEngine;

namespace GameEvents
{
    public interface IObjEvent
    {
        UnityEngine.Object AsObject();
    }
    public interface IEvent : IObjEvent
    {
        void Trigger();
    }
    public interface INodedEvent : IObjEvent
    {
        EventGraph Graph { get; set; }
        Rect WindowRect { get; set; }
        void ResetWindowRectPos();
        void ResetWindowRectSize();
        Color DefaultColor();
        string name { get; set; }
        bool CanBeManuallyDestroyed();
        string DefaultLabel();
        string TypeLabel();
        void GetAdditionalMoments(out Moment[] moments, out string[] names);
        bool LinkToGraph();
        void MoveToPos(Vector2 position);
    }
}