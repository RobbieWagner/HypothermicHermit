using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadGameMenuItem : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Canvas loadingCanvas;

    [SerializeField] private GameObject[] turnOffObjects; //game objects that need to be turned off during loading

    //Starts the game
    public virtual void OnSelectMenuItem()
    {
        canvas.enabled = false;
        loadingCanvas.enabled = true;
        SceneManager.LoadScene(gameSceneName);
    }
}
