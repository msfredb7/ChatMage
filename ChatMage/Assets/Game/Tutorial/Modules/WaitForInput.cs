using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForInput : MonoBehaviour
{
    private struct Order
    {
        public KeyCode[] keyCodes;
        public string[] buttons;
        public Action callback;
    }

    private List<Order> orders = new List<Order>();

    public void OnKeyDown(Action callback, params string[] buttons)
    {
        orders.Add(new Order() { keyCodes = null, buttons = buttons, callback = callback });
    }
    public void OnKeyDown(Action callback, params KeyCode[] keyCodes)
    {
        orders.Add(new Order() { keyCodes = keyCodes, buttons = null, callback = callback });
    }
    public void OnAnyKeyDown(Action callback)
    {
        orders.Add(new Order() { keyCodes = null, buttons = null, callback = callback });
    }

    void Update()
    {
        int count = orders.Count;
        for (int i = 0; i < count; i++)
        {
            if (orders[i].keyCodes != null)
            {
                for (int u = 0; u < orders[i].keyCodes.Length; u++)
                {
                    if (Input.GetKeyDown(orders[i].keyCodes[u]))
                    {
                        orders[i].callback();
                        orders.RemoveAt(i);
                        i--;
                        count--;
                        break;
                    }
                }
            }
            else if(orders[i].buttons != null)
            {
                for (int u = 0; u < orders[i].buttons.Length; u++)
                {
                    if (Input.GetButtonDown(orders[i].buttons[u]))
                    {
                        orders[i].callback();
                        orders.RemoveAt(i);
                        i--;
                        count--;
                        break;
                    }
                }
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    orders[i].callback();
                    orders.RemoveAt(i);
                    i--;
                    count--;
                }
            }
        }
    }
}
