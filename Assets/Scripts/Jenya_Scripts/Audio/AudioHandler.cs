using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private static AudioHandler audioHandlerInstance;
    public static AudioHandler GetInstance => audioHandlerInstance;

    public AudioSource[] musicAudio;
    public AudioSource[] sfxAudio;

    private void Awake()
    {
        if (audioHandlerInstance == null)
        {
            audioHandlerInstance = this;
        }
        else if (audioHandlerInstance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    #region SFX
    // UI
    public void PlaySoundUIClicks()
    {
        sfxAudio[0].Play();
    }
    public void PlaySoundUIClose()
    {
        sfxAudio[1].Play();
    }
    public void PlaySoundUIGameExit()
    {
        sfxAudio[2].Play();
    }
    public void PlaySoundUILockedLevel()
    {
        sfxAudio[3].Play();
    }

    // Gameplay
    public void PlaySoundGameplayChainSnap()
    {
        sfxAudio[4].Play();
    }
    #endregion

    #region Music
    public void PlayMusicMainMenu(bool play)
    {
        if (play == true)
        {
            musicAudio[0].Play();
        }
        else
        {
            musicAudio[0].Stop();
        }
    }
    public void PlayMusicGame(bool play)
    {
        if (play == true)
        {
            if(musicAudio[1].isPlaying == false)
            {
                musicAudio[1].Play();
            }
        }
        else
        {
            musicAudio[1].Stop();
        }
    }
    #endregion
}
