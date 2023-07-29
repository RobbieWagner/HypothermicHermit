using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetClickable : Clickable
{
    [SerializeField] IUnit unitComponent;
    private bool mouseOver;
    private ActionInformation actionInfo;
    [SerializeField] private UnitHover unitHover;

    private void Awake() 
    {
        actionInfo = null;  
    }

    private void OnEnable() 
    {
        unitHover.enabled = false;
        unitHover.gameObject.SetActive(false);
    }

    private void OnDisable() 
    {
        unitHover.gameObject.SetActive(true);
        unitHover.enabled = true;

        OnDisableTarget();
    }

    public delegate void OnDisableTargetDelegate();
    public event OnDisableTargetDelegate OnDisableTarget = delegate {};

    protected override void OnPointerEnter()
    {
        base.OnPointerEnter();
        IUnit unit = Combat.Instance.GetActingUnit();
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
    }
}
