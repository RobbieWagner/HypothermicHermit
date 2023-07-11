using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Defines an enemy to fight in Combats
public class Enemy : Unit
{
    public override void AddUnitToGrid()
    {
        Combat.Instance.combatEnemies.Add(this);
    }
}
