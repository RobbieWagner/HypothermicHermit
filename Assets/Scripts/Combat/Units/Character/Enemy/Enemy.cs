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
        List<CombatTile> accessibleTiles = BattleGrid.Instance.FindAccessibleTiles(UnitSpeed, new Vector2(tileXPos, tileYPos), tileXPos, tileYPos);

        CombatTile tile = accessibleTiles[UnityEngine.Random.Range(0, accessibleTiles.Count)];
        List<Node> path = BattleGrid.Instance.pathFinder.FindPath(tileXPos, tileYPos, tile.x, tile.y);

        if(path != null) yield return StartCoroutine(MoveUnitCo(path));

        StopCoroutine(TakeEnemyTurn());
    }
}
