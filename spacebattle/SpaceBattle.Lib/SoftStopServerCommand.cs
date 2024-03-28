using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        var dictOfThreads = IoC.Resolve<ConcurrentDictionary<int, BlockingCollection<ICommand>>>("Server.Take.Threads");

        dictOfThreads.ToList().ForEach(keyValuePair =>
        {
            IoC.Resolve<ICommand>("Server.SendCommand", keyValuePair.Key,
                                IoC.Resolve<ICommand>("Server.SoftStopCommand",
                                                    IoC.Resolve<Action>("Server.Action")))
            .Execute();
        });
    }
}
