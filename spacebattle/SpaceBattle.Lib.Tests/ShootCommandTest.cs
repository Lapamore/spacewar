using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ShootCommandTests
{

    public ShootCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfulShootTest()
    {
        var queue = new Queue<Lib.ICommand>();

        var cmd = new Mock<Lib.ICommand>();
        cmd.Setup(c => c.Execute()).Verifiable();

        var queuePushCommand = new Mock<Lib.ICommand>();
        queuePushCommand.Setup(c => c.Execute()).Callback(() => queue.Enqueue(cmd.Object)).Verifiable();

        var shootCommand = new Mock<Lib.ICommand>();
        shootCommand.Setup(s => s.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue", (object[] args) => queue).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.Shooting", (object[] args) => shootCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => queuePushCommand.Object).Execute();

        var obj = new Mock<IShootable>();
        var shootCommandInstance = new ShootCommand(obj.Object);

        shootCommandInstance.Execute();

        Assert.NotEmpty(queue);
        queuePushCommand.Verify(c => c.Execute(), Times.Once());
        shootCommand.Verify(s => s.Execute(), Times.Never());

    }
}