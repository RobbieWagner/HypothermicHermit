using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonSounds : MonoBehaviour, IPointerEnterHandler
{

    AudioSource navigationSound;
    AudioSource selecitonSound;

    // Start is called before the first frame update
    void Start()
    {
        navigationSound = GameObject.Find("MenuNavigationSound").GetComponent<AudioSource>();
        selecitonSound = GameObject.Find("MenuSelectionSound").GetComponent<AudioSource>();
    }

    public void PlaySelectionSound()
    {
        if(selecitonSound != null) selecitonSound.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(navigationSound != null) navigationSound.Play();
    }
}
