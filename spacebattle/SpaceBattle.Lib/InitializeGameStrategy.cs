using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InitializeGameStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            string newGameId = (string)args[0];
            var parentScope = (object)args[1];
            var timeQuantum = (double)args[2];

            var newGameScope = IoC.Resolve<object>("Scope.New", newGameId, parentScope, timeQuantum);
            var newGameQueue = IoC.Resolve<object>("Queue.New");
            var initializeCommand = IoC.Resolve<ICommand>("Command", newGameQueue, newGameScope);

            var commandList = new List<ICommand> { initializeCommand };
            var macroCommand = IoC.Resolve<ICommand>("Command.Macro", commandList);
            var injectCommand = IoC.Resolve<ICommand>("Command.Inject", macroCommand);
            var repeatCommand = IoC.Resolve<ICommand>("Command.Concurrent.Repeat", injectCommand);
            commandList.Add(repeatCommand);

            var commandRegistry = IoC.Resolve<IDictionary<string, ICommand>>("Map");
            commandRegistry.Add(newGameId, injectCommand);

            return injectCommand;
        }
    }
}
