using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    #region VAR_DECLORATION

    [SerializeField] private Color selectColour;
    [SerializeField] private Color deselectColour;

    #endregion

    // Highlights the button when it is selected

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = selectColour;

    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = deselectColour;
    }
}
