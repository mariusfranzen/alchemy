using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class EditorSettings
{
    public ElementGeneratorSettings ElementGeneratorSettings;
    public CombinationGeneratorSettings CombinationGeneratorSettings;
}

[Serializable]
public class ElementGeneratorSettings
{
    public string LastPath;
    public bool ElementDiscoveredDefault;
}

[Serializable]
public class CombinationGeneratorSettings
{
    public string LastCombinationsPath;
    public string LastElementsPath;
}
