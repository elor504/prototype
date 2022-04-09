using UnityEngine.SceneManagement;
using UnityEngine;

public class MenusInteractions : MonoBehaviour
{
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



    public void PlayB()
    {
        SceneManager.LoadScene(lastPlayedLevel);
    }
    public void LevelSelectionB()
    {
        var context = new MenuContext(new StateLevelSelection());
        context.Request();
    }
    public void SettingsB()
    {
        var context = new MenuContext(new StateSettings());
        context.Request();
    }
    public void CreditsB()
    {
        var context = new MenuContext(new StateCredits());
        context.Request();
    }
    public void QuitB()
    {
        Application.Quit();
    }
    public void BackB()
    {
        var context = new MenuContext(new StateMainMenu());
        context.Request();
    }
    public void TutorialB()
    {
        SceneManager.LoadScene(1);
    }
    public void Level1B()
    {
        //SceneManager.LoadScene(2);
    }
    public void Level2B()
    {
        //SceneManager.LoadScene(3);
    }
    public void Level3B()
    {
        //SceneManager.LoadScene(4);
    }
}
