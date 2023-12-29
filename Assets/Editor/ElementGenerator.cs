using System.Collections.Generic;
using System.IO;
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
        var selectedFile = "";
        var fileLabel = new Label("File: C:/");

        selectFile.clicked += () =>
        {
            var path = EditorUtility.OpenFilePanel("Select JSON file with elements", "", "json");
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

        generateFromJsonButton.clicked += () =>
        {
            if (!fileSelected)
            {
                //TODO
                Debug.LogError("File is not selected");
                return;
            }
            if (true)
            {
                //TODO: Check if element already exists
                Debug.LogWarning("Check if element already exists - NotImplemented");
            }
            if (true)
            {
                //TODO: Make sure sprite exists
                Debug.LogWarning("Make sure sprite exists - NotImplemented");
            }
            var json = File.ReadAllText(selectedFile);
            GetElementsFromJson(json);
        };

        fileSelectContainer.Add(fileLabel);
        fileSelectContainer.Add(selectFile);

        var nameInput = new TextField();
        nameInput.label = "Name: ";
        var descriptionInput = new TextField();
        descriptionInput.label = "Description: ";
        var addElementButton = new Button();
        addElementButton.text = "Add element";
        // TODO: Add file dialog for selecting sprite

        addElementButton.clicked += () =>
        {
            if (!fileSelected)
            {
                //TODO
                Debug.LogError("File is not selected");
                return;
            }
            if (true)
            {
                //TODO: Check if element already exists
                Debug.LogWarning("Check if element already exists - NotImplemented");
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

    public void AddElementToJson(string filePath, string name, string description)
    {
        var json = File.ReadAllText(filePath);
        Debug.Log(json);
        Debug.Log(name);
        Debug.Log(description);

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

    public List<GameObject> GetElementsFromJson(string json)
    {
        var elementModel = JsonUtility.FromJson<ElementModel>(json);
        foreach (var element in elementModel.elements)
        {
            var go = GetBaseGameObject();
            go.name = element.name;
            go.GetComponent<SpriteRenderer>().sprite = GetElementSprite(element.name);
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Elements";
            go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            go.GetComponent<ElementScript>().Name = element.name;
            go.GetComponent<ElementScript>().Discovered = element.discovered;
            
            CreatePrefab(go);
            DestroyImmediate(go);
        }

        return new List<GameObject>();
    }

    public GameObject GetBaseGameObject()
    {
        var baseGameObject = new GameObject();
        baseGameObject.AddComponent<SpriteRenderer>();
        baseGameObject.AddComponent<ElementScript>();

        return baseGameObject;
    }

    public Sprite GetElementSprite(string name)
    {
        return Resources.Load<Sprite>($"Icons/{name.ToLower()}");
    }

    public void CreatePrefab(GameObject go)
    {
        PrefabUtility.SaveAsPrefabAsset(go, $"Assets/Prefabs/Generated/{go.name}.prefab");
    }
}
