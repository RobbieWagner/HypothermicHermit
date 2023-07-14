using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    [SerializeField] protected int clickableState;

    protected virtual void OnPointerEnter()
    {
        CursorController.Instance.AddClickable(this);
    }

    protected virtual void OnPointerExit()
    {
        CursorController.Instance.RemoveClickable(this);
        Debug.Log("hi");
    }

    protected virtual void OnPointerDown()
    {
        if(CursorController.Instance.clickables.Count > 0 && CursorController.Instance.clickables[0] == this)
        {
            //Debug.Log("hi");
        }
    }

    public void UpdateClickableState()
    {
        CursorController.Instance.CursorState = clickableState;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData data)
    {
        Debug.Log("clickable");
        OnPointerEnter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData data)
    {
        OnPointerExit();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData data)
    {
        OnPointerDown();
    }
}
