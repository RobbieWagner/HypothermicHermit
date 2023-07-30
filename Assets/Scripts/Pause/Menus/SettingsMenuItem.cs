using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{

    [SerializeField] private Canvas menu;
    [SerializeField] private Canvas settings;

    public void OnSelectMenuItem()
    {
        menu.enabled = false;
        settings.enabled = true;
    }
}
