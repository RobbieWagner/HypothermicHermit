using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Defines an enemy to fight in Combats
public class Enemy : Character
{
    public override void AddUnitToGrid()
    {
        //Combat.Instance.combatEnemies.Add(this);
    }

    public virtual IEnumerator TakeEnemyTurn()
    {
        yield return CombatCameraMovement.Instance.MoveCameraCo(transform.position);
        CombatAction action = null;

        List<CombatAction> possibleActions = FindPossibleActionsToTake();

        if(possibleActions.Count > 0)
        {
            action = possibleActions[UnityEngine.Random.Range(0, possibleActions.Count)];
            CurrentAction = unitActions.IndexOf(action);
        }

        if(action != null) 
        {
            UseUnitAction(Combat.Instance.FindUnitsInRange(action, this, UnitSpeed)[0]);
        }
        else
        {
            List<Node> path = null;
            IUnit playerUnit = FindNearestPlayerUnit();
            if(playerUnit != null)
            {
                path = BattleGrid.Instance.pathFinder.FindPath(tileXPos, tileYPos, playerUnit.tileXPos, playerUnit.tileYPos).GetRange(0,UnitSpeed);
            }
            else //Choose a random accessible tile to move to
            {
                List<CombatTile> accessibleTiles = BattleGrid.Instance.FindAccessibleTiles(UnitSpeed, new Vector2(tileXPos, tileYPos), tileXPos, tileYPos);
                CombatTile tile = accessibleTiles[UnityEngine.Random.Range(0, accessibleTiles.Count)];
                
                path = BattleGrid.Instance.pathFinder.FindPath(tileXPos, tileYPos, tile.x, tile.y);
            }

            if(path != null) yield return StartCoroutine(MoveUnitCo(path));
            OutOfActionsThisTurn = true;
        }

        while(!OutOfActionsThisTurn) yield return null;

        StopCoroutine(TakeEnemyTurn());
    }

    private List<CombatAction> FindPossibleActionsToTake()
    {
        List<CombatAction> possibleActions = new List<CombatAction>();

        if(unitActions.Count > 0)
        {
            foreach(CombatAction action in unitActions)
            {
                if(Combat.Instance.FindUnitsInRange(action, this, UnitSpeed).Count > 0)
                {
                    possibleActions.Add(action);
                }
            }
        }

        return possibleActions;
    }

    private IUnit FindNearestPlayerUnit()
    {
        IUnit nearestUnit = null;
        int nearestDistance = int.MaxValue;

        foreach(IUnit unit in Combat.Instance.combatUnits)
        {
            if(unit is Ally || unit is Player)
            {
                int distance = BattleGrid.Instance.CalculateDistanceBetweenTiles(unit.tileXPos, unit.tileYPos, tileXPos, tileYPos);
                if(nearestUnit == null || distance < nearestDistance)
                {
                    nearestUnit = unit;
                    nearestDistance = distance;
                }
            }
        }

        return nearestUnit;
    }
}