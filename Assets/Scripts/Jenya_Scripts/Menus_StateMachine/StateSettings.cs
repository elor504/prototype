public class StateSettings : MenuState
{
    InterfaceHandler interfaceHandler;

    public StateSettings()
    {
        interfaceHandler = InterfaceHandler.GetInstance;
    }

    // Overwriting an abstract method and adding the logic for this state
    public override void StateLogic()
    {
        // Changing the enum state
        interfaceHandler.uiState = UIState.Settings;

        // Screens logic
        interfaceHandler.MainMenu.gameObject.SetActive(false);
        interfaceHandler.Settings.gameObject.SetActive(true);
    }
}
