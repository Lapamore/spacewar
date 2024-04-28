namespace SpaceBattle.Lib;

public class UpdateBehavior : ICommand
{
    private readonly ServerThread updatebehavior;
    private readonly Action action;
    public UpdateBehavior(ServerThread _updatebehavior, Action _action)
    {
        updatebehavior = _updatebehavior;
        action = _action;
    }
    public void Execute()
    {
        if (updatebehavior.Equals(Thread.CurrentThread))
        {
            updatebehavior.UpdateBehaviour(action);
        }
        else
        {
            throw new Exception();
        }
    }
}
