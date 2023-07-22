using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    public Node node;

    protected virtual void OnPointerEnter()
    {
        //Debug.Log("enter");
    }

    protected virtual void OnPointerExit()
    {
        
    }

    protected virtual void OnPointerDown()
    {
        //Test.Instance.FindPath(this);
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
