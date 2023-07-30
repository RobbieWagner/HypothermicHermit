using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameMenuItem : MonoBehaviour
{

    [SerializeField] private PauseMenu pauseMenu;
    
    public void OnSelectMenuItem()
    {
        pauseMenu.OnPause();
    }
}
