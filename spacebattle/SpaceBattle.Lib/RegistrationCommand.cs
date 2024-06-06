using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistrationGameCommand : ICommand
{
    private readonly string _gameId;
    public RegistrationGameCommand(string gameId)
    {
        _gameId = gameId;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<ICommand>("InicializationGameCommand");
        IoC.Resolve<ICommand>("Queue.Push", _gameId, cmd).Execute();
    }
}