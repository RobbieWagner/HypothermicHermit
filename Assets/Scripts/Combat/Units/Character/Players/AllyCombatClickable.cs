using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class AllyCombatClickable : Clickable
{
    public Character unitComponent;
    private int movementSpentThisTurn;
    private int actionsSpentThisTurn;

    private void Awake() 
    {
        ClickState = (int) clickStateEnum.disabled;
        Manager.Instance.OnGameStateChange += ChangeMovementState;
        unitComponent.OnEndMoveUnit += SpendMovement;
        unitComponent.OnAct += SpendAction;

        movementSpentThisTurn = 0;

        unitComponent.OnCompleteAction += ResetClickable;
    }

    protected virtual void ChangeMovementState(int state)
    {
        if(state == (int) GameStateEnum.combat) ClickState = (int) clickStateEnum.enabled;
        else ClickState = (int) clickStateEnum.disabled;
    }

    protected virtual void ResetClickable()
    {
        //Disable grid clickables, reset for action/movement
    }

    private void SpendMovement(int spentMovement)
    {
        ClickState = (int) clickStateEnum.disabled;
        Combat.Instance.DisableTargetClickables();
        movementSpentThisTurn += spentMovement;
        if(movementSpentThisTurn == unitComponent.UnitSpeed) 
        {
            unitComponent.OutOfMovementThisTurn = true;
            if(!unitComponent.OutOfActionsThisTurn) ClickState = (int) clickStateEnum.enabled;
        }
        else ClickState = (int) clickStateEnum.enabled;
    }

    private void SpendAction(IUnit unit)
    {
        ClickState = (int) clickStateEnum.disabled;
        unitComponent.OutOfActionsThisTurn = true;
        if(!unitComponent.OutOfMovementThisTurn) ClickState = (int) clickStateEnum.enabled;
    }

    protected override void OnPointerEnter()
    {
        if(ClickState == (int) clickStateEnum.enabled) 
        {
            //Debug.Log("enabled");
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
            BattleGrid.Instance.EnableTileColliders(unitComponent.UnitSpeed - movementSpentThisTurn, new Vector2(unitComponent.tileXPos, unitComponent.tileYPos), unitComponent.tileXPos, unitComponent.tileYPos);
            if(!unitComponent.OutOfActionsThisTurn) Combat.Instance.EnableTargetClickables(unitComponent.unitActions[unitComponent.CurrentAction], unitComponent, unitComponent.UnitSpeed - movementSpentThisTurn);
            Combat.Instance.currentSelectedUnit = unitComponent;
            CombatManager.Instance.OnTakeAction += unitComponent.UseUnitAction;

            base.OnPointerDown();
        }
    }
}
