namespace SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;

public class GameCommandTest
{
    public GameCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessExecute()
    {
        var ns = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQuantum", (object[] args) =>
        {
            var timer = new TimeSpan(0, 0, 5);
            return (object)timer;
        }).Execute();
        queue.Enqueue(new ActionCommand(() => { }));
        var cmd = new GameCommand(ns, queue);
        cmd.Execute();
        Assert.Empty(queue);
    }
    [Fact]
    public void SuccesExceptionHandler()
    {
        var handler = new Mock<IHandler>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameExceptionHandler", (object[] args) => handler.Object).Execute();
        var ns = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQuantum", (object[] args) =>
        {
            var timer = new TimeSpan(0, 0, 5);
            return (object)timer;
        }).Execute();
        queue.Enqueue(new ActionCommand(() =>
        {
            throw new Exception();
        }));
        var cmd = new GameCommand(ns, queue);
        cmd.Execute();
        handler.Verify(x => x.Handle(), Times.Once);
    }
    [Fact]
    public void UnsuccessExceptionHandler()
    {
        var handler = IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameExceptionHandler", (object[] args) => new ExceptionHandler((Exception)args[1]));
        var ns = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
        var queue = new Queue<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQuantum", (object[] args) =>
        {
            var timer = new TimeSpan(0, 0, 5);
            return (object)timer;
        }).Execute();
        handler.Execute();
        queue.Enqueue(new ActionCommand(() => { throw new Exception(); }));
        var cmd = new GameCommand(ns, queue);
        Assert.Throws<Exception>(() => cmd.Execute());
    }
}