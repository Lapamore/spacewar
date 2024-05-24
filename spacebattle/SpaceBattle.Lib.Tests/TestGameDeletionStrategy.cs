using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class TestGameDeletionStrategy
    {
        public TestGameDeletionStrategy()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void DeleteGameSuccess()
        {
            var gameCommandMap = new Dictionary<string, IInjectable>();
            var gameScopeRegistry = new Dictionary<string, object>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Delete", (object[] args) => new RemoveGameStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Map", (object[] args) => gameCommandMap).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Empty", (object[] args) => new EmptyCommand()).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Map", (object[] args) => gameScopeRegistry).Execute();

            var mockGameCommand = new Mock<IInjectable>();
            var targetGameId = "gameId";
            var targetScope = "scope";

            Assert.Empty(gameCommandMap);
            Assert.Empty(gameScopeRegistry);

            gameCommandMap.Add(targetGameId, mockGameCommand.Object);
            gameScopeRegistry.Add(targetGameId, targetScope);
            Assert.Single(gameCommandMap);
            Assert.Single(gameScopeRegistry);

            IoC.Resolve<Lib.ICommand>("Delete", targetGameId).Execute();
            Assert.Single(gameCommandMap);
            Assert.Empty(gameScopeRegistry);
        }
    }
}
