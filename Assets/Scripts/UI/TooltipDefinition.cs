using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipDefinition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string body;
    public ResourceClass[] resourceList;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Current.OpenTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Current.CloseTooltip();
    }

    private void OnDisable() {
        if (Tooltip.CurrentTooltip == this)
            Tooltip.Current.CloseTooltip();
    }

    private void OnDestroy()
    {
        if (Tooltip.CurrentTooltip == this)
            Tooltip.Current.CloseTooltip();
    }
}
