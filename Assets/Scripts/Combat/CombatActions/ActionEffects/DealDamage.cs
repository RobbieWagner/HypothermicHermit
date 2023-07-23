using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : ActionEffect
{
    [SerializeField] private int damage;

    public override void TakeAction(IUnit user)
    {
        Debug.Log("damage");
        base.TakeAction(user);
    }
}
