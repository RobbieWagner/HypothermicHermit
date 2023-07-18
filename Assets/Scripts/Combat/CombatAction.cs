using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat Action")]
public class CombatAction : ScriptableObject
{
    //Defines an action a unit can take during combat. Will create a SerializedReference for this one
    [SerializeReference] public List<ActionEffect> effects;

    [ContextMenu(nameof(DealDamage))] void AddDamageEffect(){effects.Add(new DealDamage());}
    [ContextMenu(nameof(Heal))] void AddHealEffect(){effects.Add(new Heal());}
    [ContextMenu(nameof(StatChange))] void AddStatChangeEffect(){effects.Add(new StatChange());}
}
