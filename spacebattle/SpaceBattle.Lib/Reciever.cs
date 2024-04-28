using System.Collections.Concurrent;
namespace SpaceBattle.Lib;

public class Reciever : IReciever
{
    private readonly BlockingCollection<ICommand> _commands;
    public Reciever(BlockingCollection<ICommand> _queue)
    {
        _commands = _queue;
    }
    public ICommand Recieve()
    {
        return _commands.Take();
    }
    public bool IsEmpty()
    {
        return _commands.Count() == 0;
    }
}
