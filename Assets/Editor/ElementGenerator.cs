using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static ElementModel;

public class ElementGenerator : EditorWindow
{
    [MenuItem("Tools/Element Generator")]
    public static void ShowElementGenerator()
    {
        EditorWindow window = GetWindow<ElementGenerator>();
        window.titleContent = new GUIContent("Alchemy Element Generator");
    }

    public void CreateGUI()
    {
        bool fileSelected = false;
        var generateFromJsonButton = new Button
        {
            text = "Generate from JSON"
        };

        var fileSelectContainer = new VisualElement();
        var selectFile = new Button
        {
            text = "Select file"
        };
        var selectedFile = GetLastPath();
        var fileLabel = new Label($"File: {selectedFile}");

        if (selectedFile != string.Empty)
        {
            fileSelected = true;
        }

        selectFile.clicked += () =>
        {
            var path = EditorUtility.OpenFilePanel("Select JSON file with elements", "", "json");
            if (path.Length != 0)
            {
                selectedFile = path;
                fileLabel.text = "File: " + path;
                fileSelected = true;
                SaveLastPath(path);
            } 
            else
            {
                selectedFile = "";
                fileLabel.text = "File: ";
                fileSelected = false;
            }
        };

        generateFromJsonButton.clicked += () =>
        {
            if (!fileSelected)
            {
                EditorUtility.DisplayDialog("No file selected", "You need to select a file", "Ok");
                return;
            }
            if (true)
            {
                //TODO: Make sure sprite exists
                Debug.LogWarning("Make sure sprite exists - NotImplemented");
            }
            var json = File.ReadAllText(selectedFile);
            CreatePrefabsFromJson(json);
        };

        fileSelectContainer.Add(fileLabel);
        fileSelectContainer.Add(selectFile);

        var nameInput = new TextField
        {
            label = "Name: "
        };
        var descriptionInput = new TextField
        {
            label = "Description: "
        };
        var addElementButton = new Button
        {
            text = "Add element"
        };
        // TODO: Add file dialog for selecting sprite

        addElementButton.clicked += () =>
        {
            if (!fileSelected)
            {
                EditorUtility.DisplayDialog("No file selected", "You need to select a file", "Ok");
                return;
            }
            if (ElementExists(nameInput.text, selectedFile))
            {
                EditorUtility.DisplayDialog("Element exists", "An element with that name already exists", "Ok");
                return;
            }
            AddElementToJson(selectedFile, nameInput.text, descriptionInput.text);
        };

        rootVisualElement.Add(new Label("This tool is used for adding elements, or generating element prefabs from the element.json file"));
        rootVisualElement.Add(fileSelectContainer);
        rootVisualElement.Add(new Label("----------"));
        rootVisualElement.Add(nameInput);
        rootVisualElement.Add(descriptionInput);
        rootVisualElement.Add(addElementButton);
        rootVisualElement.Add(new Label("----------"));
        rootVisualElement.Add(generateFromJsonButton);
    }

    private void AddElementToJson(string filePath, string name, string description)
    {
        var json = File.ReadAllText(filePath);
        var elementModel = JsonUtility.FromJson<ElementModel>(json);
        var elementList = new List<InnerElementModel>(elementModel.elements);
        var newElement = new InnerElementModel
        {
            name = name,
            description = description,
            discovered = false
        };

        elementList.Add(newElement);
        elementModel.elements = elementList.ToArray();

        var newJson = JsonUtility.ToJson(elementModel);
        File.WriteAllText(filePath, newJson);
    }

    private List<GameObject> CreatePrefabsFromJson(string json)
    {
        var elementModel = JsonUtility.FromJson<ElementModel>(json);
        var prefabCount = elementModel.elements.Length;
        var currentPrefab = 0;
        float prefabPercentage = 1f / prefabCount;
        foreach (var element in elementModel.elements)
        {
            EditorUtility.DisplayProgressBar("Generating prefabs", $"Prefab {currentPrefab}/{prefabCount}", prefabPercentage * currentPrefab);

            var go = GetBaseGameObject();
            var goCircle = GetCirclePrefab();
            go.name = element.name;
            go.GetComponent<SpriteRenderer>().sprite = GetElementSprite(element.name);
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Elements";
            go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            go.GetComponent<ElementScript>().Name = element.name;
            go.GetComponent<ElementScript>().Discovered = element.discovered;
            go.AddComponent<BoxCollider2D>();
            Instantiate(goCircle, go.transform);
            CreatePrefab(go);
            DestroyImmediate(go);
            currentPrefab++;
        }
        EditorUtility.ClearProgressBar();

        return new List<GameObject>();
    }

    private GameObject GetCirclePrefab()
    {
        var circle = Resources.Load<GameObject>("Prefabs/SelectionCircle");
        circle.transform.localScale = Vector3.zero;

        return circle;
    }

    private GameObject GetBaseGameObject()
    {
        var baseGameObject = new GameObject();
        baseGameObject.AddComponent<SpriteRenderer>();
        baseGameObject.AddComponent<ElementScript>();

        return baseGameObject;
    }

    private Sprite GetElementSprite(string name)
    {
        return Resources.Load<Sprite>($"Icons/{name.ToLower()}");
    }

    private void CreatePrefab(GameObject go)
    {
        PrefabUtility.SaveAsPrefabAsset(go, $"Assets/Prefabs/Generated/{go.name}.prefab");
    }

    private string GetLastPath()
    {
        return Utils.GetEditorSettings().ElementGeneratorSettings.LastPath ?? "";
    }

    private void SaveLastPath(string path)
    {
        var settings = Utils.GetEditorSettings();
        settings.ElementGeneratorSettings.LastPath = path;
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

    private List<InnerElementModel> GetElementList(string path)
    {
        return GetElementModel(path).elements.ToList();
    }

    private ElementModel GetElementModel(string path)
    {
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<ElementModel>(json);
    }
}
