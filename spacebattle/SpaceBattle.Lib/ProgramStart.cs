using Hwdtech;

namespace SpaceBattle.Lib;

public class ProgramStart : ICommand
{
    private readonly int _serverCount;

    public ProgramStart(int serverCount)
    {
        _serverCount = serverCount;
    }

    public void Execute()
    {
        Console.WriteLine($"Запуск {_serverCount} поточного сервера.");
        var barrier = IoC.Resolve<Barrier>("Server.MakeBarrier", _serverCount);
        IoC.Resolve<ICommand>("Server.Start", _serverCount).Execute();
        Console.WriteLine($"Поточный сервер {_serverCount} успешно запущен.");
        Console.ReadLine();
        Console.WriteLine($"Остановка поточного сервера {_serverCount}...");
        IoC.Resolve<ICommand>("Server.Stop").Execute();
        barrier.SignalAndWait();
        Console.WriteLine($"Поточный сервер {_serverCount} остановлен.");
    }
}
