public class StateLevelSelection : MenuState
{
    InterfaceHandler interfaceHandler;

    public StateLevelSelection()
    {
        interfaceHandler = InterfaceHandler.GetInstance;
    }

    // Overwriting an abstract method and adding the logic for this state
    public override void StateLogic()
    {
        // Changing the enum state
        interfaceHandler.uiState = UIState.Levels;

        // Screens logic
        interfaceHandler.MainMenu.gameObject.SetActive(false);
        interfaceHandler.LevelSelection.gameObject.SetActive(true);
    }
}
