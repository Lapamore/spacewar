using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateTurnCommand : IStrategy
{
    public IUObject? _obj;
    public object Invoke(params object[] args)
    {
        _obj = (IUObject)args[0];
        return new TurnCommand(IoC.Resolve<ITurn>("Adapter", typeof(ITurn), _obj));
    }
}
