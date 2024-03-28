using Hwdtech;

namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{
    private readonly int _serverSize;

    public StartServerCommand(int serverSize) => _serverSize = serverSize;

    public void Execute()
    {
        Enumerable.Range(0, _serverSize)
            .Select(i => IoC.Resolve<ICommand>("Server.Thread.Start", i))
            .ToList()
            .ForEach(command => command.Execute());
    }
}
