using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class IUnit : MonoBehaviour
{
    [HideInInspector] public Vector2 position;
    [SerializeField] private Animator animator;

    public TargetClickable targetClickable;

    public List<CombatAction> unitActions;
    private int currentAction;
    public int CurrentAction
    {
        get{return currentAction;}
        set
        {
            if(currentAction == value % unitActions.Count) return;
            currentAction = value % unitActions.Count;
        }
    }

    public int tileXPos;
    public int tileYPos;
    
    [SerializeField] private int unitSpeed = 5;
    public virtual int UnitSpeed
    {
        get {return unitSpeed;}
        set
        {
            if(value == unitSpeed) return;
            unitSpeed = value;
        }
    }

    private bool outOfMovementThisTurn;
    public virtual bool OutOfMovementThisTurn
    {
        get{return outOfMovementThisTurn;}
        set
        {
            outOfMovementThisTurn = value;
            OnCompleteAction();
            if(outOfActionsThisTurn && outOfMovementThisTurn) 
            {
                OnCompleteTurn(this);
            }
        }
    }

    private bool outOfActionsThisTurn;
    public bool OutOfActionsThisTurn
    {
        get{return outOfActionsThisTurn;}
        set
        {
            outOfActionsThisTurn = value;
            OnCompleteAction();
            if(outOfActionsThisTurn && outOfMovementThisTurn) OnCompleteTurn(this);
        }
    }

    public delegate void OnCompleteTurnDelegate(IUnit unit);
    public event OnCompleteTurnDelegate OnCompleteTurn = delegate { };

    public delegate void OnCompleteActionDelegate();
    public event OnCompleteActionDelegate OnCompleteAction = delegate { };

    protected virtual void Awake() 
    {
        UnitSpeed = unitSpeed;
        targetClickable.gameObject.SetActive(false);
    }

    public virtual void AddUnitToGrid()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        CombatTile combatTile = other.gameObject.GetComponent<CombatTile>();
        
        if(combatTile != null)
        {
            combatTile.AddCollidingUnit(this);
        }
    }

    public virtual void UseUnitMovement(Vector2 newPosition, int spentMovement)
    {
        MoveUnit(newPosition, spentMovement);
        OnMoveUnit(this);
    }

    public delegate void OnMoveUnitDelegate(IUnit unit);
    public event OnMoveUnitDelegate OnMoveUnit = delegate { };

    public virtual void UseUnitAction(IUnit target)
    {
        StartCoroutine(UseUnitActionCo(target, unitActions[currentAction]));
    }

    public virtual IEnumerator UseUnitActionCo(IUnit target, CombatAction action)
    {
        //if the action is a melee attack, move the ally before attacking
        if(action.GetType().Equals(typeof(MeleeAttack)))
        {
            List<Node> path = BattleGrid.Instance.pathFinder.FindPath(tileXPos, tileYPos, target.tileXPos, target.tileYPos);
            path.RemoveAt(path.Count-1);
            yield return StartCoroutine(MoveUnitCo(path));
        }

        unitActions[CurrentAction].Act(this, target);
        OnAct(this);
    }

    public delegate void OnActDelegate(IUnit unit);
    public event OnActDelegate OnAct = delegate { };

    #region Movement
    public virtual void MoveUnit(Vector2 newPosition, int spentMovement, float movementDuration = 1f)
    {
        float duration = (1-Mathf.InverseLerp(1,0, transform.position.x));
        StopAllCoroutines();
        StartMovementAnimation(newPosition);
        transform.DOMove(newPosition, movementDuration, false)
            .OnComplete(StopMovement);
        position = newPosition;
        if(spentMovement > 0) OnEndMoveUnit(spentMovement);
    }

    public virtual void MoveUnit(List<Node> path, float movementDuration = .2f)
    {
        StartCoroutine(MoveUnitCo(path, movementDuration));
    }

    private IEnumerator MoveUnitCo(List<Node> path, float movementDuration = .2f)
    {
        if(path == null) Debug.Log(";p");
        foreach(Node node in path)
        {
            CombatTile destination = node.GetTile();
            Vector3 newPosition = destination.transform.position - BattleGrid.Instance.TILE_OFFSET;
            StartMovementAnimation(newPosition);
            yield return transform.DOMove(newPosition, movementDuration, false)
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
            position = newPosition;
        }

        StopMovement();
        OnEndMoveUnit(path.Count);
        StopCoroutine(MoveUnitCo(path, movementDuration));
    }

    private void StopMovement()
    {
        animator.SetBool("combatMoving", false);
        animator.ResetTrigger("combatLeft");
        animator.ResetTrigger("combatRight");
        animator.ResetTrigger("combatUp");
        animator.ResetTrigger("combatDown");
        BattleGrid.Instance.EnableTileColliders();
    }

    public delegate void OnEndMoveUnitDelegate(int spentMovement);
    public event OnEndMoveUnitDelegate OnEndMoveUnit = delegate { };    

    private void StartMovementAnimation(Vector2 targetPosition)
    {
        //Debug.Log("movement started");
        Vector2 direction = targetPosition - new Vector2(transform.position.x, transform.position.y);
        animator.SetBool("combatMoving", true);
        if(Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            if(direction.x >= 0) animator.SetTrigger("combatRight");
            else animator.SetTrigger("combatLeft");
        }
        else
        {
            if(direction.y >= 0) animator.SetTrigger("combatUp");
            else animator.SetTrigger("combatDown");
        }
    }
    #endregion
    public void StartUnitsTurn()
    {
        OutOfActionsThisTurn = false;
        OutOfMovementThisTurn = false;
    }

    public int CalculateDistanceFromUnit(IUnit other)
    {
        int distanceX = Math.Abs(tileXPos - other.tileXPos);
        int distanceY = Math.Abs(tileYPos - other.tileYPos);

        return Math.Max(distanceX, distanceY);
    }
}
