using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static CombinationModel;
using static ElementModel;

public class CombinationGenerator : EditorWindow
{
    [MenuItem("Tools/Combination Generator")]
    public static void ShowCombinationGenerator()
    {
        EditorWindow window = GetWindow<CombinationGenerator>();
        window.titleContent = new GUIContent("Alchemy Combination Generator");
    }

    public void CreateGUI()
    {
        bool combinationFileSelected = false;
        bool elementsFileSelected = false;

        // Combinations select

        var combinationsSelectContainer = new VisualElement();
        var selectCombinationFile = new Button
        {
            text = "Select combinations file"
        };
        var selectedCombinationsFile = GetLastCombinationsPath();
        var combinationsFileLabel = new Label($"Combinations file: {selectedCombinationsFile}");

        if (selectedCombinationsFile != string.Empty)
        {
            combinationFileSelected = true;
        }

        selectCombinationFile.clicked += () =>
        {
            var path = EditorUtility.OpenFilePanel("Select JSON file with combinations", "", "json");
            if (path.Length != 0)
            {
                selectedCombinationsFile = path;
                combinationsFileLabel.text = "Combinations file: " + path;
                combinationFileSelected = true;
                SaveLastCombinationsPath(path);
            }
            else
            {
                selectedCombinationsFile = "";
                combinationsFileLabel.text = "Combinations file: ";
                combinationFileSelected = false;
            }
        };

        combinationsSelectContainer.Add(combinationsFileLabel);
        combinationsSelectContainer.Add(selectCombinationFile);

        //Elements select

        var elementsSelectContainer = new VisualElement();
        var selectElementsFile = new Button
        {
            text = "Select elements file"
        };
        var selectedElementsFile = GetLastElementsPath();
        var elementsFileLabel = new Label($"Elements file: {selectedElementsFile}");

        if (selectedElementsFile != string.Empty)
        {
            elementsFileSelected = true;
        }

        selectElementsFile.clicked += () =>
        {
            var path = EditorUtility.OpenFilePanel("Select JSON file with elements", "", "json");
            if (path.Length != 0)
            {
                selectedElementsFile = path;
                elementsFileLabel.text = "Elements file: " + path;
                elementsFileSelected = true;
                SaveLastElementsPath(path);
            }
            else
            {
                selectedElementsFile = "";
                elementsFileLabel.text = "Elements file: ";
                elementsFileSelected = false;
            }
        };

        elementsSelectContainer.Add(elementsFileLabel);
        elementsSelectContainer.Add(selectElementsFile);

        // end

        var element1 = new TextField
        {
            label = "Element 1: "
        };
        var element2 = new TextField
        {
            label = "Element 2: "
        };
        var resultElement = new TextField
        {
            label = "Result element: "
        };
        var addCombination = new Button
        {
            text = "Add combination"
        };

        addCombination.clicked += () =>
        {
            if (!combinationFileSelected || !elementsFileSelected)
            {
                EditorUtility.DisplayDialog("No file selected", "You need to select a file", "Ok");
                return;
            }
            if (!(ElementExists(element1.text, selectedElementsFile) && ElementExists(element2.text, selectedElementsFile) && ElementExists(resultElement.text, selectedElementsFile)))
            {
                EditorUtility.DisplayDialog("Element Missing", "One or more of the selected elements doesn't exist", "Ok");
                return;
            }
            if (CombinationExists(selectedCombinationsFile, element1.text, element2.text, resultElement.text))
            {
                EditorUtility.DisplayDialog("Combination exists", "The specified combination already exists", "Ok");
                return;
            }

            var json = File.ReadAllText(selectedCombinationsFile);
            var combinations = JsonUtility.FromJson<CombinationModel>(json);
            var combinationList = new List<InnerCombinationModel>(combinations.combinations);
            var newCombination = new InnerCombinationModel
            {
                element1 = element1.text.FirstCharToUpper(),
                element2 = element2.text.FirstCharToUpper(),
                result = resultElement.text.FirstCharToUpper()
            };

            combinationList.Add(newCombination);
            combinations.combinations = combinationList.ToArray();
            File.WriteAllText(selectedCombinationsFile, JsonUtility.ToJson(combinations));
        };

        rootVisualElement.Add(new Label("This tool is used for generating the element combinations"));
        rootVisualElement.Add(elementsSelectContainer);
        rootVisualElement.Add(combinationsSelectContainer);
        rootVisualElement.Add(new Label("----------"));
        rootVisualElement.Add(element1);
        rootVisualElement.Add(element2);
        rootVisualElement.Add(resultElement);
        rootVisualElement.Add(addCombination);
    }

    private string GetLastCombinationsPath()
    {
        return Utils.GetEditorSettings().CombinationGeneratorSettings.LastCombinationsPath ?? "";
    }

    private string GetLastElementsPath()
    {
        return Utils.GetEditorSettings().CombinationGeneratorSettings.LastElementsPath ?? "";
    }

    private void SaveLastCombinationsPath(string path)
    {
        var settings = Utils.GetEditorSettings();
        settings.CombinationGeneratorSettings.LastCombinationsPath = path;
        Utils.SaveEditorSettings(settings);
    }

    private void SaveLastElementsPath(string path)
    {
        var settings = Utils.GetEditorSettings();
        settings.CombinationGeneratorSettings.LastElementsPath = path;
        Utils.SaveEditorSettings(settings);
    }

    private bool ElementExists(string name, string path)
    {
        var elements = GetElementList(path);
        foreach (var element in elements)
        {
            if (element.name.ToLower() == name.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    private bool CombinationExists(string path, string element1, string element2, string result)
    {
        var combinations = GetCombinationList(path);

        foreach (var combination in combinations)
        {
            if (combination.element1.ToLower() == element1.ToLower())
            {
                if (combination.element2.ToLower() == element2.ToLower())
                {
                    if (combination.result.ToLower() == result.ToLower())
                    {
                        return true;
                    }
                }
            }
            else if (combination.element1.ToLower() == element2.ToLower())
            {
                if (combination.element2.ToLower() == element1.ToLower())
                {
                    if (combination.result.ToLower() == result.ToLower())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private List<InnerElementModel> GetElementList(string path)
    {
        return GetElementModel(path).elements.ToList();
    }

    private ElementModel GetElementModel(string path)
    {
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<ElementModel>(json);
    }

    private List<InnerCombinationModel> GetCombinationList(string path)
    {
        return GetCombinationModel(path).combinations.ToList();
    }

    private CombinationModel GetCombinationModel(string path)
    {
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<CombinationModel>(json);
    }
}
