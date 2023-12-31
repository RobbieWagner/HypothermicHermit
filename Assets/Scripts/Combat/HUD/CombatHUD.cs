using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CombatHUD : MonoBehaviour
{

    private List<ActionInformation> displayedActionInformation;
    private List<UnitInformation> displayedUnitInformation;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    public static CombatHUD Instance {get; private set;}

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 

        displayedActionInformation = new List<ActionInformation>();
        displayedUnitInformation = new List<UnitInformation>();
    }

    private void FixedUpdate() 
    {
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        if (mousePos.x > .5f) verticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        else verticalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
    }

    public ActionInformation AddActionInformation(IUnit user, IUnit target)
    {
        //Debug.Log("yuh");
        ActionInformation newActionInfo = Instantiate(user.unitActions[user.CurrentAction].actionInfo.gameObject, transform).GetComponent<ActionInformation>();
        ConfigureActionInformation(user, target, newActionInfo);
        displayedActionInformation.Add(newActionInfo);
        return newActionInfo;
    }

    public ActionInformation RemoveActionInformation(ActionInformation info)
    {
        //Debug.Log("nuh");
        if(displayedActionInformation.Contains(info)) displayedActionInformation.Remove(info);
        Destroy(info.gameObject);
        return null;
    }

    public void RemoveAllActionInformation()
    {
        foreach(ActionInformation actionInfo in displayedActionInformation)
        {
            Destroy(actionInfo.gameObject);
        }
        displayedActionInformation.Clear();
    }

    private void ConfigureActionInformation(IUnit user, IUnit target, ActionInformation actionInfo)
    {
        actionInfo.Configure(user, target);
    }

    
    public UnitInformation AddUnitInformation(IUnit unit)
    {
        UnitInformation newUnitInfo = Instantiate(unit.unitInfo.gameObject, transform).GetComponent<UnitInformation>();
        ConfigureUnitInformation(unit, newUnitInfo); 
        displayedUnitInformation.Add(newUnitInfo);
        return newUnitInfo;   
    }

    public UnitInformation RemoveUnitInformation(UnitInformation info)
    {
        if(displayedUnitInformation.Contains(info)) displayedUnitInformation.Remove(info);
        Destroy(info.gameObject);
        return null;
    }

    public void RemoveAllUnitInformation()
    {
        foreach(UnitInformation unitInformation in displayedUnitInformation)
        {
            Destroy(unitInformation.gameObject);
        }
        displayedUnitInformation.Clear();
    }

    public void ConfigureUnitInformation(IUnit unit, UnitInformation unitInfo)
    {
        unitInfo.Configure(unit);
    }
}
