using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
