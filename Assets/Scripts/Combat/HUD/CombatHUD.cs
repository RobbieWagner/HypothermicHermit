using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{

    [SerializeField] private ActionInformation actionInformationPrefab;
    private List<ActionInformation> displayedActionInformation;

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
    }

    public ActionInformation AddActionInformation(IUnit user, IUnit target)
    {
        Debug.Log("yuh");
        ActionInformation newActionInfo = Instantiate(actionInformationPrefab.gameObject, transform).GetComponent<ActionInformation>();
        displayedActionInformation.Add(newActionInfo);
        return newActionInfo;
    }

    public ActionInformation RemoveActionInformation(ActionInformation info)
    {
        Debug.Log("nuh");
        if(displayedActionInformation.Contains(info)) displayedActionInformation.Remove(info);
        Destroy(info.gameObject);
        return null;
    }
}
