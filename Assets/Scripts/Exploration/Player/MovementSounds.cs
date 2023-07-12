using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{

    [SerializeField] public AudioSource movementSounds;

    [SerializeField] private AudioClip defaultWalkSound;
    [SerializeField] private AudioClip defaultRunSound;

    [SerializeField] private AudioClip[] alternativeWalkSounds;
    [SerializeField] private AudioClip[] alternativeRunSounds;

    int currentSound;
    bool running;

    private void Start()
    {
        currentSound = -1;
        running = false;

        ChangeSound(currentSound);
    }

    public void ToggleMovementSounds(bool playSound) 
    {
        if(playSound) movementSounds.Play();
        else movementSounds.Stop();

        //if(movementSounds.isPlaying) Debug.Log("hi");
        //else Debug.Log("bye");
    }

    public void ToggleRun(bool isRunning)
    {
        bool playSound = movementSounds.isPlaying;

        ToggleMovementSounds(false);

        if(isRunning)
        {
            running = true;
            if(currentSound < 0) movementSounds.clip = defaultRunSound;
            else movementSounds.clip = alternativeRunSounds[currentSound];
        }
        else
        {
            running = false;
            if(currentSound < 0) movementSounds.clip = defaultWalkSound;
            else movementSounds.clip = alternativeWalkSounds[currentSound];
        }

        if(playSound) ToggleMovementSounds(true);
    }

    public void ChangeSound(int newSound)
    {
        bool playSound = movementSounds.isPlaying;
    
        ToggleMovementSounds(false);

        currentSound = newSound;

        if(running)
        {
            if(currentSound < 0) movementSounds.clip = defaultRunSound;
            else movementSounds.clip = alternativeRunSounds[currentSound];
        }
        else
        {
            running = false;
            if(currentSound < 0) movementSounds.clip = defaultWalkSound;
            else movementSounds.clip = alternativeWalkSounds[currentSound];
        }

        if(playSound) ToggleMovementSounds(true);
    }

    public void ResetToDefaultSound()
    {
        bool playSound = movementSounds.isPlaying;
        ToggleMovementSounds(false);

        currentSound = -1;

        if(running) movementSounds.clip = defaultRunSound;
        else movementSounds.clip = defaultWalkSound;

        if(playSound) ToggleMovementSounds(true);
    }
}
