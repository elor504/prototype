public class MenuContext
{
    private MenuState state = null;

    public MenuContext(MenuState _state)
    {
        this.TrabsitionTo(_state);
    }

    public void TrabsitionTo(MenuState _state)
    {
        this.state = _state;

        this.state.SetContext(this);
    }

    public void Request()
    {
        this.state.StateLogic();
    }
}
