using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTile : Clickable
{
    [SerializeField] private SpriteRenderer tileSpriteRenderer;
    public Collider2D tileCollider;

    public List<IUnit> collidingUnits;

    public int tileXPos;
    public int tileYPos;

    private void Awake() 
    {
        collidingUnits = new List<IUnit>();    
    }

    public void AddCollidingUnit(IUnit unit)
    {
        if(!collidingUnits.Contains(unit)) 
        {
            collidingUnits.Add(unit);
            tileCollider.enabled = false;
            unit.tileXPos = tileXPos;
            unit.tileYPos = tileYPos;
        }
    }

    public void RemoveCollidingUnit(IUnit unit)
    {
        if(collidingUnits.Contains(unit)) collidingUnits.Remove(unit);
        if(collidingUnits.Count == 0) 
        {
            tileCollider.enabled = true;
        }
    }

    protected override void OnPointerEnter()
    {
        base.OnPointerEnter();
    }

    protected override void OnPointerExit()
    {
        base.OnPointerExit();
    }

    protected override void OnPointerDown()
    {
        if(CursorController.Instance.clickables.Count > 0 && CursorController.Instance.clickables[0] == this && CursorController.Instance.selectedClickable != null)
        {
            AllyCombatMovement allyMovement = CursorController.Instance.selectedClickable.GetComponent<AllyCombatMovement>();
            if(allyMovement != null)
            {
                IUnit unit = allyMovement.unitComponent;

                unit.UseUnitMovement(new Vector2(transform.position.x, transform.position.y - BattleGrid.Instance.GetCellSize()/4));
            }
        }
    }
}
