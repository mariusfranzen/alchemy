using System;
using UnityEngine;

[Serializable]
public class CombinationModel
{
    public InnerCombinationModel[] combinations;

    [Serializable]
    public class InnerCombinationModel
    {
        public string result;
        public string element1;
        public string element2;
    }
}
