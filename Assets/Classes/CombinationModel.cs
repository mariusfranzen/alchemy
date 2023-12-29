using System;

[Serializable]
public class CombinationModel
{
    public InnerCombinationModel[] combinations;

    [Serializable]
    public class InnerCombinationModel
    {
        public string element1;
        public string element2;
        public string result;
    }
}
