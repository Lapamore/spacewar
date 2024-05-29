namespace SpaceBattle.Lib.Tests;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class TestGameCreationStrategy
{
    public TestGameCreationStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void СreateGameSuccess()
    {
        var mockCmd = new Mock<Lib.ICommand>();
        mockCmd.Setup(x => x.Execute()).Verifiable();
        var gameCommandMap = new Dictionary<string, Lib.ICommand>();
        var commandQueue = new BlockingCollection<Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create",
            (object[] args) => new InitializeGameStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.New", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.New", (object[] args) => new Queue<Lib.ICommand>()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command", (object[] args) => mockCmd.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Queue", (object[] args) => commandQueue).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Macro",
            (object[] args) => new MacroCommand((List<Lib.ICommand>)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Inject",
            (object[] args) => new BridgeCommand((Lib.ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Concurrent.Repeat",
            (object[] args) => new RepeatConcurrentCommand((Lib.ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Map", (object[] args) => gameCommandMap).Execute();


        Assert.Empty(gameCommandMap);

        var newGameId = "gameId";
        var createGameCommand = IoC.Resolve<Lib.ICommand>(
            "Create",
            newGameId,
            IoC.Resolve<object>("Scopes.Current"),
            400d
        );

        Assert.Single(gameCommandMap);
        Assert.Equal(typeof(BridgeCommand), gameCommandMap[newGameId].GetType());
        Assert.Equal(typeof(BridgeCommand), createGameCommand.GetType());
        Assert.Equal(createGameCommand, gameCommandMap[newGameId]);

        Assert.Empty(commandQueue);
        commandQueue.Add(createGameCommand);
        Assert.Single(commandQueue);

        commandQueue.Take().Execute();
        commandQueue.Take().Execute();
        commandQueue.Take().Execute();
        mockCmd.Verify(x => x.Execute(), Times.Exactly(3));

        Assert.Single(commandQueue);
        commandQueue.Take();
        Assert.Empty(commandQueue);
    }
}
