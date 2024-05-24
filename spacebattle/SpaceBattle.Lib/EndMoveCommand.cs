using Hwdtech;
namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    public IUObject target { get; }
    public BridgeCommand command { get; }
    public IEnumerable<string> property { get; }
}

public class EndMoveCommand : ICommand
{
    private readonly IMoveCommandEndable CommandEndable;

    public EndMoveCommand(IMoveCommandEndable moveCommandEndable)
    {
        CommandEndable = moveCommandEndable;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Target.DeleteProperty", CommandEndable.target, CommandEndable.property).Execute();
        var commandEmpty = new EmptyCommand();
        CommandEndable.command.Inject(commandEmpty);
    }
}
