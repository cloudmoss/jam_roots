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

        _body.text = string.Empty;

        if (tooltipDef.resourceList != null)
        {
            _body.text += "Cost:\n";
            foreach (var resource in tooltipDef.resourceList)
            {
                var resourceText = string.Format("{0}: {1}\n", resource.Name, resource.Amount.ToString("F0"));
                _body.text += resourceText;
            }
            _body.text += "\n";
            _body.text += "\n";
        }

        if (_body.text != tooltipDef.body)
            _body.text += tooltipDef.body;

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

        if (transform.position.y < 160f)
            transform.position = new Vector3(transform.position.x, 160f, transform.position.z);
    }

}
