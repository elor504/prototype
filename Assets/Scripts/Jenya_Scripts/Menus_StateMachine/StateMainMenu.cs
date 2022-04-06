public class StateMainMenu : MenuState
{
    InterfaceHandler interfaceHandler;

    public StateMainMenu()
    {
        interfaceHandler = InterfaceHandler.GetInstance;
    }

    // Overwriting an abstract method and adding the logic for this state
    public override void StateLogic()
    {
        // Changing the enum state
        interfaceHandler.uiState = UIState.MainMenu;

        // Screens logic
        interfaceHandler.LevelSelection.gameObject.SetActive(false);
        interfaceHandler.Credits.gameObject.SetActive(false);
        interfaceHandler.Settings.gameObject.SetActive(false);
        interfaceHandler.MainMenu.gameObject.SetActive(true);
    }
}
