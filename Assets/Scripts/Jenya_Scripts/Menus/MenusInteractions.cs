using UnityEngine.SceneManagement;
using UnityEngine;

public class MenusInteractions : MonoBehaviour
{
    [HideInInspector] public bool isFullScreenMenus = true;
    private int lastPlayedLevel;

    void Start()
    {
        var context = new MenuContext(new StateMainMenu());
        context.Request();

        // Get the data from last scene
        lastPlayedLevel = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(InterfaceHandler.GetInstance.uiState != UIState.MainMenu)
            {
                BackB();
            }
        }
    }


    #region Buttons
    public void PlayB()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(lastPlayedLevel);
    }
    public void LevelSelectionB()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        var context = new MenuContext(new StateLevelSelection());
        context.Request();
    }
    public void SettingsB()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        var context = new MenuContext(new StateSettings());
        context.Request();
    }
    public void CreditsB()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        var context = new MenuContext(new StateCredits());
        context.Request();
    }
    public void QuitB()
    {
        AudioHandler.GetInstance.PlaySoundUIGameExit();
        Application.Quit();
    }
    public void BackB()
    {
        AudioHandler.GetInstance.PlaySoundUIClose();
        var context = new MenuContext(new StateMainMenu());
        context.Request();
    }
    public void TutorialB()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(1);
    }
    public void Level1B()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        //SceneManager.LoadScene(2);
    }
    public void Level2B()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        //SceneManager.LoadScene(3);
    }
    public void Level3B()
    {
        AudioHandler.GetInstance.PlaySoundUIClicks();
        //SceneManager.LoadScene(4);
    }
    #endregion

    #region Options
    public void ScreenResolutionDropdownInputData(int value)
    {
        if (value == 0)
        {
            Debug.Log("Resolution 1");
            Screen.SetResolution(1920, 1080, isFullScreenMenus);
        }
        if (value == 1)
        {
            Debug.Log("Resolution 2");
            Screen.SetResolution(1366, 768, isFullScreenMenus);
        }
        if (value == 2)
        {
            Debug.Log("Resolution 3");
            Screen.SetResolution(1280, 720, isFullScreenMenus);
        }
    }

    public void WindowModeCheckBoxInputData(bool value)
    {
        if (value == true)
        {
            Debug.Log("Full screen");
            isFullScreenMenus = true;
            Screen.fullScreen = isFullScreenMenus;
        }
        if (value == false)
        {
            Debug.Log("Window screen");
            isFullScreenMenus = false;
            Screen.fullScreen = isFullScreenMenus;
        }
    }

    #endregion
}
