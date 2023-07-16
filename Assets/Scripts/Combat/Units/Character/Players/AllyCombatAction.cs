using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class AllyCombatAction : Clickable
{
    public IUnit unitComponent;
    public bool outOfMovement;
    private int movementSpentThisTurn;

    private void Awake() 
    {
        ClickState = (int) clickStateEnum.disabled;
        Manager.Instance.OnGameStateChange += ChangeMovementState;
        unitComponent.OnEndMoveUnit += SpendMovement;

        movementSpentThisTurn = 0;
    }

    protected virtual void ChangeMovementState(int state)
    {
        if(state == (int) GameStateEnum.combat) ClickState = (int) clickStateEnum.enabled;
        else ClickState = (int) clickStateEnum.disabled;
    }

    private void SpendMovement(int spentMovement)
    {
        movementSpentThisTurn += spentMovement;
        if(movementSpentThisTurn == unitComponent.UnitSpeed) outOfMovement = true;
        else ClickState = (int) clickStateEnum.enabled;
    }

    protected override void OnPointerEnter()
    {
        if(ClickState == (int) clickStateEnum.enabled) 
        {
            base.OnPointerEnter();
            if(CursorController.Instance.clickables[0] == this)
            {
                ClickState = (int) clickStateEnum.hover;
                CursorController.Instance.CursorState = (int) GameCursorState.hovering_state;
            }
        }
    }

    protected override void OnPointerExit()
    {
        if(ClickState == (int) clickStateEnum.hover)
        {
            ClickState = (int) clickStateEnum.enabled;
        }
        base.OnPointerExit();
    }

    protected override void OnPointerDown()
    {
        if(ClickState == (int) clickStateEnum.hover)
        {
            ClickState = (int) clickStateEnum.selected;
            CursorController.Instance.SetSelectedClickable(this);
            BattleGrid.Instance.DisableAllTileColliders();
            BattleGrid.Instance.EnableTileColliders(unitComponent.UnitSpeed - movementSpentThisTurn, new Vector2(unitComponent.tileXPos, unitComponent.tileYPos));
            base.OnPointerDown();
        }
    }
}
