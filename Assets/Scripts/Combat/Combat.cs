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

    public void EnableTargetClickables(CombatAction action, Character user, int unitMovementLeft)
    {
        //Add if statements to check for action type once more are added (Like AOE)
        TileGrid combatGrid = BattleGrid.Instance.tileGrid;
        PathFinder pathFinder = BattleGrid.Instance.pathFinder;
        List<IUnit> targetUnits = new List<IUnit>();
        
        if(action.targetsOpponents) targetUnits.AddRange(enemies);
        else
        {
            targetUnits.Add(Player.Instance);
            targetUnits.AddRange(allies);
        }

        foreach(IUnit unit in targetUnits)
        {

            if(pathFinder.CalculateDistance(combatGrid.grid[unit.tileXPos, unit.tileYPos], combatGrid.grid[user.tileXPos, user.tileYPos]) <= action.range) 
            {
                unit.targetClickable.gameObject.SetActive(true);
            }
            else
            {
                List<Node> currentPath = null;
                List<Node> neighbors = pathFinder.GetNeighbors(combatGrid.grid[unit.tileXPos, unit.tileYPos]);
                // Debug.Log(neighbors.Count);

                foreach(Node node in neighbors)
                {
                    List<Node> path = pathFinder.FindPath(user.tileXPos, user.tileYPos, node.x, node.y);
                    if(currentPath == null || currentPath.Count > path.Count) 
                    {
                        //Debug.Log("new path found for " + unit.gameObject.name);
                        currentPath = path;
                    }
                }

                if(currentPath != null && currentPath.Count < (action.range + unitMovementLeft)) 
                {
                    //Debug.Log(unit.gameObject.name + currentPath.Count);
                    unit.targetClickable.gameObject.SetActive(true);
                }
            }
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
