using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetClickable : Clickable
{
    [SerializeField] IUnit unitComponent;
    private bool mouseOver;
    private ActionInformation actionInfo;

    private void Awake() 
    {
        actionInfo = null;    
    }

    protected override void OnPointerEnter()
    {
        base.OnPointerEnter();
        IUnit unit = GetActingUnit();
        if(unit != null) actionInfo = CombatHUD.Instance.AddActionInformation(unit, unitComponent);
    }

    //Add functionality for AOE bubble trigger enter to also call mouse over

    protected override void OnPointerExit()
    {
        base.OnPointerExit();
        if(actionInfo != null) actionInfo = CombatHUD.Instance.RemoveActionInformation(actionInfo);
    }

    protected override void OnPointerDown()
    {
        CombatManager.Instance.TryTakeAction(Combat.Instance.currentSelectedUnit, unitComponent);
        base.OnPointerDown();
        CursorController.Instance.RemoveClickable(this);
        if(actionInfo != null) CombatHUD.Instance.RemoveActionInformation(actionInfo);
        gameObject.SetActive(false);
    }

    private IUnit GetActingUnit()
    {
        Clickable actingClickable = CursorController.Instance.selectedClickable;
        AllyCombatClickable actingAlly = null;
        if(actingClickable.GetType().Equals(typeof(AllyCombatClickable))) actingAlly = (AllyCombatClickable) actingClickable;
        if(actingAlly != null) return actingAlly.unitComponent;
        Debug.Log("oopsie");
        return null;
    }
}
