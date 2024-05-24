namespace SpaceBattle.Lib;

public class BridgeCommand : ICommand, IInjectable
{
    private ICommand internalCommand;

    public BridgeCommand(ICommand internalCommand)
    {
        this.internalCommand = internalCommand;
    }

    public void Inject(ICommand other)
    {
        internalCommand = other;
    }

    public void Execute()
    {
        internalCommand.Execute();
    }
    public ICommand GetCommand()
    {
        return internalCommand;
    }
}
