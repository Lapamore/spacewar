namespace SpaceBattle.Lib.Tests;
using Hwdtech;
using Hwdtech.Ioc;

public class TestNewGameScopeStrategy
{
    public TestNewGameScopeStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void CreateGameScopeSuccess()
    {
        var gameScopeRegistry = new Dictionary<string, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.New",
            (object[] args) => new NewGameScopeStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Map",
            (object[] args) => gameScopeRegistry).Execute();

        var initialScope = IoC.Resolve<object>("Scopes.Current");
        var newGameScope = IoC.Resolve<object>("Scope.New", "gameId", initialScope, 400d);

        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Get.Time.Quantum"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", newGameScope).Execute();
        try
        {
            var timeQuantum = IoC.Resolve<object>("Get.Time.Quantum");
            Assert.Equal(400d, timeQuantum);
        }
        catch (Exception e) { Assert.Fail(e.Message); }

        IoC.Resolve<ICommand>("Scopes.Current.Set", initialScope).Execute();
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Get.Time.Quantum"));
    }
}
