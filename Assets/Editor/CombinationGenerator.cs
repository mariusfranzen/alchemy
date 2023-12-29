using System.Collections.Generic;
using System.IO;
using Ionic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static CombinationModel;

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
        bool fileSelected = false;

        var fileSelectContainer = new VisualElement();
        var selectFile = new Button
        {
            text = "Select file"
        };
        var selectedFile = "";
        var fileLabel = new Label("File: C:/");

        selectFile.clicked += () =>
        {
            var path = EditorUtility.OpenFilePanel("Select JSON file with combinations", "", "json");
            if (path.Length != 0)
            {
                selectedFile = path;
                fileLabel.text = "File: " + path;
                fileSelected = true;
            }
            else
            {
                selectedFile = "";
                fileLabel.text = "File: ";
                fileSelected = false;
            }
        };

        fileSelectContainer.Add(fileLabel);
        fileSelectContainer.Add(selectFile);

        var element1 = new TextField();
        element1.label = "Element 1: ";
        var element2 = new TextField();
        element2.label = "Element 2: ";
        var resultElement = new TextField();
        resultElement.label = "Result element: ";
        var addCombination = new Button();
        addCombination.text = "Add combination";

        addCombination.clicked += () =>
        {
            if (!fileSelected)
            {
                //TODO
                Debug.LogError("File is not selected");
                return;
            }
            if (true)
            {
                //TODO: Make sure elements actually exists
                Debug.LogWarning("Make sure elements actually exists - NotImplemented");
            }
            if (true)
            {
                //TODO: Check if combination already exists
                Debug.LogWarning("Check if combination already exists - NotImplemented");
            }
            var json = File.ReadAllText(selectedFile);
            var combinations = JsonUtility.FromJson<CombinationModel>(json);
            var combinationList = new List<InnerCombinationModel>(combinations.combinations);
            var newCombination = new InnerCombinationModel
            {
                element1 = element1.text,
                element2 = element2.text,
                result = resultElement.text
            };

            combinationList.Add(newCombination);
            combinations.combinations = combinationList.ToArray();
            File.WriteAllText(selectedFile, JsonUtility.ToJson(combinations));
        };

        rootVisualElement.Add(new Label("This tool is used for generating the element combinations"));
        rootVisualElement.Add(fileSelectContainer);
        rootVisualElement.Add(new Label("----------"));
        rootVisualElement.Add(element1);
        rootVisualElement.Add(element2);
        rootVisualElement.Add(resultElement);
        rootVisualElement.Add(addCombination);
    }
}
