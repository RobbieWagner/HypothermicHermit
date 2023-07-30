using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnMenuItem : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    public void OnSelectMenuItem()
    {
        if(CombatManager.Instance.CombatPhase == (int) CombatPhaseEnum.ally 
           && CursorController.Instance.selectedClickable == null 
           && CombatManager.Instance.canEndTurn)
        {
            Debug.Log("ending turn");
            foreach(IUnit unit in Combat.Instance.combatUnits)
            {
                if(unit is Player || unit is Ally)
                {
                    unit.OutOfActionsThisTurn = true;
                    unit.OutOfMovementThisTurn = true;
                }
            }
            pauseMenu.OnPause();
        }
    }
}
