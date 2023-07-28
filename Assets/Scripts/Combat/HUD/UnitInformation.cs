using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UnitInformation : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI displayName;
    [SerializeField] public TextMeshProUGUI unitHealth;
    [SerializeField] public ActionEffectDisplayLine displayLinePrefab;
    private ActionEffectDisplayLine[] displayLines;
    [SerializeField] public VerticalLayoutGroup panel;

    public virtual void Configure(IUnit unit)
    {
        displayName.text = unit.unitName;
        unitHealth.text = unit.Health.ToString() + "/" + unit.maxHealth.ToString();
    }
}
