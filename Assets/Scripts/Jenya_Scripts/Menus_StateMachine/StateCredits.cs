public class StateCredits : MenuState
{
    InterfaceHandler interfaceHandler;

    public StateCredits()
    {
        interfaceHandler = InterfaceHandler.GetInstance;
    }

    // Overwriting an abstract method and adding the logic for this state
    public override void StateLogic()
    {
        // Changing the enum state
        interfaceHandler.uiState = UIState.Credits;

        // Screens logic
        interfaceHandler.MainMenu.gameObject.SetActive(false);
        interfaceHandler.Credits.gameObject.SetActive(true);
    }
}
