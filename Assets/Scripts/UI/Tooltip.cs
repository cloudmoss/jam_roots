using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public static TooltipDefinition CurrentTooltip { get; private set; }
    public static Tooltip Current { get; private set; }

    [SerializeField] private Text _title;
    [SerializeField] private Text _body;

    private bool _isOpen;

    private void Awake() {
        Current = this;
    }

    public void OpenTooltip(TooltipDefinition tooltipDef)
    {
        CurrentTooltip = tooltipDef;
        _isOpen = true;

        if (_title.text != tooltipDef.title)
            _title.text = tooltipDef.title;

        if (_body.text != tooltipDef.body)
            _body.text = tooltipDef.body;

    }

    public void CloseTooltip()
    {
        CurrentTooltip = null;
        _isOpen = false;
    }


    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isOpen ? Vector3.one : Vector3.zero, Time.deltaTime * 30f);

        var mousePos = Input.mousePosition;
        transform.position = mousePos;

        if (transform.position.y < 120f)
            transform.position = new Vector3(transform.position.x, 120f, transform.position.z);
    }

}
