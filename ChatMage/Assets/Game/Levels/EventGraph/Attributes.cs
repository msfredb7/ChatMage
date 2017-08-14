using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public class MenuItem : Attribute
    {
        public string menuItem;
        public MenuItem(string menuItem) { this.menuItem = menuItem; }
    }
    public class DefaultNodeName : Attribute
    {
        public string nodeName;
        public DefaultNodeName(string nodeName) { this.nodeName = nodeName; }
    }
}
