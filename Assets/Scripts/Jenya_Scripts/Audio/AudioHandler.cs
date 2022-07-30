using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private static AudioHandler audioHandlerInstance;
    public static AudioHandler GetInstance => audioHandlerInstance;

    public AudioSource[] musicAudio;
    public AudioSource[] sfxAudio;

    private int SfxSoundsCount = 11;
    private bool keepFadingIn, keepFadingOut;
    private int randomDeathSFX, randomKeySFX, randomLevelPassSFX, randomPlayBSFX, randomQuitBSFX;

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
    public void PlaySoundUIButtonToggle()
    {
        sfxAudio[8].Play();
    }
    public void PlaySoundUIPausePull()
    {
        sfxAudio[5].Play();
    }
    public void PlaySoundUIHover()
    {
        sfxAudio[9].Play();
    }
    public void PlaySoundUIPlayB()
    {
        randomPlayBSFX = Random.Range(24, 25);
        sfxAudio[randomPlayBSFX].Play();
    }
    public void PlaySoundUIQuitB()
    {
        randomQuitBSFX = Random.Range(26, 28);
        sfxAudio[randomQuitBSFX].Play();
    }

    // Gameplay
    public void PlaySoundGameplayChainSnap()
    {
        sfxAudio[4].Play();
    }
    public void PlaySoundGameplayChainMovement(bool play)
    {
        if (play == true)
        {
            sfxAudio[6].Play();
        }
        else
        {
            sfxAudio[6].Stop();
        }
    }
    public void PlaySoundGameplayPlatformMovement(bool play)
    {
        if (play == true)
        {
            sfxAudio[7].Play();
        }
        else
        {
            StartCoroutine(FadeOutSfx(7, 0.3f));
        }
    }
    public void PlaySoundGameplayKeyPickUp()
    {
        randomKeySFX = Random.Range(15, 18);
        sfxAudio[randomKeySFX].Play();
    }
    public void PlaySoundGameplayLevelPass()
    {
        randomLevelPassSFX = Random.Range(19, 23);
        sfxAudio[randomLevelPassSFX].Play();
    }
    public void PlaySoundGameplayDeath()
    {
        randomDeathSFX = Random.Range(10, 14);
        sfxAudio[randomDeathSFX].Play();
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
            if (musicAudio[1].isPlaying == false)
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

    public void StopAllSfx()
    {
        for(int i = 0; i<SfxSoundsCount; i++)
        {
            sfxAudio[i].Stop();
        }
    }

    IEnumerator FadeInSfx(int track, float speed, float maxVolume)
    {
        keepFadingIn = true;
        keepFadingOut = false;

        sfxAudio[track].volume = 0;
        float audioVolume = sfxAudio[track].volume;

        while(sfxAudio[track].volume < maxVolume && keepFadingIn)
        {
            audioVolume += speed;
            sfxAudio[track].volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutSfx(int track, float speed)
    {
        keepFadingIn = false;
        keepFadingOut = true;

        float audioVolume = sfxAudio[track].volume;

        while (sfxAudio[track].volume >= speed && keepFadingOut)
        {
            audioVolume -= speed;
            sfxAudio[track].volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeInMusic(int track, float speed, float maxVolume)
    {
        keepFadingIn = true;
        keepFadingOut = false;

        musicAudio[track].volume = 0;
        float audioVolume = musicAudio[track].volume;

        while (musicAudio[track].volume < maxVolume)
        {
            audioVolume += speed;
            musicAudio[track].volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutMusic(int track, float speed)
    {
        keepFadingIn = false;
        keepFadingOut = true;

        float audioVolume = musicAudio[track].volume;

        while (musicAudio[track].volume > 0)
        {
            audioVolume -= speed;
            musicAudio[track].volume = audioVolume;
            yield return new WaitForSeconds(1f);
            musicAudio[track].Stop();
        }
    }
}
