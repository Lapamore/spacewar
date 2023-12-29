namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly IEnumerable<ICommand> _containedCommands;

    public MacroCommand(IEnumerable<ICommand> commands)
    {
        _containedCommands = commands;
    }

    public void Execute()
    {
        _containedCommands.ToList().ForEach(command => command.Execute());
    }
}

