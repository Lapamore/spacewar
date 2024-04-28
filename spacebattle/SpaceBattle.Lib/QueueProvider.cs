using Hwdtech;

namespace SpaceBattle.Lib
{
    public class QueueProvider : IStrategy
    {
        public object Invoke(params object[] args)
        {
            int gameID = (int)args[0];

            Queue<ICommand>? queue;

            if (IoC.Resolve<IDictionary<int, Queue<ICommand>>>("Queue.Mapping").TryGetValue(gameID, out queue))
            {
                return queue;
            }

            throw new Exception();
        }
    }
}
