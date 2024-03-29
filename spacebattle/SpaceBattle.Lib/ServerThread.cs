using Hwdtech;

namespace SpaceBattle.Lib;

public class ServerThread
{
    internal bool stop = false;
    private readonly IReciever _queue;
    private readonly Thread _thread;
    private Action _action;

    public ServerThread(IReciever queue, object newScope)
    {
        _queue = queue;
        _action = new Action(() =>
        {
            ExceptionHandler();
        });
        _thread = new Thread(() =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();
            while (!stop)
            {
                _action.Invoke();
            }
        });
    }
    public void Wait()
    {
        _thread.Join();
    }
    public void Start()
    {
        _thread.Start();
    }
    public void Stop()
    {
        stop = true;
    }
    internal void ExceptionHandler()
    {
        var cmd = _queue.Recieve();
        try
        {
            cmd.Execute();
        }
        catch (Exception e)
        {
            var exceptionCMD = IoC.Resolve<ICommand>("ExceptionHandler", e, cmd);
            exceptionCMD.Execute();
        }
    }
    public void UpdateBehaviour(Action behavior)
    {
        _action = behavior;
    }
    public bool Stopped()
    {
        return stop;
    }
    public bool EmptyQueue()
    {
        return _queue.IsEmpty();
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() == typeof(Thread))
        {
            return _thread == (Thread)obj;
        }

        if (obj.GetType() == typeof(ServerThread))
        {
            return GetHashCode() == obj.GetHashCode();
        }

        return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
