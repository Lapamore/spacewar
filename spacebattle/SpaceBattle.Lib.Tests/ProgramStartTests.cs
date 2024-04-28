using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class ProgramStartCommandTests
{
    public ProgramStartCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void ProgramStartCommandSuccessTest()
    {
        var _serverCount = 2;
        var barrier = new Barrier(_serverCount + 1);
        var consoleInput = new StringReader("Space");
        Console.SetIn(consoleInput);
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var command = new Mock<ICommand>();
        command.Setup(x => x.Execute());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.MakeBarrier", (object[] args) =>
        {
            return barrier;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start", (object[] args) =>
        {
            return command.Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Stop", (object[] args) =>
        {
            barrier.RemoveParticipants(_serverCount);
            return command.Object;
        }).Execute();

        var maincmd = new ProgramStart(_serverCount);
        maincmd.Execute();
        var output = consoleOutput.ToString();

        Assert.Contains($"Запуск {_serverCount} поточного сервера.", output);
        Assert.Contains($"Поточный сервер {_serverCount} успешно запущен.", output);
        Assert.Contains($"Остановка поточного сервера {_serverCount}...", output);
        Assert.Contains($"Поточный сервер {_serverCount} остановлен.", output);
    }
}

