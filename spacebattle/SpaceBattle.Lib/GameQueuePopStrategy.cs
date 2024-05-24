using Hwdtech;

namespace SpaceBattle.Lib
{
    public class GameQueuePopStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            int gameQueueId = (int)args[0];

            var queue = IoC.Resolve<Queue<ICommand>>("Get.Queue", gameQueueId);

            return new ActionCommand(() => { queue.Dequeue(); });
        }
    }
}
