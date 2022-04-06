using UnityEngine.SceneManagement;
using UnityEngine;

public class MenusInteractions : MonoBehaviour
{
    

    void Start()
    {
        var context = new MenuContext(new StateMainMenu());
        context.Request();
    }



    public void PlayB()
    {
        SceneManager.LoadScene(1);
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
}
