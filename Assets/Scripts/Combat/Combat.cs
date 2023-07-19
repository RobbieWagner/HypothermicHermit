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

    [HideInInspector] public Character currentSelectedUnit;

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
        CombatManager.Instance.OnPhaseChange += StartNextCombatPhase;
        BattleGrid.Instance.OnBattleGridCreated += InitalizeCombat;
    }

    public void InitalizeCombat()
    {
        allies = CombatManager.Instance.characters.OfType<Ally>().ToList();
        enemies = CombatManager.Instance.characters.OfType<Enemy>().ToList();

        CombatManager.Instance.CombatPhase = (int) CombatPhaseEnum.ally;
    }

    private void StartNextCombatPhase(int phase)
    {
        if(phase == (int) CombatPhaseEnum.ally) 
        {
            foreach(Ally ally in allies) ally.StartUnitsTurn();
            Player.Instance.StartUnitsTurn();
        }
    }

    public void CheckForCombatEnd()
    {
        Manager.Instance.GameState = (int) GameStateEnum.explore;
    }

    public void EnableTargetClickables(bool targetsOpponents, int range, Character user)
    {
        List<IUnit> targetUnits = new List<IUnit>();
        if(targetsOpponents) targetUnits.AddRange(enemies);
        else
        {
            targetUnits.AddRange(allies);
            targetUnits.Add(Player.Instance);
        }

        foreach(IUnit unit in targetUnits)
        {
            if(unit.CalculateDistanceFromUnit(user) <= range) unit.targetClickable.gameObject.SetActive(true);
        }
    }

    public void DisableTargetClickables()
    {
        if(allies != null) foreach(IUnit unit in allies){unit.targetClickable.gameObject.SetActive(false);}
        if(enemies != null) foreach(IUnit unit in enemies){unit.targetClickable.gameObject.SetActive(false);}
        Player.Instance.targetClickable.gameObject.SetActive(false);
    }

    public void EndCombat()
    {

    }
}
