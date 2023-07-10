using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Defines an enemy to fight in Combats
public class Enemy : Unit
{
    public override void AddUnitToGrid()
    {
        Combat.Instance.combatEnemies.Add(this);

        Vector2 playerPosition = GameGrid.Instance.GetTilePosition(Player.Instance.transform);
        float startingDistanceFromEnemies = BattleGridManager.Instance.startingDistanceFromEnemies;

        float distanceFromPlayer = Vector2.Distance(transform.position, playerPosition);
        if(distanceFromPlayer < startingDistanceFromEnemies)
        {
            float distanceToMove = -(startingDistanceFromEnemies - distanceFromPlayer);
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, distanceToMove);
        }

        transform.position = new Vector2(MathF.Truncate(transform.position.x), MathF.Truncate(transform.position.y));

        Combat.Instance.combatEnemies.Add(this);
    }
}
