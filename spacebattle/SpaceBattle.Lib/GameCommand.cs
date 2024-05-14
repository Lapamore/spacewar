using System.Diagnostics;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class GameCommand : ICommand
    {
        object _scope;
        IReciever _rec;
        Stopwatch stopwatch = new Stopwatch();
        public GameCommand(object scope, IReciever rec)
        {
            _scope = scope;
            _rec = rec;
        }

        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
            var gametime = IoC.Resolve<int>("GameQuantum");
            stopwatch.Reset();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds <= gametime && !_rec.IsEmpty())
            {
                var cmd = _rec.Recieve();

                try
                {
                    cmd.Execute();
                }

                catch (Exception ex)
                {
                    IoC.Resolve<ICommand>("ExceptionHandlerGame", cmd, ex).Execute();
                }
            }

            stopwatch.Stop();
        }
    }
}