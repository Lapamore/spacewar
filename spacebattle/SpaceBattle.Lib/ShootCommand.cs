using Hwdtech;

namespace SpaceBattle.Lib;

public class ShootCommand : ICommand
{
    private readonly IShootable _shootable;
    public ShootCommand(IShootable shootable)
    {
        _shootable = shootable;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<ICommand>("Operations.Shooting", _shootable);
        IoC.Resolve<ICommand>("Queue.Push", IoC.Resolve<Queue<ICommand>>("Game.Queue"), cmd).Execute();
    }
}