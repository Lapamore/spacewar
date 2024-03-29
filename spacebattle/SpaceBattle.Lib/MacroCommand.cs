namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly IEnumerable<ICommand> command;
    public MacroCommand(IEnumerable<ICommand> _command)
    {
        command = _command;
    }
    public void Execute()
    {
        var icmd = command.GetEnumerator();
        while (icmd.MoveNext())
        {
            icmd.Current.Execute();
        }
    }
}
