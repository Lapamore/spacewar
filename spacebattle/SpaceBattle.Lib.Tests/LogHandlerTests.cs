using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class LogHandlerTests
{
    public LogHandlerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void LogHandlerSuccessTest()
    {
        var mockCommand = new Mock<ICommand>();
        var exception = new DivideByZeroException();
        var logFilePath = Path.GetTempFileName();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetReportInfo", (object[] args) =>
        {
            return (string)new ErrorLoggingStrategy().Invoke(args);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GetLogFilePath", (object[] args) =>
        {
            return logFilePath;
        }).Execute();

        var handler = new LogHandler(mockCommand.Object, exception);
        handler.Handle();

        var str = File.ReadLines(logFilePath).Last();
        Assert.Contains("При выполнении команды <Castle.Proxies.ICommandProxy> возникло исключение <System.DivideByZeroException>", str);
    }
}
