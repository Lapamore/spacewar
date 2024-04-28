using Hwdtech;

namespace SpaceBattle.Lib;

public class HardStop : IStrategy
{
    public object Invoke(params object[] args)
    {
        var id = args[0];
        var st = IoC.Resolve<ServerThread>("ServerThreadID", id);
        var hardstop = new StopServerThread(st);
        if (args.Length > 1)
        {
            var action = (Action)args[1];
            var command = new List<ICommand>();
            var actioncmd = new ActionCommand((Action)args[1]);
            command.Add(hardstop);
            command.Add(actioncmd);
            return new MacroCommand(command);
        }
        else
        {
            return hardstop;
        }
    }
}
