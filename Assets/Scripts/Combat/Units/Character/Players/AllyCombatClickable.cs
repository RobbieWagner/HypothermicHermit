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
        BattleGrid.Instance.OnBattleGridCreated += Enable;
        unitComponent.OnEndMoveUnit += SpendMovement;
        unitComponent.OnAct += SpendAction;

        movementSpentThisTurn = 0;

        unitComponent.OnCompleteAction += ResetClickable;
        unitComponent.OnStartUnitsTurn += RegainSpentResources;
        unitComponent.OnActionChange += DisplayActionTiles;
    }

    protected virtual void Enable()
    {
        ClickState = (int) clickStateEnum.enabled;
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
        CombatManager.Instance.OnTakeAction -= unitComponent.UseUnitAction;
    }

    private void SpendAction(IUnit unit)
    {
        //Debug.Log("action spent");
        ClickState = (int) clickStateEnum.disabled;
        unitComponent.OutOfActionsThisTurn = true;
        if(!unitComponent.OutOfMovementThisTurn) ClickState = (int) clickStateEnum.enabled;
        CombatManager.Instance.OnTakeAction -= unitComponent.UseUnitAction;
    }

    private void RegainSpentResources()
    {
        movementSpentThisTurn = 0;
        actionsSpentThisTurn = 0;
        unitComponent.OutOfActionsThisTurn = false;
        unitComponent.OutOfMovementThisTurn = false;
    }

    protected override void OnPointerEnter()
    {
        if(ClickState == (int) clickStateEnum.enabled && !unitComponent.IsDead) 
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
        if(ClickState == (int) clickStateEnum.hover && !unitComponent.IsDead)
        {
            ClickState = (int) clickStateEnum.selected;
            CursorController.Instance.SetSelectedClickable(this);
            
            DisplayActionTiles();

            Combat.Instance.currentSelectedUnit = unitComponent;
            CombatManager.Instance.OnTakeAction += unitComponent.UseUnitAction;
            CombatCameraMovement.Instance.MoveCamera(transform.position);

            base.OnPointerDown();
        }
    }

    protected void DisplayActionTiles()
    {
        if(CursorController.Instance.selectedClickable == this)
        {
            BattleGrid.Instance.DisableAllTileColliders();
            Combat.Instance.DisableTargetClickables();
            CombatHUD.Instance.RemoveAllActionInformation();
            BattleGrid.Instance.EnableTileColliders(unitComponent.UnitSpeed - movementSpentThisTurn, new Vector2(unitComponent.tileXPos, unitComponent.tileYPos), unitComponent.tileXPos, unitComponent.tileYPos);
            if(!unitComponent.OutOfActionsThisTurn) 
            {
                Combat.Instance.EnableTargetClickables(unitComponent.unitActions[unitComponent.CurrentAction], unitComponent, unitComponent.UnitSpeed - movementSpentThisTurn);
            }
        }
    }
}
