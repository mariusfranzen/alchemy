using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ElementListEntryController
{
    Label NameLabel;
    VisualElement Sprite;

    public void SetVisualElement(VisualElement visualElement)
    {
        NameLabel = visualElement.Q<Label>("element-name");
        Sprite = visualElement.Q<VisualElement>("element-icon");
    }

    public void SetElementUiData(ElementUiData elementUiData)
    {
        NameLabel.text = elementUiData.ElementName;
        Sprite.style.backgroundImage = elementUiData.ElementSprite.texture;
    }
}
