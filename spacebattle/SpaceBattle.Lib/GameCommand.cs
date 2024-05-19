using System.Diagnostics;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class GameCommand : ICommand
    {
        private readonly object _scope;
        private readonly Queue<ICommand> _queue;
        public GameCommand(object scope, Queue<ICommand> queue)
        {
            _scope = scope;
            _queue = queue;
        }
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
            var time = new Stopwatch();
            var timer = IoC.Resolve<TimeSpan>("GameQuantum");
            time.Start();
            while (_queue.Count != 0 && time.Elapsed < timer)
            {
                var cmd = _queue.Dequeue();
                try
                {
                    cmd.Execute();
                }
                catch (Exception exc)
                {
                    IoC.Resolve<IHandler>("GameExceptionHandler", cmd, exc).Handle();
                }
            }
            time.Stop();
        }
    }
}