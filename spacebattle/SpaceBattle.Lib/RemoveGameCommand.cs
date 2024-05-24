using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RemoveGameCommand : ICommand
    {
        private string _targetGameId;

        public RemoveGameCommand(string gameId) => _targetGameId = gameId;

        public void Execute()
        {
            var gameCommandRegistry = IoC.Resolve<IDictionary<string, IInjectable>>("Map");
            gameCommandRegistry[_targetGameId].Inject(
                IoC.Resolve<ICommand>("Command.Empty")
            );

            var gameScopeRegistry = IoC.Resolve<IDictionary<string, object>>("Scope.Map");
            gameScopeRegistry.Remove(_targetGameId);
        }
    }
}
