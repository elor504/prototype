public abstract class MenuState
{
    protected MenuContext context;

    public void SetContext(MenuContext _context)
    {
        this.context = _context; 
    }

    // Abstract methid to overrite in different states
    public abstract void StateLogic();
}

// Enum StateMachine
public enum UIState
{
    MainMenu,
    Levels,
    Settings,
    Credits
}
