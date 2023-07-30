using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameMenuItem : MonoBehaviour
{
    public void OnSelectMenuItem()
    {
        Application.Quit();
    }
}
