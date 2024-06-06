using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class RegistrationCommandTests
{

    public RegistrationCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfulRegistrationCommandTest()
    {
        var queue = new Queue<Lib.ICommand>();
        var cmd = new Mock<Lib.ICommand>();
        cmd.Setup(c => c.Execute()).Verifiable();
        var pushCommand = new Mock<Lib.ICommand>();
        pushCommand.Setup(c => c.Execute()).Callback(() => queue.Enqueue(cmd.Object));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "InicializationGameCommand", (object[] args) => cmd.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => pushCommand.Object).Execute();

        var id = "id";
        var registrationGameCommand = new RegistrationGameCommand(id);
        registrationGameCommand.Execute();

        cmd.Verify(c => c.Execute(), Times.Never);
        pushCommand.Verify(c => c.Execute(), Times.Once);
    }

}
