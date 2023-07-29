using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private IUnit unit;
    [SerializeField] public UnitInformation unitInfoPrefab;
    private UnitInformation currentUnitInfo;
    [SerializeField] private TargetClickable targetClickable;

    private void Awake() 
    {
        triggerCollider.enabled = false;

        targetClickable.OnDisableTarget += EnableHover;
        BattleGrid.Instance.OnBattleGridCreated += EnableHover;
    }

    private void EnableHover()
    {
        triggerCollider.enabled = true;
    }

    private void OnDisable()
    {
        if(currentUnitInfo != null) currentUnitInfo = CombatHUD.Instance.RemoveUnitInformation(currentUnitInfo);
        triggerCollider.enabled = false;
    }

    public void OnPointerEnter()
    {
        if(Manager.Instance.GameState == (int) GameStateEnum.combat 
        && unitInfoPrefab != null) 
        {
            currentUnitInfo = CombatHUD.Instance.AddUnitInformation(unit);
        }
    }

    public void OnPointerExit()
    {
        if(currentUnitInfo != null) currentUnitInfo = CombatHUD.Instance.RemoveUnitInformation(currentUnitInfo);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData data)
    {
        OnPointerEnter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData data)
    {
        OnPointerExit();
    }
}
