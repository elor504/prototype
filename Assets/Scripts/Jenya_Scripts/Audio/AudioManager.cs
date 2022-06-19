using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider musicSlider, soundEffectsSlider;
    private float musicVolumeFloat, soundEffectsVolumeFloat;
    [SerializeField] private List<AudioSource> musicAudio;
    [SerializeField] private List<AudioSource> sfxAudio;

    private void Start()
    {
        musicAudio = new List<AudioSource>(AudioHandler.GetInstance.musicAudio);
        sfxAudio = new List<AudioSource>(AudioHandler.GetInstance.sfxAudio);
        AudioHandler.GetInstance.PlayMusicGame(false);
        AudioHandler.GetInstance.PlayMusicMainMenu(true);

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        // Check if we play the game for the first time
        if (firstPlayInt == 0)
        {
            // Set and save default values for the volume
            musicVolumeFloat = 0.1f;
            soundEffectsVolumeFloat = 0.3f;
            musicSlider.value = musicVolumeFloat;
            soundEffectsSlider.value = soundEffectsVolumeFloat;
            PlayerPrefs.SetFloat(MusicPref, musicVolumeFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsVolumeFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            // Get previously saved values and apply it for the volume
            musicVolumeFloat = PlayerPrefs.GetFloat(MusicPref);
            soundEffectsVolumeFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

            // Set sliders to the same values
            musicSlider.value = musicVolumeFloat;
            soundEffectsSlider.value = soundEffectsVolumeFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    private void OnApplicationFocus(bool inFocus)
    {
        // If we pause or minimize or exit the game - save the volume settings
        if (inFocus == false)
        {
            SaveSoundSettings();
        }
    }

    private void OnApplicationQuit()
    {
        SaveSoundSettings();
    }

    public void UpdateAudio()
    {
        // Set volume of all audio clips in the array to slider value
        for (int i = 0; i < musicAudio.Count; i++)
        {
            musicAudio[i].volume = musicSlider.value;
        }
        for (int i = 0; i < sfxAudio.Count; i++)
        {
            sfxAudio[i].volume = soundEffectsSlider.value;
        }
    }

    public void TriggerUIClicks()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
    }
    public void TriggerToggle()
    {
        AudioHandler.GetInstance.PlaySoundUIButtonHover();
    }
}