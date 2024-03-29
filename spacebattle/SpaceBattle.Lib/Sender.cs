using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class Sender : ISender
{
    private readonly BlockingCollection<ICommand> queue;
    public Sender(BlockingCollection<ICommand> _queue)
    {
        queue = _queue;
    }
    public void Send(ICommand command)
    {
        queue.Add(command);
    }
}
