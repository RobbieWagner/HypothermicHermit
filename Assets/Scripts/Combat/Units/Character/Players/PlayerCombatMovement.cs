using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Awake() 
    {
        Manager.Instance.OnGameStateChange += ToggleCombatAnimation;
    }

    private void ToggleCombatAnimation(int state)
    {
        if(state == (int) GameStateEnum.combat)
        {
            animator.SetBool("inCombat", true);
        }
        else animator.SetBool("inCombat", false);
    }
}
