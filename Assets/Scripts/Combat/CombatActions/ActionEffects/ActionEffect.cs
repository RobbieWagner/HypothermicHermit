using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionEffect
{
    public virtual void TakeAction(IUnit user)
    {
    }
}
