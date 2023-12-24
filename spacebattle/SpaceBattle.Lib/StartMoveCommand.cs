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
        _obj.Parameters.ToList().ForEach(parameter =>
        IoC.Resolve<ICommand>("Object.SetProperty", _obj.UObject, parameter.Key, parameter.Value).Execute());

        var mCommand = IoC.Resolve<ICommand>("Commands.MoveCommand", _obj.UObject);
        var injectCommand = IoC.Resolve<ICommand>("Commands.Inject", _obj.UObject);
        IoC.Resolve<ICommand>("Object.SetProperty", _obj.UObject, "Commands.InjectMoveCommand", injectCommand);
        IoC.Resolve<IQueue>("Queue.Push").Push(injectCommand);

    }
}
