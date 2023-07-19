using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClickable : Clickable
{
    [SerializeField] IUnit unitComponent;
    protected override void OnPointerDown()
    {
        CombatManager.Instance.TryTakeAction(Combat.Instance.currentSelectedUnit, unitComponent);
        base.OnPointerDown();
        CursorController.Instance.RemoveClickable(this);
        gameObject.SetActive(false);
    }
}
