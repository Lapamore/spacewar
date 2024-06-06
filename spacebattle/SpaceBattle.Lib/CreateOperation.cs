using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateOperation : IStrategy
{
    private IUObject? _obj;
    private string? _type;

    public object Invoke(params object[] args)
    {
        _obj = (IUObject)args[0];
        _type = (string)args[1];
        var rulesList = IoC.Resolve<IEnumerable<string>>("Rules.Get." + _type);
        var commandList = rulesList.ToList().Select(rule => IoC.Resolve<ICommand>("Command.Create", rule, _obj));
        return IoC.Resolve<ICommand>("Command.Macro.Create", commandList);
    }
}