using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainView : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset ListEntryTemplate;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        // Initialize the character list controller
        var elementListController = new ElementListController();
        elementListController.InitializeElementList(uiDocument.rootVisualElement, ListEntryTemplate);
    }
}
