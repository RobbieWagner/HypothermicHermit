using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum clickStateEnum
{
    disabled,
    enabled,
    hover,
    selected
}

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] protected int cursorState;

    private int clickState;
    public int ClickState
    {
        get
        {
            return clickState;
        }
        set
        {
            if(value == clickState) return;
            Debug.Log(value);
            clickState = value;
            OnChangeClickState(clickState);
        }
    }

    public delegate void OnChangeClickStateDelegate(int state);
    public event OnChangeClickStateDelegate OnChangeClickState = delegate {};

    protected virtual void OnPointerEnter()
    {
        CursorController.Instance.AddClickable(this);
    }

    protected virtual void OnPointerExit()
    {
        CursorController.Instance.RemoveClickable(this);
    }

    protected virtual void OnPointerDown()
    {
        if(CursorController.Instance.clickables.Count > 0 && CursorController.Instance.clickables[0] == this)
        {
            CursorController.Instance.SetSelectedClickable(this);
        }
    }

    public void UpdateClickableState()
    {
        if(CursorController.Instance.selectedClickable == null) CursorController.Instance.CursorState = cursorState;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData data)
    {
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
