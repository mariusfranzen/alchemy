using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementModel
{
    public InnerElementModel[] elements;

    [Serializable]
    public class InnerElementModel
    {
        public string name;
        public string description;
        public bool discovered;
    }
}
