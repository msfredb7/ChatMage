using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class RemoteTriggerListener : MonoBehaviour
{
    IRemoteTriggerHandler handler = null;
    
    public void SetHandler(IRemoteTriggerHandler handler)
    {
        this.handler = handler;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (handler != null)
            handler.RemoteTriggerEnter2D(other, this);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (handler != null)
            handler.RemoteTriggerExit2D(other, this);
    }
}

public interface IRemoteTriggerHandler
{
    void RemoteTriggerEnter2D(Collider2D other, RemoteTriggerListener source);
    void RemoteTriggerExit2D(Collider2D other, RemoteTriggerListener source);
}
