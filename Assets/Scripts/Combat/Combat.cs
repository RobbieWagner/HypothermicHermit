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

    //[SerializeField] 
    public List<Character> combatUnits;
    private List<Enemy> enemies;
    private List<Character> allies;
    private List<IUnit> currentTurnsUnits;

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
        currentTurnsUnits = new List<IUnit>();

        allies = CombatManager.Instance.characters.OfType<Ally>().ToList<Character>();
        allies.Add(Player.Instance);
        enemies = CombatManager.Instance.characters.OfType<Enemy>().ToList();
        combatUnits = CombatManager.Instance.characters;

        foreach(Character character in combatUnits) 
        {
            character.OnHealthZero += KillUnit;
            character.unitsExploreCollider.enabled = false;
        }

        CombatManager.Instance.CombatPhase = (int) CombatPhaseEnum.ally;

        OnInitializeCombat();
    }

    public delegate void OnInitializeCombatDelegate();
    public event OnInitializeCombatDelegate OnInitializeCombat = delegate {};

    private void StartNextCombatPhase(int phase)
    {
        if(phase == (int) CombatPhaseEnum.ally) 
        {
            StartCoroutine(StartNewRound());
            foreach(Character ally in allies) 
            {
                EnableCharacterUse(ally);
            }
            CombatCameraMovement.Instance.canMove = true;
        }

        else if(phase == (int) CombatPhaseEnum.enemy)
        {
            StartCoroutine(EnemyPhaseCo());
        }
    }

    private IEnumerator StartNewRound()
    {
        Manager.Instance.canPause = false;
        RemoveDeadUnits();
        CombatManager.Instance.canEndTurn = false;

        yield return StartCoroutine(CombatCameraMovement.Instance.MoveCameraCo(Player.Instance.transform.position));

        Manager.Instance.canPause = true;
        CombatManager.Instance.canEndTurn = true;
        Manager.Instance.canPause = true;
        StopCoroutine(StartNewRound());
    }

    private void RemoveDeadUnits()
    {
        enemies.RemoveAll(enemy => enemy.IsDead);
        allies.RemoveAll(ally => ally.IsDead);

        foreach(Character character in combatUnits)
        {
            if(!enemies.Contains(character) && !allies.Contains(character))
            {
                character.IsInCombat = false;
            }
        }
    }

    private void CompleteEnemiesPhase()
    {
        currentTurnsUnits.Clear();
        CombatManager.Instance.NextCombatPhase();
    }

    private void CompleteUnitsTurn(IUnit unit)
    {
        currentTurnsUnits.Remove(unit);
        CheckForCombatEnd();
        if(unit is Character)
        {
            Character character =(Character) unit;
            character.actionClickable.ClickState = (int) clickStateEnum.disabled;
        }
        //Debug.Log(unit.gameObject.name);
        
        if(currentTurnsUnits.Count == 0)
        {
            CombatManager.Instance.NextCombatPhase();
        }
    }

    private void KillUnit(IUnit unit)
    {
        StartCoroutine(KillUnitCo(unit));
    }

    public IEnumerator KillUnitCo(IUnit unit)
    {
        CombatManager.Instance.canEndTurn = false;
        unit.IsDead = true;
        yield return null;
        if(CombatManager.Instance.CombatPhase == (int) CombatPhaseEnum.ally 
            && unit is Character)
        {
            unit.OutOfActionsThisTurn = true;
            unit.OutOfMovementThisTurn = true;
        }

        CombatManager.Instance.canEndTurn = true;
        StopCoroutine(KillUnitCo(unit));
    }

    public void CheckForCombatEnd()
    {
        //Manager.Instance.GameState = (int) GameStateEnum.explore;
    }

    public List<IUnit> FindUnitsInRange(CombatAction action, Character user, int unitMovementLeft)
    {
        //Add if statements to check for action type once more are added (Like AOE)
        TileGrid combatGrid = BattleGrid.Instance.tileGrid;
        PathFinder pathFinder = BattleGrid.Instance.pathFinder;
        List<IUnit> targetUnits = new List<IUnit>();

        List<IUnit> targetUnitsInRange = new List<IUnit>();
        
        if((action.targetsOpponents && (user is Player || user is Ally)) || (!action.targetsOpponents && user is Enemy)) targetUnits.AddRange(enemies);
        else
        {
            targetUnits.Add(Player.Instance);
            targetUnits.AddRange(allies);
        }

        foreach(IUnit unit in targetUnits)
        {
            if(!unit.IsDead
                && pathFinder.CalculateDistance(combatGrid.grid[unit.tileXPos, unit.tileYPos], combatGrid.grid[user.tileXPos, user.tileYPos])
                   <= action.range) 
            {
                targetUnitsInRange.Add(unit);
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
                    targetUnitsInRange.Add(unit);
                }
            }
        }

        //Debug.Log(targetUnitsInRange.Count);
        return targetUnitsInRange;
    }

    public void EnableTargetClickables(CombatAction action, Character user, int unitMovementLeft)
    {
        List<IUnit> units = FindUnitsInRange(action, user, unitMovementLeft);
        foreach(IUnit unit in units)
        {
            unit.targetClickable.gameObject.SetActive(true);
            //Debug.Log(unit.name);
        }
    }

    public void DisableTargetClickables()
    {
        if(allies != null) foreach(IUnit unit in allies){unit.targetClickable.gameObject.SetActive(false);}
        if(enemies != null) foreach(IUnit unit in enemies){unit.targetClickable.gameObject.SetActive(false);}
        Player.Instance.targetClickable.gameObject.SetActive(false);
    }

    private void EnableCharacterUse(Character character)
    {
        character.StartUnitsTurn();
        currentTurnsUnits.Add(character);
        character.OnCompleteTurn += CompleteUnitsTurn;
        character.actionClickable.ClickState = (int) clickStateEnum.enabled;
    }

    public void EndCombat()
    {

    }

    private IEnumerator EnemyPhaseCo()
    {
        foreach(Character ally in allies)
        {
            ally.OnCompleteTurn -= CompleteUnitsTurn;
        }
        Player.Instance.OnCompleteTurn -= CompleteUnitsTurn;

        //Debug.Log(enemies.Count);
        currentTurnsUnits.Clear();
        foreach(Enemy enemy in enemies) currentTurnsUnits.Add((IUnit) enemy);
        foreach(IUnit unit in currentTurnsUnits)
        {
            Enemy enemy = (Enemy) unit;
            if(!enemy.IsDead)
            {
                enemy.StartUnitsTurn();
                yield return StartCoroutine(enemy.TakeEnemyTurn()); 
                enemy.OutOfMovementThisTurn = true;         
            }
        }
        CompleteEnemiesPhase();
        StopCoroutine(EnemyPhaseCo());
    }

    public IUnit GetActingUnit()
    {
        Clickable actingClickable = CursorController.Instance.selectedClickable;
        AllyCombatClickable actingAlly = null;
        if(actingClickable != null && actingClickable is AllyCombatClickable) actingAlly = (AllyCombatClickable) actingClickable;
        if(actingAlly != null) return actingAlly.unitComponent;
        //Debug.Log("oopsie");
        return null;
    }
}
