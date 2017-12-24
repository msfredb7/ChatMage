using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using Dialoguing;
using CCC.Manager;
using UnityEngine.Events;

public class Fred_TestScript : MonoBehaviour
{
    public GameObject[] clients;
    public int nextClient = 0;
    public SharedTables sharedTables;

    void Start()
    {
        MasterManager.Sync();
        Debug.LogWarning("Hello, je suis un Fred_TestScript, ne pas m'oublier ici (" + gameObject.name + ")");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int table;
            int seat;
            bool success = sharedTables.TakeSeat(clients[nextClient], out table, out seat);
            print("Success: " + success + "  Table: " + table + "  Seat: " + seat);
            nextClient++;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Release: " + sharedTables.ReleaseSeats(clients[nextClient]));
            nextClient++;
        }
    }
}