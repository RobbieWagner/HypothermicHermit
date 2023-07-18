using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTile : Clickable
{
    [SerializeField] private SpriteRenderer tileSpriteRenderer;
    [SerializeField] private Collider2D tileCollider;
    [SerializeField] private SpriteRenderer tileIsActiveSprite;

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
            EnableTrigger(false);
            unit.tileXPos = tileXPos;
            unit.tileYPos = tileYPos;
        }
    }

    public void RemoveCollidingUnit(IUnit unit)
    {
        if(collidingUnits.Contains(unit)) collidingUnits.Remove(unit);
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
            AllyCombatClickable allyClickable = CursorController.Instance.selectedClickable.GetComponent<AllyCombatClickable>();
            if(allyClickable != null)
            {
                IUnit unit = allyClickable.unitComponent;

                int spentMovement = BattleGrid.Instance.CalculateDistanceBetweenTiles(tileXPos, tileYPos, allyClickable.unitComponent.tileXPos, allyClickable.unitComponent.tileYPos);

                unit.UseUnitMovement(new Vector2(transform.position.x, transform.position.y - BattleGrid.Instance.GetCellSize()/4), spentMovement);
                BattleGrid.Instance.DisableAllTileColliders();
                CursorController.Instance.UnsetSelectedClickable();
            }
        }
    }

    public void EnableTrigger(bool enable, bool enableActiveSprite = false)
    {
        tileCollider.enabled = enable;
        tileIsActiveSprite.enabled = enableActiveSprite;
    }
}
