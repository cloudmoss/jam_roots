using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [System.Serializable]
    public class Definition
    {
        public string title;
        public string description;
        public System.Action onClick;
        public Sprite sprite;
        public ResourceClass[] resourceList;
    }

    public Definition definition;

    private Button _button;
    private TooltipDefinition _tooltip;


    public void Init()
    {
        _tooltip = gameObject.AddComponent<TooltipDefinition>();
        _tooltip.title = definition.title;
        _tooltip.body = definition.description;
        _tooltip.resourceList = definition.resourceList;

        var icon = transform.Find("Icon").GetComponent<Image>();
        icon.sprite = definition.sprite;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => definition.onClick?.Invoke());
    }
}
