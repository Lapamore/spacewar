namespace SpaceBattle.Lib;
using Hwdtech;

public class StartMoveCommand : ICommand
{
    private readonly IMoveCommandStartable _obj;

    public StartMoveCommand(IMoveCommandStartable obj)
    {
        _obj = obj;
    }

    public void Execute()
    {
        _obj.property.ToList().ForEach(parameter =>
        IoC.Resolve<ICommand>("Object.SetProperty", _obj.target, parameter.Key, parameter.Value).Execute());

        var mCommand = IoC.Resolve<ICommand>("Commands.MoveCommand", _obj.target);
        var injectCommand = IoC.Resolve<ICommand>("Commands.Inject", _obj.target);
        IoC.Resolve<ICommand>("Object.SetProperty", _obj.target, "Commands.InjectMoveCommand", injectCommand);
        IoC.Resolve<IQueue>("Queue.Push").Add(injectCommand);

    }
}
