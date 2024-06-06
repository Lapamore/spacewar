using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateEndMoveCommand : IStrategy
{
    public IUObject? _obj;
    public object Invoke(params object[] args)
    {
        _obj = (IUObject)args[0];
        return new EndMoveCommand(IoC.Resolve<IMoveCommandEndable>("Adapter", typeof(IMoveCommandEndable), _obj));
    }
}