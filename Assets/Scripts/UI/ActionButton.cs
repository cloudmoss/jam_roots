using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public UnityEvent OnClick;

    [SerializeField] private string _title;
    [SerializeField] private string _description;

    private Button _button;
    private TooltipDefinition _tooltip;


    void Awake()
    {
        _tooltip = gameObject.AddComponent<TooltipDefinition>();
        _tooltip.title = _title;
        _tooltip.body = _description;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => OnClick.Invoke());
    }
}
