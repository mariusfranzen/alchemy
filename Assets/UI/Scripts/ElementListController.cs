using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ElementListController
{
    VisualTreeAsset ListEntryTemplate;

    ListView ElementList;

    List<ElementUiData> UiAllDiscoveredElements;

    public void InitializeElementList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllElements();

        ListEntryTemplate = listElementTemplate;
        ElementList = root.Q<ListView>("element-list");

        FillElementList();
    }

    void EnumerateAllElements()
    {
        UiAllDiscoveredElements = new List<ElementUiData>();
        UiAllDiscoveredElements.AddRange(Resources.LoadAll<ElementUiData>("UiElementData"));
    }

    void FillElementList()
    {
        ElementList.makeItem = () =>
        {
            var newListEntry = ListEntryTemplate.Instantiate();
            var newListEntryLogic = new ElementListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };

        ElementList.bindItem = (item, index) =>
        {
            (item.userData as ElementListEntryController).SetElementUiData(UiAllDiscoveredElements[index]);
            item.RegisterCallback<MouseDownEvent, string>(TestCallback, UiAllDiscoveredElements[index].ElementName);
        };

        ElementList.fixedItemHeight = 70;
        ElementList.itemsSource = UiAllDiscoveredElements;
        ElementList.selectionType = SelectionType.None;
    }

    private void TestCallback(MouseDownEvent evt, string name)
    {
        Debug.Log(name);
        evt.StopPropagation();
        var item = Game.GetMasterScript().SpawnElementAtMouse(name);
    }
}
