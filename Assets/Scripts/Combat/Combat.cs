using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Defines a Combat encounter. Only one combat encounter is allowed in a scene
public class Combat : MonoBehaviour
{
    //Serialize Field for now, will change
    //public List<Enemy> combatEnemies;

    [SerializeField] Canvas canvas;

    [SerializeField] 
    private List<Enemy> enemies;
    private List<Ally> allies;

    public static Combat Instance {get; private set;}
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        canvas.worldCamera = Camera.main;

        CombatManager.Instance.OnEndCombat += EndCombat;
        BattleGridManager.Instance.OnBattleGridCreated += InitalizeCombat;
    }

    public void InitalizeCombat()
    {
        allies = CombatManager.Instance.characters.OfType<Ally>().ToList();
        enemies = CombatManager.Instance.characters.OfType<Enemy>().ToList();
    }

    public void CheckForCombatEnd()
    {
        Manager.Instance.GameState = (int) GameStateEnum.explore;
    }

    public void EndCombat()
    {

    }
}
