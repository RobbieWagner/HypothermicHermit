using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : ActionEffect
{
    public override void TakeAction(IUnit user, IUnit target)
    {
        user.OutOfMovementThisTurn = true;
    }

    public override void ConfigureEffectDisplayLine(IUnit user, IUnit target, ActionEffectDisplayLine displayLine)
    {
    }
}
