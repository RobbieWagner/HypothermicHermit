using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionInformation : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI actionName;
    [SerializeField] public ActionEffectDisplayLine displayLinePrefab;
    private ActionEffectDisplayLine[] displayLines;
    [SerializeField] public VerticalLayoutGroup panel;

    public virtual void Configure(IUnit user, IUnit target)
    {
        CombatAction action = user.unitActions[user.CurrentAction];
        actionName.text = action.actionName;

        displayLines = new ActionEffectDisplayLine[action.effects.Count];
        for(int i = 0; i < displayLines.Length; i++)
        {
            displayLines[i] = Instantiate(displayLinePrefab, panel.transform);
            action.effects[i].ConfigureEffectDisplayLine(user, target, displayLines[i]);
        }
    }
}
