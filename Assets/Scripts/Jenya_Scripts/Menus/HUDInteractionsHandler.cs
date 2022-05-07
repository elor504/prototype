using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDInteractionsHandler : MonoBehaviour
{
    private AudioSettings audioSettings;
    private HUDstate hudState;
    private bool isPauseAvailable;
    bool isFullScreenHUD;

    #region PublicFields
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    #endregion

    void Start()
    {
        audioSettings = AudioSettings.ASInstance;

        // Defalut HUD settings
        isPauseAvailable = true;
        if (MenusInteractions.GetInstance != null)
        {
            isFullScreenHUD = MenusInteractions.GetInstance.isFullScreenMenus;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (hudState == HUDstate.options)
            {
                Button_Back();
            }
            else if (hudState == HUDstate.pause && isPauseAvailable == false)
            {
                AudioHandler.GetInstance.PlaySoundUIClose();

                // Unfreeze time
                Time.timeScale = 1f;
                
                // Close pause menu
                pauseMenu.SetActive(false);

                // Set a flag for pause menu
                isPauseAvailable = true;
            }
            else if (isPauseAvailable == true)
            {
                AudioHandler.GetInstance.PlaySoundUIClicks();
                // Open pause menu
                pauseMenu.SetActive(true);

                // Change state
                hudState = HUDstate.pause;

                // Set a flag for pause menu
                isPauseAvailable = false;

                // Freeze time
                Time.timeScale = 0f;
            }
        }
    }
    public void Button_Continue()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        // Unfreeze time
        Time.timeScale = 1f;

        // Close pause menu
        pauseMenu.SetActive(false);

        // Set a flag for pause menu
        isPauseAvailable = true;
    }
    public void Button_Options()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        hudState = HUDstate.options;

        // Close pause menu and open options menu
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void Button_ExitToMM()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        // Unfreeze time
        Time.timeScale = 1f;

        // Save volume settings
        audioSettings.SaveSoundSettings();

        // Turn off game music
        AudioHandler.GetInstance.PlayMusicGame(false);

        SceneManager.LoadScene(0);
    }
    public void Button_Back()
    {
        AudioHandler.GetInstance.PlaySoundUIClose();
        // Close options menu and open pause menu
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        hudState = HUDstate.pause;
    }
    public void ScreenResolutionDropdownInputData(int value)
    {
        if (value == 0)
        {
            Debug.Log("Resolution 1");
            Screen.SetResolution(1920, 1080, isFullScreenHUD);
        }
        if (value == 1)
        {
            Debug.Log("Resolution 2");
            Screen.SetResolution(1366, 768, isFullScreenHUD);
        }
        if (value == 2)
        {
            Debug.Log("Resolution 3");
            Screen.SetResolution(1280, 720, isFullScreenHUD);
        }
    }
    public void WindowModeCheckBoxInputData(bool value)
    {
        if (value == true)
        {
            Debug.Log("Full screen");
            isFullScreenHUD = true;
            Screen.fullScreen = isFullScreenHUD;
        }
        if (value == false)
        {
            Debug.Log("Window screen");
            isFullScreenHUD = false;
            Screen.fullScreen = isFullScreenHUD;
        }
    }
}

enum HUDstate
{
    pause,
    options
}

