using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : ActionEffect
{
    [SerializeField] private int damage;

    public override void TakeAction(IUnit user, IUnit target)
    {
        //Debug.Log("damage");
        target.Health -= damage;
        base.TakeAction(user, target);
    }

    public override void ConfigureEffectDisplayLine(IUnit user, IUnit target, ActionEffectDisplayLine displayLine)
    {
        displayLine.beforeText.text = target.Health.ToString();
        displayLine.afterText.text = (target.Health - damage).ToString();
    }
}
