using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class AudioSettings : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private float musicVolumeFloat, soundEffectsVolumeFloat;
    public Slider musicSlider, soundEffectsSlider;
    [SerializeField] private List<AudioSource> musicAudio;
    [SerializeField] private List<AudioSource> sfxAudio;

    public static AudioSettings ASInstance;

    private void Awake()
    {
        ASInstance = this;

        if (AudioHandler.GetInstance != null)
        {
            musicAudio = new List<AudioSource>(AudioHandler.GetInstance.musicAudio);
            sfxAudio = new List<AudioSource>(AudioHandler.GetInstance.sfxAudio);
            ContinueSettings();
        }

        if (AudioHandler.GetInstance != null)
        {
            AudioHandler.GetInstance.PlayMusicMainMenu(false);
            AudioHandler.GetInstance.PlayMusicGame(true);
        }
    }

    private void ContinueSettings()
    {
        // Get volume floats info from PlayerPrefs and set it to local floats
        musicVolumeFloat = PlayerPrefs.GetFloat(MusicPref);
        soundEffectsVolumeFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

        // Set sliders to the same values
        musicSlider.value = musicVolumeFloat;
        soundEffectsSlider.value = soundEffectsVolumeFloat;


        // Set volume of all audio clips in the array to slider value
        for (int i = 0; i < musicAudio.Count; i++)
        {
            musicAudio[i].volume = musicVolumeFloat;
        }
        for (int i = 0; i < sfxAudio.Count; i++)
        {
            sfxAudio[i].volume = soundEffectsVolumeFloat;
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
