using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class StartThread : IStrategy
    {
        public object RunStrategy(params object[] args)
        {
            var senderDict = IoC.Resolve<ConcurrentDictionary<string, ISender>>("SenderDictionary");
            var threadDict = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("ThreadDictionary");
            var newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
            var queue = new BlockingCollection<ICommand>(100);
            var sender = new Sender(queue);
            if (args.Length > 1)
            {
                sender.Send(new ActionCommand((Action)args[1]));
            }

            var reciever = new Reciever(queue);
            var st = new ServerThread(reciever, newScope);
            st.Start();
            senderDict.TryAdd((string)args[0], sender);
            threadDict.TryAdd((string)args[0], st);
            return st;
        }
    }
}
