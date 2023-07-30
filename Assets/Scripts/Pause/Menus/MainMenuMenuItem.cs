using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMenuItem : MonoBehaviour
{

    [SerializeField] string mainMenuScene;
    public void OnSelectMenuItem()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }
}
