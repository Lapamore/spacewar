using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateStartMoveCommand : IStrategy
{
    public IUObject? _obj;
    public object Invoke(params object[] args)
    {
        _obj = (IUObject)args[0];
        return new StartMoveCommand(IoC.Resolve<IMoveCommandStartable>("Adapter", typeof(IMoveCommandStartable), _obj));
    }
}