namespace SpaceBattle.Lib;

public class ActionCommand : ICommand
{
    private readonly Action action;
    public ActionCommand(Action _action)
    {
        action = _action;
    }
    public void Execute()
    {
        action();
    }
}
