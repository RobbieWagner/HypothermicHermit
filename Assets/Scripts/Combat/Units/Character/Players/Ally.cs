using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnActionSelectionUp()
    {
        IUnit unit = null;
        if(Combat.Instance != null) unit = Combat.Instance.GetActingUnit();
        if(unit != null && unit == this) CurrentAction++;
    }

    protected void OnActionSelectionDown()
    {
        IUnit unit = null;
        if(Combat.Instance != null) unit = Combat.Instance.GetActingUnit();
        if(unit != null && unit == this) CurrentAction--;
    }
}
