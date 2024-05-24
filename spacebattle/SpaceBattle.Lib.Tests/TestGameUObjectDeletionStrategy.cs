using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class TestGameUObjectDeletionStrategy
    {
        public TestGameUObjectDeletionStrategy()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void DeleteGameUObjectSuccess()
        {
            var gameUObjectRegistry = new Dictionary<int, IUObject>();
            var mockGameUObject = new Mock<IUObject>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Delete.UObject", (object[] args) => new RemoveGameUObjectStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.Map", (object[] args) => gameUObjectRegistry).Execute();

            Assert.Empty(gameUObjectRegistry);
            gameUObjectRegistry.Add(0, mockGameUObject.Object);
            Assert.Single(gameUObjectRegistry);
            IoC.Resolve<Lib.ICommand>("Delete.UObject", 0).Execute();
            Assert.Empty(gameUObjectRegistry);
        }
    }
}
