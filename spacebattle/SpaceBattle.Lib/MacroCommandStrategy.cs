using Hwdtech;
namespace SpaceBattle.Lib;

public class MacroCommandStrategy : IStrategy
{
    public object Init(params object[] initializationArgs)
    {
        var operationName = (string)initializationArgs[0];
        var targetObject = (IUObject)initializationArgs[1];

        var dependentCommands = IoC.Resolve<string[]>("Component." + operationName);
        var commands = dependentCommands.Select(dependency => IoC.Resolve<ICommand>(dependency, targetObject));

        return IoC.Resolve<ICommand>("Game.Command.CreateMacro", commands);
    }
}

