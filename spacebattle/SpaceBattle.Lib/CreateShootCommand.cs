using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateShootCommand : IStrategy
{
    public IUObject? _obj;
    public object Invoke(params object[] args)
    {
        _obj = (IUObject)args[0];
        return new ShootCommand(IoC.Resolve<IShootable>("Adapter", typeof(IShootable), _obj));
    }
}
