using Hwdtech;

namespace SpaceBattle.Lib;
public class LogHandler : IHandler
{
    private readonly ICommand _cmd;
    private readonly Exception _exception;

    public LogHandler(ICommand cmd, Exception exception)
    {
        _cmd = cmd;
        _exception = exception;
    }

    public void Handle()
    {
        var path = IoC.Resolve<string>("Server.GetLogFilePath");
        var exceptionReport = IoC.Resolve<string>("Server.GetReportInfo", _cmd, _exception);

        using (var sw = new StreamWriter(path))
        {
            sw.WriteLine(exceptionReport);
        }
    }
}
