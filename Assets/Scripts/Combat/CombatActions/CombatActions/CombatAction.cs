using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat Action")]
public class CombatAction : ScriptableObject
{
    [SerializeField] public bool targetsOpponents;
    [SerializeField] public int range;
    //[SerializeField] public bool AOE;
    [SerializeReference] public List<ActionEffect> effects;

    [ContextMenu(nameof(DealDamage))] void AddDamageEffect(){effects.Add(new DealDamage());}
    [ContextMenu(nameof(Heal))] void AddHealEffect(){effects.Add(new Heal());}
    [ContextMenu(nameof(StatChange))] void AddStatChangeEffect(){effects.Add(new StatChange());}

    public virtual void Act(IUnit user, IUnit target)
    {
        if(!user.OutOfActionsThisTurn)
        {
            foreach(ActionEffect effect in effects)
            {
                effect.TakeAction(user);
            }
            user.OutOfActionsThisTurn = true;
        }
    }
}
