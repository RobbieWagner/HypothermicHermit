using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool paused;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] Canvas pauseMenuCanvas;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource[] levelMusicTracks;

    bool musicWasPlaying;

    bool timeWasOn;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        pauseMenuCanvas.enabled = false;

        musicWasPlaying = false;

        timeWasOn = true;
    }

    public void OnPause()
    {
        //Pause or unpause at the appropriate times
        if(!paused || (paused && pauseMenuCanvas.enabled)) 
        {
            paused = !paused;

            if(paused) 
            {
                if(Time.timeScale != 0) timeWasOn = true;
                Time.timeScale = 0;
                pauseMenuCanvas.enabled = true;
                
                audioMixer.SetFloat("music", -80);
                audioMixer.SetFloat("game", -80);

                foreach(AudioSource track in levelMusicTracks)
                {
                    if(track.isPlaying)
                    {
                        track.Pause();
                        musicWasPlaying = true;
                    }
                }

                Cursor.visible = false;
            }
            else 
            {
                if(timeWasOn) Time.timeScale = 1f;
                pauseMenuCanvas.enabled = false;
                AudioListener.pause = false;

                audioMixer.SetFloat("music", PlayerPrefs.GetFloat("music_volume", 0));
                audioMixer.SetFloat("game", PlayerPrefs.GetFloat("game_volume", 0));

                if(musicWasPlaying)
                {
                    foreach(AudioSource track in levelMusicTracks)
                    {
                        track.UnPause();
                    }
                }

                musicWasPlaying = false;
                Cursor.visible = true;
            }
        }
    }
}
