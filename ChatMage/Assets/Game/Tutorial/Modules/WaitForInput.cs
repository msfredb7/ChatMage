using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForInput : MonoBehaviour
{
    private struct Order
    {
        public KeyCode[] keyCodes;
        public Action callback;
    }

    private List<Order> orders = new List<Order>();

    public void OnKeyDown(Action callback, params KeyCode[] keyCodes)
    {
        orders.Add(new Order() { keyCodes = keyCodes, callback = callback });
    }
    public void OnAnyKeyDown(Action callback)
    {
        orders.Add(new Order() { keyCodes = null, callback = callback });
    }

    void Update()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if(orders[i].keyCodes == null)
            {
                if (Input.anyKeyDown)
                {
                    orders[i].callback();
                    orders.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                for (int u = 0; u < orders[i].keyCodes.Length; u++)
                {
                    if (Input.GetKeyDown(orders[i].keyCodes[u]))
                    {
                        orders[i].callback();
                        orders.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }
    }
}
