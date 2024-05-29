using Hwdtech;

namespace SpaceBattle.Lib
{
    public class NewGameScopeStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            string newGameId = (string)args[0];
            var parentScope = (object)args[1];
            var timeQuantum = (double)args[2];

            var newGameScope = IoC.Resolve<object>("Scopes.New", parentScope);

            var gameScopeRegistry = IoC.Resolve<IDictionary<string, object>>("Scope.Map");
            gameScopeRegistry.Add(newGameId, newGameScope);

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newGameScope).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Time.Quantum", (object[] args) => (object)timeQuantum).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => new QueuePusher().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Pop", (object[] args) => new GameQueuePopStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.UObject", (object[] args) => new UObjectProvider().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Delete.UObject", (object[] args) => new RemoveGameUObjectStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();

            return newGameScope;
        }
    }
}
