using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class AllyCombatMovement : Clickable
{

    private bool canSelectUnit;

    private void Awake() 
    {
        canSelectUnit = false;
    }

    protected override void OnPointerEnter()
    {
        if(Manager.Instance.GameState == (int) GameStateEnum.combat) 
        {
            base.OnPointerEnter();
            if(CursorController.Instance.clickables[0] == this)
            {
                canSelectUnit = true;
                CursorController.Instance.CursorState = (int) GameCursorState.hovering_state;
            }
        }
    }

    protected override void OnPointerExit()
    {

    }
}
