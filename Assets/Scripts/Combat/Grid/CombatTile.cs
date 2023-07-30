using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Acts as the "nodes" for smart movement when clicked
public class CombatTile : Clickable
{
    [SerializeField] private SpriteRenderer tileSpriteRenderer;
    [SerializeField] private Collider2D tileCollider;
    [SerializeField] private SpriteRenderer tileIsActiveSprite;

    public List<IUnit> collidingUnits;

    public int x;
    public int y;

    [SerializeField] public float cost = 1;

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
            unit.tileXPos = x;
            unit.tileYPos = y;
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

                List<Node> path = BattleGrid.Instance.pathFinder.FindPath(allyClickable.unitComponent.tileXPos, allyClickable.unitComponent.tileYPos, x, y);
                allyClickable.unitComponent.MoveUnit(path);
                BattleGrid.Instance.DisableAllTileColliders();
                //CursorController.Instance.UnsetSelectedClickable();
                Combat.Instance.DisableTargetClickables();
            }
        }
    }

    public void EnableTrigger(bool enable, bool enableActiveSprite = false)
    {
        tileCollider.enabled = enable;
        tileIsActiveSprite.enabled = enableActiveSprite;
    }
}
