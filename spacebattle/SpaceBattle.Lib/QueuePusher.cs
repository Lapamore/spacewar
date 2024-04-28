using Hwdtech;

namespace SpaceBattle.Lib
{
    public class QueuePusher : IStrategy
    {
        public object Invoke(params object[] args)
        {
            int id = (int)args[0];

            ICommand cmd = (ICommand)args[1];

            var queue = IoC.Resolve<Queue<ICommand>>("Queue.Retriever", id);

            return new ActionCommand(() => { queue.Enqueue(cmd); });
        }
    }
}
