using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : MonoBehaviour
{
    [SerializeField] float _travelSpeed;

    public float TravelSpeed { get { return _travelSpeed; } set { _travelSpeed = value; } }
    public float ArriveThreshold { get; set; }

    private Action _onComplete;
    private bool _traveling = false;
    private Vector3 _destination;
    private Transform _tr;

    void Awake()
    {
        _tr = transform;
        ArriveThreshold = 0.005f;
    }

    public void TravelTo(Vector3 destination, Action onComplete = null)
    {
        _destination = destination;
        _onComplete = onComplete;
        _traveling = true;
    }

    void Update()
    {
        if (_traveling)
        {
            _tr.position = Vector3.MoveTowards(_tr.position, _destination, _travelSpeed * Time.deltaTime);
            if((_tr.position - _destination).sqrMagnitude <= ArriveThreshold * ArriveThreshold)
            {
                _traveling = false;
                if (_onComplete != null)
                {
                    _onComplete.Invoke();
                    _onComplete = null;
                }
            }
        }
    }
}
