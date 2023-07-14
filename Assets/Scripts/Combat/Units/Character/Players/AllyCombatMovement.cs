using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public enum movementState
{
    disabled,
    enabled,
    hover,
    selected
}

public class AllyCombatMovement : Clickable
{
    private int unitState;

    private void Awake() 
    {
        unitState = (int) movementState.disabled;
        Manager.Instance.OnGameStateChange += ChangeMovementState;
    }

    protected virtual void ChangeMovementState(int state)
    {
        if(state == (int) GameStateEnum.combat) unitState = (int) movementState.enabled;
        else unitState = (int) movementState.disabled;
    }

    protected override void OnPointerEnter()
    {
        if(unitState == (int) movementState.enabled) 
        {
            base.OnPointerEnter();
            if(CursorController.Instance.clickables[0] == this)
            {
                unitState = (int) movementState.hover;
                CursorController.Instance.CursorState = (int) GameCursorState.hovering_state;
            }
        }
    }

    protected override void OnPointerExit()
    {
        if(unitState == (int) movementState.hover)
        {
            unitState = (int) movementState.enabled;
            base.OnPointerExit();
        }
    }

    protected override void OnPointerDown()
    {
        if(unitState == (int) movementState.hover)
        {
            unitState = (int) movementState.selected;
            CursorController.Instance.SetSelectedClickable(this);
            base.OnPointerDown();
        }
    }
}
