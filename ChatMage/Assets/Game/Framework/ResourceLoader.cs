using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CCC.Manager;
using System;

public class ResourceLoader : BaseManager<ResourceLoader>
{
    private abstract class PendingRequest
    {
        public PendingRequest(ResourceRequest request)
        {
            this.request = request;
        }
        public ResourceRequest request;
        public abstract void Callback();
    }
    private class PendingRequest<T> : PendingRequest where T : UnityEngine.Object
    {
        public PendingRequest(ResourceRequest request, UnityAction<T> callback) : base(request)
        {
            this.callback = callback;
        }
        public UnityAction<T> callback;
        public override void Callback()
        {
            callback(request.asset as T);
        }
    }

    private List<PendingRequest> pendingRequests = new List<PendingRequest>();

    public override void Init()
    {
        CompleteInit();
    }

    /// <summary>
    /// Passe à travers la listes des loading en attente, et clear ceux qui sont terminé.
    /// </summary>
    void Update()
    {
        if (pendingRequests.Count > 0)
            for (int i = 0; i < pendingRequests.Count; i++)
            {
                if (pendingRequests[i].request.isDone)
                {
                    pendingRequests[i].Callback();
                    pendingRequests.RemoveAt(i);
                    i--;
                }
            }
    }

    public const string ENEMY = "Units/Enemies/";
    public const string PLAYER = "Units/Player";

    static private T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load(path, typeof(T)) as T;
    }
    static private void LoadAsync<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync(path, typeof(T));
        if (callback != null)
        {
            PendingRequest<T> pendingRequest = new PendingRequest<T>(request, callback);
            instance.pendingRequests.Add(pendingRequest);
        }
    }

    static public GameObject LoadPrefab(string path)
    {
        return Load<GameObject>(path);
    }

    static public void LoadPrefabAsync(string path, UnityAction<GameObject> callback)
    {
        LoadAsync(path, callback);
    }

    static public Unit LoadEnemy(string name)
    {
        return LoadPrefab(ENEMY + name).GetComponent<Unit>();
    }
    static public void LoadEnemyAsync(string name, UnityAction<Unit> callback)
    {
        LoadPrefabAsync(ENEMY + name, delegate (GameObject obj)
        {
            if (callback != null)
                callback.Invoke(obj.GetComponent<Unit>());
        });
    }

    static public Vehicle LoadPlayer()
    {
        return LoadPrefab(PLAYER).GetComponent<Vehicle>();
    }
    static public void LoadPlayerAsync(UnityAction<Vehicle> callback)
    {
        LoadPrefabAsync(PLAYER, delegate (GameObject obj)
        {
            if (callback != null)
                callback.Invoke(obj.GetComponent<Vehicle>());
        });
    }
}
