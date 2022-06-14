using UnityEngine.SceneManagement;
using UnityEngine;

public class MenusInteractions : MonoBehaviour
{
    [HideInInspector] public bool isFullScreenMenus = true;
    [SerializeField]private int lastPlayedLevel = 0;
    private static MenusInteractions MIHInstance;
    public static MenusInteractions GetInstance => MIHInstance;

    private void Awake()
    {
        if (MIHInstance == null)
        {
            MIHInstance = this;
        }
        else if (MIHInstance != this)
        {
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        var context = new MenuContext(new StateMainMenu());
        context.Request();

        if(HUDInteractionsHandler.playedLevel != 0)
        {
            // Get the data from last scene
            lastPlayedLevel = HUDInteractionsHandler.playedLevel;
        }
        else
        {
            lastPlayedLevel = 1;    
        }
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
    public void LockedLevelB()
    {
        AudioHandler.GetInstance.PlaySoundUILockedLevel();
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
    public void Level1B()
    {
        lastPlayedLevel = 1;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(1);
    }
    public void Level2B()
    {
        lastPlayedLevel = 2;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(2);
    }
    public void Level3B()
    {
        lastPlayedLevel = 3;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(3);
    }
    public void Level4B()
    {
        lastPlayedLevel = 4;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(4);
    }
    public void Level5B()
    {
        lastPlayedLevel = 5;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(5);
    }
    public void Level6B()
    {
        lastPlayedLevel = 6;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(6);
    }
    public void Level7B()
    {
        lastPlayedLevel = 7;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(7);
    }
    public void Level8B()
    {
        lastPlayedLevel = 8;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(8);
    }
    public void Level9B()
    {
        lastPlayedLevel = 9;
        AudioHandler.GetInstance.PlaySoundUIClicks();
        SceneManager.LoadScene(9);
    }
    #endregion

    #region Settings
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
